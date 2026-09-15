---
title: Asp.Net - Deployment to Azure
source: Deploy_to_azure.pdf
course_week: 10-11
topic: Release, deployment og security
---

# ASP.NET — Deployment to Azure

Step-by-step guide (screenshot-based deck) for deploying an ASP.NET Core Web API (example project: `RestExample`) to Azure App Service with an Azure SQL Database, directly from Visual Studio.

## Set up

- Open a free Azure account if you do not have one: https://azure.microsoft.com/en-us/free/
  - Free tier: $200 credit for 30 days, 12 months of popular free products, 25+ always-free products
- Sign in with your AU account: `username@uni.au.dk` (username is your auid with `au` prefixed, e.g. `au12345`)

## Start the Publish wizard

- Right-click on the **project (not the solution)** in Solution Explorer in Visual Studio and select **Publish...**
- Target: choose **Azure** ("Publish your application to the Microsoft cloud")
  - Alternative shown: Docker Container Registry

## Choose specific target

"Which Azure service would you like to use to host your application?" Options:

- **Azure App Service (Windows)** — publish application code to a managed infrastructure that is easy to scale (the one used in this guide)
- Azure App Service (Linux)
- Azure Container Apps (Linux) — scalable containerized apps/microservices on a serverless platform
- Azure App Service Container — publish as Docker image to Azure Container Registry and run on App Service
- Azure Container Registry — publish as Docker image to ACR
- Azure Virtual Machine — manage your own infrastructure

## App Service

- Select subscription (e.g. "Azure subscription 1"). **Remember to log in with your Azure account** (top-right corner of the dialog)
- If no existing instances: click **Create a new instance**
- In "App Service (Windows) — Create new":
  - **Name**: e.g. `RestExample20231129095945`
  - **Subscription name**
  - **Resource group**: e.g. `RestExample20231129095526ResourceGroup (East US)` (create new if needed)
  - **Hosting Plan**: e.g. `RestExample20231129095945Plan (East US, S1)`
  - Click **Create**
- After creating the App Service, go into the Azure Portal at https://portal.azure.com/ — select Home -> App Services and check that it was created
- Visual Studio might say "No instance available" — refresh, wait a few minutes (maybe 5) and it should find it. If not, restart Visual Studio

## API Management

- The wizard step "Enable API consumption for teams, customers, and Logic and Power Apps"
- **Check the "Skip this step" box** — it has nothing to do with REST APIs

## Deployment type

"How would you like to deploy your application?"

- **Publish (generates pubxml file)** — deploys application to target on click of the Publish button (used here)
- CI/CD using GitHub Actions workflows (generates yml file) — deploys automatically on code push to a GitHub repo

## Connect to dependency — Azure SQL Database

- In "Connect to dependency", select **Azure SQL Database** ("Intelligent, scalable, cloud database service") — not the on-premise SQL Server Database
- If no databases exist: click **Create new**
- In "Azure SQL Database — Create new":
  - **Database name**: e.g. `RestExample_db`
  - **Subscription name**, **Resource group**
  - **Database server**: if no server exists in the dropdown — create a new one
  - **Database administrator username** and **password**: required — **make sure you note down your username and password somewhere**
- After creation, select the new database (e.g. `RestExample_db`, server `restexampledbserver`) and continue

## Connection string from the Azure portal

- In the portal: SQL databases -> your database -> **Connection strings**
- Two ADO.NET variants are shown:
  - ADO.NET (Microsoft Entra passwordless authentication):

    ```
    Server=tcp:restexampledbserver.database.windows.net,1433;Initial Catalog=RestExample_db;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication="Active Directory Default";
    ```

  - **ADO.NET (SQL authentication) — use this one in your application** (in Visual Studio in your DbContext; put in your DB password where it says to):

    ```
    Server=tcp:restexampledbserver.database.windows.net,1433;Initial Catalog=RestExample_db;Persist Security Info=False;User ID=zaifrun;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
    ```

## Publish summary page

- The Publish page in Visual Studio shows Hosting info: Subscription, Resource group, Resource name and **Site** — e.g. `https://restexample20231129095945.azurewebsites.net`
  - This is your main web site URL. If you have no actual web app, just APIs, this page will be blank — go to the subpages for your APIs or use Postman
- Service Dependencies show e.g. Azure API Management and **Azure SQL Database: RestExample_db (Connected)** with connection string name (e.g. `myconnection`)

## Publishing and republishing

- Click **Publish**. On success: "Publish succeeded" with an **Open site** link
- Settings shown: Configuration = Release, Target Framework (e.g. net6.0), Deployment Mode = Framework-dependent, Target Runtime = Portable
- **You can always republish if you have made changes to your application**
- Under **More actions** you can Edit, Rename, Delete, Restore or Preview the publish profile — use **Edit** if you need to modify settings later
