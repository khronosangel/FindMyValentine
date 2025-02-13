CREATE TABLE [dbo].[Student] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [StudentName]   VARCHAR (MAX)  NOT NULL,
    [StudentNumber] VARCHAR (MAX)  NOT NULL,
    [Hint] VARCHAR (MAX)  NULL,
    [Gender]        VARCHAR (255)  NOT NULL,
    [Level]         NVARCHAR (255) NOT NULL,
    [Course]        VARCHAR (255)  NOT NULL,
    [Year]          VARCHAR (255)  NOT NULL,
    [Section]       VARCHAR (255)  NOT NULL,
    [CreatedDate]   DATETIME       NULL,
    [ModifiedDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Account] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Username]    NVARCHAR (MAX) NOT NULL,
    [Password]    NVARCHAR (MAX) NOT NULL,
    [DisplayName] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


CREATE TABLE [dbo].[Match] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [StudentNumberMale] VARCHAR (MAX)  NOT NULL,
    [StudentNumberFemale] VARCHAR (MAX)  NOT NULL,
    [Level]         NVARCHAR (255) NOT NULL,
    [Matched]         bit NOT NULL,
    [MaleHint]         VARCHAR (MAX)  NOT NULL,
    [FemaleHint]         VARCHAR (MAX)  NOT NULL,
    [CreatedDate]   DATETIME       NULL,
    [ModifiedDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[MatchControl] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [IsActive]         bit NOT NULL,
    [EndOverride]         bit NOT NULL,
    [StartDateTime]    NVARCHAR (255) NOT NULL,
    [EndtDateTime]    NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

ALTER TABLE [dbo].[Student]
ADD [Hint] VARCHAR (MAX)  NULL