-- Initial schema for the Mandat platform.
CREATE TABLE dbo.Mentors
(
    Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Mentors PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    DisplayName     NVARCHAR(200)    NOT NULL,
    Email           NVARCHAR(320)    NOT NULL,
    PhoneNumber     NVARCHAR(32)     NULL,
    Biography       NVARCHAR(MAX)    NULL,
    HourlyRate      DECIMAL(10, 2)   NOT NULL CONSTRAINT CK_Mentors_Rate CHECK (HourlyRate >= 0),
    AcceptsOnline   BIT              NOT NULL CONSTRAINT DF_Mentors_Online DEFAULT 1,
    AcceptsInPerson BIT              NOT NULL CONSTRAINT DF_Mentors_InPerson DEFAULT 0,
    CreatedAt       DATETIMEOFFSET   NOT NULL CONSTRAINT DF_Mentors_Created DEFAULT SYSDATETIMEOFFSET(),
    DeletedAt       DATETIMEOFFSET   NULL
);
GO

CREATE UNIQUE INDEX UX_Mentors_Email ON dbo.Mentors (Email) WHERE DeletedAt IS NULL;
GO
