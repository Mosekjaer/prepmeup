-- 01 Start up, add table, seed users
CREATE TABLE Users
(
   Id INT IDENTITY(1,1),
   [Name] NVARCHAR(200) NOT NULL,
   Email NVARCHAR(200) NOT NULL
)

INSERT INTO USERS
VALUES 
	('Jacob',	'jba@ece.au.dk'),
	('Poul Ejnar', 'per@ece.au.dk'),
	('Jørn Martin', 'haj@ece.au.dk')