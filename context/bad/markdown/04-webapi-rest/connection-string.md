---
title: Connection String
source: Connection String.html
course_week: 4
topic: Web APIs + REST + Dapper
---

# Connection String

Find your connection string. If you are using Docker, it probably looks something like:

```
Data Source=127.0.0.1,1433;Database=DAB_E22;User Id=sa;Password=Password1;TrustServerCertificate=True
```

If you are using SQL Server via a native installation, your connection string may look like this:

```
Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
```

In that case, here is a guide to find the precise one via Visual Studio:
[Generate or find Connection String from Visual Studio (c-sharpcorner.com)](https://www.c-sharpcorner.com/UploadFile/suthish_nair/how-to-generate-or-find-connection-string-from-visual-studio/)
