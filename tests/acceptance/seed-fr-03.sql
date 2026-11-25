SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DECLARE @replaced UNIQUEIDENTIFIER = 'f0300000-0000-0000-0000-000000000001';
DECLARE @water UNIQUEIDENTIFIER = '7034103d-0cce-40d5-a06a-345b47276ded';

DELETE FROM PackageContents WHERE PackageTypeId = @replaced;
DELETE FROM PackageTypes WHERE Id = @replaced;

INSERT INTO PackageTypes (Id, Name, Status) VALUES
  (@replaced, 'FR-03 Replaced', 1);

INSERT INTO PackageContents (Id, PackageTypeId, ItemId, Quantity) VALUES
  ('f0310000-0000-0000-0000-000000000001', @replaced, @water, 1);

COMMIT TRANSACTION;

SELECT Name, Status
FROM PackageTypes
ORDER BY Name;
