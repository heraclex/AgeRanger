CREATE TABLE [dbo].[AgeGroup]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[MinAge] bigint NULL,
	[MaxAge] bigint NULL,
	[Description] nvarchar(max) NOT NULL
)

/*
The VARCHAR(MAX) type is a replacement for TEXT. 
The basic difference is that a TEXT type will always store the data in a blob 
whereas the VARCHAR(MAX) type will attempt to store the data directly in the row 
unless it exceeds the 8k limitation and at that point it stores it in a blob.
*/