﻿CREATE TABLE [dbo].[Student]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Gender] NVARCHAR(50) NOT NULL, 
    [UniversityId] INT NOT NULL, 
    CONSTRAINT [UniversityFK] FOREIGN KEY ([UniversityId]) REFERENCES [dbo].[University]([Id]) ON DELETE CASCADE
)
