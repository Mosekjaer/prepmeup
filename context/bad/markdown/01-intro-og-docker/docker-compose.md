---
title: Docker Compose — Multi-container applications
source: Docker Compose.pdf
course_week: 1
topic: Intro til backend + Docker
---

# Docker Compose — Multi-container applications

## Why multi-container applications?

- There's a good chance you'd have to scale APIs and front-ends differently than databases.
- Separate containers let you version and update versions in isolation.
- While you may use a container for the database locally, you may want to use a managed service for the database in production — you don't want to ship your database engine with your app then.
- Running multiple processes in one container would require a process manager (the container only starts one process), which adds complexity to container startup/shutdown.

A multi-container app example: one container for the web server, one for the database.

## What is Docker Compose?

- Compose is a tool for defining, sharing, and running multi-container applications.
- Compose files are defined using YAML.
- The three-step process:
  1. Define the application environment with a Dockerfile.
  2. Define the services that make up the application in `compose.yml`.
  3. Run `docker compose up` to start the entire application.
- Gives a concise environment description.

## Common use cases

- **Development environments** — concise environment description, isolated environment.
- **Automated testing environments** — set up the environment for automated test suites; create and destroy isolated testing environments.

## Compose file structure

The Compose file is a YAML file defining:

- Services (required)
- Networks
- Volumes
- Configs
- Secrets

The default path for a Compose file is `compose.yaml` (preferred) or `compose.yml` placed in the working directory. Compose also supports `docker-compose.yaml` and `docker-compose.yml` for backwards compatibility with earlier versions.

## Compose file example

File name: `compose.yaml`

```yaml
services:
  api:
    build:
      dockerfile: Dockerfile
    ports:
      - "6000:8080"
    depends_on:
      - db
  db:
    image: mcr.microsoft.com/mssql/server
    user: root
    volumes:
      - hello-compose:/var/opt/mssql/data
    environment:
      MSSQL_SA_PASSWORD: "suchSecureVeryWordSoPassW0w!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
volumes:
  hello-compose:
    name: hello-compose-db
```

## Services top-level element

- A service is an abstract definition of a computing resource within an application which can be scaled or replaced independently from other components.
- Services are backed by a set of containers, run by the platform according to replication requirements and placement constraints.
- As services are backed by containers, they are defined by a Docker image and a set of runtime arguments. All containers within a service are identically created with these arguments.
- A Compose file must declare a `services` top-level element as a map whose keys are string representations of service names, and whose values are service definitions.
- A service definition contains the configuration that is applied to each service container.

## Docker Compose commands

```bash
docker compose up -d      # start the application stack (-d = run in background)
docker compose down       # stop and remove containers, networks
docker compose build      # build or rebuild services
docker compose --help     # further info
```

If you change a service's Dockerfile or the contents of its build directory, run `docker compose build` to rebuild it.

## Docker Compose Watch

- Automatically updates and previews your running Compose services as you edit and save your code.
- For many projects this enables a hands-off development workflow once Compose is running, as services automatically update themselves when you save your work.
- Example: when running `docker compose watch`, a container for the web service is launched using an image built from the Dockerfile in the project's root. The web service runs `npm start` as its command, launching a development version of the application with Hot Module Reload enabled in the bundler. Whenever a source file in the `web/` directory is changed, Compose syncs the file to the corresponding location under `/src/web`.
- You can use this feature with `dotnet watch`.

```bash
docker compose up --watch
```

## Environments

- Plugging app configuration into the Compose file lets you use the same Docker images in different ways and be explicit about the settings for each environment.
- You can have separate Compose files for your development and test environments, publishing different ports and triggering different features of the app.

```yaml
db:
  image: mcr.microsoft.com/mssql/server
  user: root
  volumes:
    - hello-compose:/var/opt/mssql/data
  environment:
    MSSQL_SA_PASSWORD: "suchSecureVeryWordSoPassW0w!"
    ACCEPT_EULA: "Y"
  ports:
    - "1433:1433"
```

## How to wait for MSSQL in Docker Compose?

Use a healthcheck on the database service and `depends_on` with `condition: service_healthy`:

```yaml
services:
  sqlserver:
    container_name: db_service
    image: microsoft/mssql-server-linux:2022-latest
    healthcheck:
      test: /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "suchSecureVeryWordSoPassW0w!" -Q "SELECT 1" -b -o /dev/null
      interval: 1m
      timeout: 3s
      retries: 5
      start_period: 10m
      start_interval: 10s

  my_service:
    container_name: my_service_container
    build:
      dockerfile: Dockerfile
    depends_on:
      sql.data:
        condition: service_healthy
```

See also https://docs.docker.com/compose/startup-order/

## Container networking

- Containers run in isolation and don't know anything about other processes or containers on the same machine.
- To allow one container to talk to another, use networking: if you place two containers on the same network, they can talk to each other.

## Docker Compose network

- By default, Compose sets up a single network for your app.
- Each container for a service joins the default network and is both reachable by other containers on that network, and discoverable by them at a **hostname identical to the container/service name**.
- Your app's network is given a name based on the "project name", which is based on the name of the directory it lives in. You can override the project name with the `--project-name` flag. Often, we don't care about the network name.

### Network example (from the compose.yaml above)

Diagram (described): on the computer, both `db` and `api` containers sit on the shared Compose network. Each has a port mapping out to the host (`6000:8080` for api, `1433:1433` for db). Inside the Compose network the api reaches the database by the service name `db` as hostname, e.g. in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=db;Database= ..."
}
```

## Custom networks example

The example defines two custom networks. The `proxy` service is isolated from the `db` service because they do not share a network in common — only `app` can talk to both.

```yaml
services:
  proxy:
    build: ./proxy
    networks:
      - frontend
  app:
    build: ./app
    networks:
      - frontend
      - backend
  db:
    image: postgres
    networks:
      - backend

networks:
  frontend:
    # Use a custom driver
    driver: custom-driver-1
  backend:
    # Use a custom driver which takes special options
    driver: custom-driver-2
    driver_opts:
      foo: "1"
```

## Orchestration

Tools to manage, scale, and maintain containerized applications in clusters and/or clouds are called **orchestrators**. Popular tools:

- Kubernetes
- OpenShift (built on top of Kubernetes)
- Docker Swarm
- Rancher

This is not in scope for SW4BAD — the department offers the course Web Architecture and Orchestration (SWWAO).

## Troubleshooting

- Error: "Network error: connection to database" — can sometimes be solved by deleting all containers and starting them again.
- Error: the database is not created on the SQL server — run `dbcontext.Migrate()` in `Program.cs`, or use `context.Database.EnsureCreated();`.

## References & links

- Docker Compose overview: https://docs.docker.com/compose/
- Multi container apps: https://docs.docker.com/get-started/07_multi_container/
- Use Docker Compose: https://docs.docker.com/get-started/08_using_compose/
- Compose file (services): https://docs.docker.com/compose/compose-file/05-services/
- Startup order: https://docs.docker.com/compose/startup-order/
- Hosting ASP.NET Core images with Docker Compose over HTTPS: https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https
