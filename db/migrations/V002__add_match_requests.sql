CREATE TABLE dbo.MatchRequests
(
    Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_MatchRequests PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    StudentId   UNIQUEIDENTIFIER NOT NULL,
    MentorId    UNIQUEIDENTIFIER NOT NULL,
    Status      TINYINT          NOT NULL CONSTRAINT DF_MatchRequests_Status DEFAULT 0,
    RequestedAt DATETIMEOFFSET   NOT NULL CONSTRAINT DF_MatchRequests_Requested DEFAULT SYSDATETIMEOFFSET(),
    AnsweredAt  DATETIMEOFFSET   NULL,

    CONSTRAINT FK_MatchRequests_Mentors FOREIGN KEY (MentorId)
        REFERENCES dbo.Mentors (Id) ON DELETE CASCADE,
    CONSTRAINT CK_MatchRequests_Status CHECK (Status IN (0, 1, 2))
);
GO

CREATE UNIQUE INDEX UX_MatchRequests_Pair ON dbo.MatchRequests (StudentId, MentorId);
GO
