
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/24/2016 13:37:12
-- Generated from EDMX file: c:\Users\vha\documents\visual studio 2015\Projects\WWWings\WWWings\WWWingsModell.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [tempdb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PassagierFlug_Passagier]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PassagierFlug] DROP CONSTRAINT [FK_PassagierFlug_Passagier];
GO
IF OBJECT_ID(N'[dbo].[FK_PassagierFlug_Flug]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PassagierFlug] DROP CONSTRAINT [FK_PassagierFlug_Flug];
GO
IF OBJECT_ID(N'[dbo].[FK_PilotFlug]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FlugSet] DROP CONSTRAINT [FK_PilotFlug];
GO
IF OBJECT_ID(N'[dbo].[FK_Passagier_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonSet_Passagier] DROP CONSTRAINT [FK_Passagier_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Pilot_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonSet_Pilot] DROP CONSTRAINT [FK_Pilot_inherits_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[PersonSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet];
GO
IF OBJECT_ID(N'[dbo].[FlugSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FlugSet];
GO
IF OBJECT_ID(N'[dbo].[PersonSet_Passagier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet_Passagier];
GO
IF OBJECT_ID(N'[dbo].[PersonSet_Pilot]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonSet_Pilot];
GO
IF OBJECT_ID(N'[dbo].[PassagierFlug]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PassagierFlug];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'PersonSet'
CREATE TABLE [dbo].[PersonSet] (
    [Id] int  NOT NULL,
    [Vorname] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Geburtsdatum] datetime  NOT NULL
);
GO

-- Creating table 'FlugSet'
CREATE TABLE [dbo].[FlugSet] (
    [Id] int  NOT NULL,
    [Abflugort] nvarchar(max)  NOT NULL,
    [Zielort] nvarchar(max)  NOT NULL,
    [Datum] datetime  NOT NULL,
    [Plaetze] smallint  NOT NULL,
    [FreiePlaetze] smallint  NOT NULL,
    [PilotId] int  NOT NULL
);
GO

-- Creating table 'PersonSet_Passagier'
CREATE TABLE [dbo].[PersonSet_Passagier] (
    [PassagierStatus] nvarchar(max)  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PersonSet_Pilot'
CREATE TABLE [dbo].[PersonSet_Pilot] (
    [Einstellungsdatum] datetime  NOT NULL,
    [Id] int  NOT NULL
);
GO

-- Creating table 'PassagierFlug'
CREATE TABLE [dbo].[PassagierFlug] (
    [Passagier_Id] int  NOT NULL,
    [Flug_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'PersonSet'
ALTER TABLE [dbo].[PersonSet]
ADD CONSTRAINT [PK_PersonSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FlugSet'
ALTER TABLE [dbo].[FlugSet]
ADD CONSTRAINT [PK_FlugSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet_Passagier'
ALTER TABLE [dbo].[PersonSet_Passagier]
ADD CONSTRAINT [PK_PersonSet_Passagier]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonSet_Pilot'
ALTER TABLE [dbo].[PersonSet_Pilot]
ADD CONSTRAINT [PK_PersonSet_Pilot]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Passagier_Id], [Flug_Id] in table 'PassagierFlug'
ALTER TABLE [dbo].[PassagierFlug]
ADD CONSTRAINT [PK_PassagierFlug]
    PRIMARY KEY CLUSTERED ([Passagier_Id], [Flug_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Passagier_Id] in table 'PassagierFlug'
ALTER TABLE [dbo].[PassagierFlug]
ADD CONSTRAINT [FK_PassagierFlug_Passagier]
    FOREIGN KEY ([Passagier_Id])
    REFERENCES [dbo].[PersonSet_Passagier]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Flug_Id] in table 'PassagierFlug'
ALTER TABLE [dbo].[PassagierFlug]
ADD CONSTRAINT [FK_PassagierFlug_Flug]
    FOREIGN KEY ([Flug_Id])
    REFERENCES [dbo].[FlugSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PassagierFlug_Flug'
CREATE INDEX [IX_FK_PassagierFlug_Flug]
ON [dbo].[PassagierFlug]
    ([Flug_Id]);
GO

-- Creating foreign key on [PilotId] in table 'FlugSet'
ALTER TABLE [dbo].[FlugSet]
ADD CONSTRAINT [FK_PilotFlug]
    FOREIGN KEY ([PilotId])
    REFERENCES [dbo].[PersonSet_Pilot]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PilotFlug'
CREATE INDEX [IX_FK_PilotFlug]
ON [dbo].[FlugSet]
    ([PilotId]);
GO

-- Creating foreign key on [Id] in table 'PersonSet_Passagier'
ALTER TABLE [dbo].[PersonSet_Passagier]
ADD CONSTRAINT [FK_Passagier_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'PersonSet_Pilot'
ALTER TABLE [dbo].[PersonSet_Pilot]
ADD CONSTRAINT [FK_Pilot_inherits_Person]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[PersonSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------