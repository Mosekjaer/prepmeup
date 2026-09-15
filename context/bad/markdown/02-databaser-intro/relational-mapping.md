---
title: "SW4BAD: Relational Mapping (ER-to-Relational Transformation)"
source: "SW4BAD - Relational Mapping.pdf"
course_week: 2
topic: Databaser intro + datamodellering
---

# Relational Mapping — Transform ER to SQL Tables

Part of the database lifecycle's logical design ("Transform to SQL tables" step).

## Steps

- Have a final ER diagram!
- Transform each entity into a table
- Transform every many-to-many binary or binary recursive relationship into a table
- Transform every ternary or higher-level n-ary relationship into a table

## Transform each entity into a table

The resulting tables fit the following cases:

1. **SQL table with the same information content as the original entity** from which it is derived.
   - Ex: binary many-to-many, one-to-many, one-to-one, ...
2. **SQL table with the embedded foreign key of the parent entity.**
   - Ex: binary one-to-many; one-to-one for one of the entities; binary relationships
3. **SQL table (AKA junction table) derived from a relationship**, containing the foreign keys of all the entities in the relationship.
   - Ex: ternary, binary many-to-many, binary recursive many-to-many

The slides then present the standard mapping catalogue (from the DMaD book) as exercises: compare the mappings, identify primary/foreign keys, note how optionals are handled. The catalogue:

## One-to-one binary relationships

**(a) One-to-one, both entities mandatory.** `Report 1 — has-abbr — 1 Abbreviation`. "Every report has one abbreviation, and every abbreviation represents exactly one report." The child embeds a `not null unique` foreign key:

```sql
create table report
    (report_no integer,
     report_name varchar(256),
     primary key (report_no));
create table abbreviation
    (abbr_no char(6),
     report_no integer not null unique,
     primary key (abbr_no),
     foreign key (report_no) references report
         on delete cascade on update cascade);
```

**(b) One-to-one, one entity optional, one mandatory.** `Department 1 ○— managed-by — 1 Employee`. "Every department must have a manager, but an employee can be a manager of at most one department." The mandatory side embeds the FK:

```sql
create table department
    (dept_no integer,
     dept_name char(20),
     mgr_id char(10) not null unique,
     primary key (dept_no),
     foreign key (mgr_id) references employee
         on delete set default on update cascade);
create table employee
    (emp_id char(10),
     emp_name char(20),
     primary key (emp_id));
```

**(c) One-to-one, both entities optional.** `Engineer 1 ○— has-allocated —○ 1 Desktop`. "Some desktop computers are allocated to engineers, but not necessarily to all engineers." The FK is nullable:

```sql
create table engineer
    (emp_id char(10),
     desktop_no integer,
     primary key (emp_id));
create table desktop
    (desktop_no integer,
     emp_id char(10),
     primary key (desktop_no),
     foreign key (emp_id) references engineer
         on delete set null on update cascade);
```

## One-to-many binary relationships

**(d) One-to-many, both entities mandatory.** `Department 1 — has — N Employee`. "Every employee works in exactly one department, and each department has at least one employee." The N side embeds a `not null` FK:

```sql
create table department
    (dept_no integer,
     dept_name char(20),
     primary key (dept_no));
create table employee
    (emp_id char(10),
     emp_name char(20),
     dept_no integer not null,
     primary key (emp_id),
     foreign key (dept_no) references department
         on delete set default on update cascade);
```

**(e) One-to-many, one entity optional, one mandatory.** `Department 1 ○— publishes — N Report`. "Each department publishes one or more reports. A given report may not necessarily be published by a department." The FK is nullable:

```sql
create table department
    (dept_no integer,
     dept_name char(20),
     primary key (dept_no));
create table report
    (report_no integer,
     dept_no integer,
     primary key (report_no),
     foreign key (dept_no) references department
         on delete set null on update cascade);
```

## Many-to-many binary relationships

**(f) Many-to-many, both entities optional.** `Engineer N ○— belongs-to —○ N Prof-assoc`. "Every professional association could have none, one, or many engineer members. Each engineer could be a member of none, one, or many professional associations." Mapped to a **junction table** whose primary key is the combination of the two foreign keys — equivalent to two one-to-many relationships:

```sql
create table engineer
    (emp_id char(10),
     primary key (emp_id));
create table prof_assoc
    (assoc_name varchar(256),
     primary key (assoc_name));
create table belongs_to
    (emp_id char(10),
     assoc_name varchar(256),
     primary key (emp_id, assoc_name),
     foreign key (emp_id) references engineer
         on delete cascade on update cascade,
     foreign key (assoc_name) references prof_assoc
         on delete cascade on update cascade);
```

## Binary recursive relationships

**(a) One-to-one, both sides optional.** `Employee ○1 — is-married-to — 1○ Employee`. "Any employee is allowed to be married to another employee in this company." Self-referencing nullable FK:

```sql
create table employee
    (emp_id char(10),
     emp_name char(20),
     spouse_id char(10),
     primary key (emp_id),
     foreign key (spouse_id) references employee
         on delete set null on update cascade);
```

**(b) One-to-many, one side mandatory, many side optional.** `Engineer 1 — is-group-leader-of — N○ Engineer`. "Engineers are divided into groups for certain projects. Each group has a leader." Self-referencing `not null` FK:

```sql
create table engineer
    (emp_id char(10),
     leader_id char(10) not null,
     primary key (emp_id),
     foreign key (leader_id) references engineer
         on delete set default on update cascade);
```

**(c) Many-to-many, both sides optional.** `Employee ○N — is-coauthor-with — N○ Employee`. "Each employee has the opportunity to coauthor a report with one or more other employees, or to write the report alone." Junction table with two FKs to the same table:

```sql
create table employee
    (emp_id char(10),
     emp_name char(20),
     primary key (emp_id));
create table coauthor
    (author_id char(10),
     coauthor_id char(10),
     primary key (author_id, coauthor_id),
     foreign key (author_id) references employee
         on delete cascade on update cascade,
     foreign key (coauthor_id) references employee
         on delete cascade on update cascade);
```

## Ternary relationships

Always mapped to a junction table containing the FKs of all three entities. The primary key of the junction table consists of the keys of the entities on the **"many" sides** (the FDs determine it); "one"-side combinations that must stay unique get `unique` constraints.

**One-to-one-to-one.** `Technician 1 / Project 1 / Notebook 1 — uses-notebook`. "A technician uses exactly one notebook for each project. Each notebook belongs to one technician for each project. A technician may still work on many projects and maintain different notebooks for different projects."

```sql
create table technician (emp_id char(10),
                primary key (emp_id));
create table project (project_name char(20),
                primary key (project_name));
create table notebook (notebook_no integer,
                primary key (notebook_no));
create table uses_notebook (emp_id char(10),
                project_name char(20),
                notebook_no integer not null,
                primary key (emp_id, project_name),
                foreign key (emp_id) references technician
                    on delete cascade on update cascade,
                foreign key (project_name) references project
                    on delete cascade on update cascade,
                foreign key (notebook_no) references notebook
                    on delete cascade on update cascade,
                unique (emp_id, notebook_no),
                unique (project_name, notebook_no));
```

**One-to-many-to-many.** `Manager 1 — assigned-to — N Engineer / N Project`. "Each engineer working on a particular project has exactly one manager, but a project can have many managers; an engineer may have many managers and many projects. A manager may manage several projects."

```sql
create table project (project_name char(20),
                primary key (project_name));
create table manager (mgr_id char(10),
                primary key (mgr_id));
create table engineer (emp_id char(10),
                primary key (emp_id));
create table manages (project_name char(20),
                mgr_id char(10) not null,
                emp_id char(10),
                primary key (project_name, emp_id),
                foreign key (project_name) references project
                    on delete cascade on update cascade,
                foreign key (mgr_id) references manager
                    on delete cascade on update cascade,
                foreign key (emp_id) references engineer
                    on delete cascade on update cascade);
```

**One-to-one-to-many.** `Project 1 — assigned-to — N Employee / 1 Location`. "Each employee assigned to a project works at only one location for that project, but can be at a different location for a different project. At a given location, an employee works on only one project. At a particular location, there can be many employees assigned to a given project."

```sql
create table employee (emp_id char(10),
                emp_name char(20),
                primary key (emp_id));
create table project (project_name char(20),
                primary key (project_name));
create table location (loc_name char(15),
                primary key (loc_name));
create table assigned_to (emp_id char(10),
                project_name char(20),
                loc_name char(15) not null,
                primary key (emp_id, project_name),
                foreign key (emp_id) references employee
                    on delete cascade on update cascade,
                foreign key (project_name) references project
                    on delete cascade on update cascade,
                foreign key (loc_name) references location
                    on delete cascade on update cascade,
                unique (emp_id, loc_name));
```

**Many-to-many-to-many.** `Employee N / Skill N / Project N — skill-used`. "Employees can use different skills on any one of many projects, and each project has many employees with various skills." All three keys form the primary key:

```sql
create table employee (emp_id char(10),
                emp_name char(20),
                primary key (emp_id));
create table skill (skill_type char(15),
                primary key (skill_type));
create table project (project_name char(20),
                primary key (project_name));
create table skill_used (emp_id char(10),
                skill_type char(15),
                project_name char(20),
                primary key (emp_id, skill_type, project_name),
                foreign key (emp_id) references employee
                    on delete cascade on update cascade,
                foreign key (skill_type) references skill
                    on delete cascade on update cascade,
                foreign key (project_name) references project
                    on delete cascade on update cascade);
```

## Generalization and aggregation

Three approaches to mapping a supertype/subtype hierarchy:

1. **Table for the supertype** containing the common attributes; **subtype tables** contain the supertype entity key plus subtype-specific attributes. Requires a foreign key constraint.
2. **One table for each subtype**, pushing the common attributes down into the specific subtypes (null entries).
3. **Single table** that includes all attributes from the supertype and the subtypes.

The same applies for disjoint and overlapping generalizations. Consider update integrity.

Example of approach 1 — overlapping generalization `Individual (o) → Employee, Customer` ("An individual may be either an employee or a customer, or both, or neither"):

```sql
create table individual (indiv_id char(10),
                indiv_name char(20),
                indiv_addr char(20),
                primary key (indiv_id));
create table employee (emp_id char(10),
                job_title char(15),
                primary key (emp_id),
                foreign key (emp_id) references individual
                    on delete cascade on update cascade);
create table customer (cust_no char(10),
                cust_credit char(12),
                primary key (cust_no),
                foreign key (cust_no) references individual
                    on delete cascade on update cascade);
```

Example of approach 3 (single table, T-SQL flavour) — one `Products` table covering both soup- and bread-specific attributes, with nulls where a subtype attribute does not apply:

```sql
CREATE TABLE Products (
    id INT PRIMARY KEY,
    price INT, soup_type VARCHAR(10),
    expiration DATE, bakedOn VARCHAR(120),
    slices int, type VARCHAR(5)
)

INSERT INTO Products (id, price, soup_type, expiration, type)
VALUES (1, 10, 'mushroom', getDate(), 'soup'),
       (2, 15, 'shrimp',  getDate(), 'soup')

INSERT INTO Products (id, price, bakedOn, slices, type)
VALUES (3, 9,  'full',  30, 'bread'),
       (4, 12, 'white', 25, 'bread')
```
