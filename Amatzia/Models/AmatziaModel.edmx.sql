
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/09/2018 18:40:45
-- Generated from EDMX file: C:\Users\Moshe Katz\Desktop\yarin\Amatzia\Amatzia\Models\AmatziaModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Amatzia];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Recepies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Recepies];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [FirstName] varchar(50)  NULL,
    [LastName] varchar(50)  NULL,
    [DateOfBirth] datetime  NULL,
    [Gender] varchar(15)  NULL,
    [Country] varchar(50)  NULL,
    [UserName] varchar(50)  NOT NULL,
    [Password] varchar(50)  NOT NULL,
    [UserId] int  NOT NULL,
    [IsManager] bit  NOT NULL
);
GO

-- Creating table 'Recepies'
CREATE TABLE [dbo].[Recepies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Ingredients] nvarchar(max)  NOT NULL,
    [Instructions] nvarchar(max)  NOT NULL,
    [Difficulty] int  NULL,
    [UploadDate] datetime  NOT NULL,
    [duration] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [Id] in table 'Recepies'
ALTER TABLE [dbo].[Recepies]
ADD CONSTRAINT [PK_Recepies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------