CREATE TABLE dbo.AuditLog
(
    Id           UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_AuditLog PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    Actor        NVARCHAR(320)    NOT NULL,
    Action       NVARCHAR(64)     NOT NULL,
    ResourceType NVARCHAR(64)     NOT NULL,
    ResourceId   NVARCHAR(64)     NOT NULL,
    OccurredAt   DATETIMEOFFSET   NOT NULL CONSTRAINT DF_AuditLog_Occurred DEFAULT SYSDATETIMEOFFSET(),
    Changes      NVARCHAR(MAX)    NULL CONSTRAINT CK_AuditLog_Changes CHECK (ISJSON(Changes) = 1)
);
GO

CREATE INDEX IX_AuditLog_Resource ON dbo.AuditLog (ResourceType, ResourceId, OccurredAt DESC);
GO
