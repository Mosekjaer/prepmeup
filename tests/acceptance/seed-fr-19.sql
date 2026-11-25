SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

DELETE ol
FROM OrderLines ol
JOIN Orders o ON o.Id = ol.OrderId
JOIN Subscriptions s ON s.Id = o.SubscriptionId
JOIN Customers c ON c.Id = s.CustomerId
WHERE c.KeycloakId LIKE 'fr19-%';

DELETE o
FROM Orders o
JOIN Subscriptions s ON s.Id = o.SubscriptionId
JOIN Customers c ON c.Id = s.CustomerId
WHERE c.KeycloakId LIKE 'fr19-%';

DELETE s
FROM Subscriptions s
JOIN Customers c ON c.Id = s.CustomerId
WHERE c.KeycloakId LIKE 'fr19-%';

DELETE FROM Customers WHERE KeycloakId LIKE 'fr19-%';

DECLARE @now DATETIMEOFFSET = SYSDATETIMEOFFSET();
DECLARE @adult UNIQUEIDENTIFIER = 'dbd55106-cbca-4197-bf2b-e1b21b36e7bd';
DECLARE @child UNIQUEIDENTIFIER = '52f89479-6e11-4698-8602-e7cbe63004ec';

INSERT INTO Customers (Id, KeycloakId, Name, Email, Phone, Street, PostalCode, City) VALUES
  ('f1900000-0000-0000-0000-000000000015', 'fr19-c15', 'FR-19 C15', 'fr19-c15@example.com', NULL, 'Testvej 1', '8000', 'Aarhus C'),
  ('f1900000-0000-0000-0000-000000000014', 'fr19-c14', 'FR-19 C14', 'fr19-c14@example.com', NULL, 'Testvej 1', '8000', 'Aarhus C'),
  ('f1900000-0000-0000-0000-0000000000cf', 'fr19-cp', 'FR-19 CP', 'fr19-cp@example.com', NULL, 'Testvej 1', '8000', 'Aarhus C');

INSERT INTO Subscriptions (Id, CustomerId, StartedAt, Status) VALUES
  ('f1910000-0000-0000-0000-000000000015', 'f1900000-0000-0000-0000-000000000015', DATEADD(day, -351, @now), 0),
  ('f1910000-0000-0000-0000-000000000014', 'f1900000-0000-0000-0000-000000000014', DATEADD(day, -352, @now), 0),
  ('f1910000-0000-0000-0000-0000000000cf', 'f1900000-0000-0000-0000-0000000000cf', DATEADD(day, -352, @now), 1);

INSERT INTO Orders (Id, SubscriptionId, PlannedAt, SentAt, ExpiresAt) VALUES
  ('f1920000-0000-0000-0000-000000000015', 'f1910000-0000-0000-0000-000000000015',
    DATEADD(year, -1, DATEADD(day, 15, @now)), DATEADD(year, -1, DATEADD(day, 15, @now)), DATEADD(day, 15, @now)),
  ('f1920000-0000-0000-0000-000000000014', 'f1910000-0000-0000-0000-000000000014',
    DATEADD(year, -1, DATEADD(day, 14, @now)), DATEADD(year, -1, DATEADD(day, 14, @now)), DATEADD(day, 14, @now)),
  ('f1920000-0000-0000-0000-0000000000cf', 'f1910000-0000-0000-0000-0000000000cf',
    DATEADD(year, -1, DATEADD(day, 14, @now)), DATEADD(year, -1, DATEADD(day, 14, @now)), DATEADD(day, 14, @now));

INSERT INTO OrderLines (Id, OrderId, PackageTypeId, Quantity) VALUES
  ('f1930000-0000-0000-0000-000000000151', 'f1920000-0000-0000-0000-000000000015', @adult, 2),
  ('f1930000-0000-0000-0000-000000000152', 'f1920000-0000-0000-0000-000000000015', @child, 1),
  ('f1930000-0000-0000-0000-000000000141', 'f1920000-0000-0000-0000-000000000014', @adult, 2),
  ('f1930000-0000-0000-0000-000000000142', 'f1920000-0000-0000-0000-000000000014', @child, 1),
  ('f1930000-0000-0000-0000-0000000000c1', 'f1920000-0000-0000-0000-0000000000cf', @adult, 2),
  ('f1930000-0000-0000-0000-0000000000c2', 'f1920000-0000-0000-0000-0000000000cf', @child, 1);

COMMIT TRANSACTION;

SELECT c.Name, s.Status, o.SentAt, o.ExpiresAt
FROM Orders o
JOIN Subscriptions s ON s.Id = o.SubscriptionId
JOIN Customers c ON c.Id = s.CustomerId
WHERE c.KeycloakId LIKE 'fr19-%'
ORDER BY c.Name;
