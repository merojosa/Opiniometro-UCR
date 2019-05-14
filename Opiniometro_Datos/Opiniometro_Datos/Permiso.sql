CREATE TABLE [dbo].[Permiso]
(
	[Id]			TINYINT			NOT NULL,
	[Descripcion]	NVARCHAR(150)	NOT NULL,
	CONSTRAINT PK_Permiso
		PRIMARY KEY (Id)
)
