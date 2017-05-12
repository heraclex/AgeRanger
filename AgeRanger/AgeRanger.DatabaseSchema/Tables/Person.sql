CREATE TABLE [dbo].[Person]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FirstName] varchar(max) NULL,
	[LastName] varchar(max) NULL,
	[Age] bigint NULL
)
