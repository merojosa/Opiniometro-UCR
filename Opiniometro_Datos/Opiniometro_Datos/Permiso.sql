CREATE TABLE [dbo].[Permiso]
(
	[Id]			SMALLINT		NOT NULL,
	[Descripcion]	NVARCHAR(150)	NOT NULL,
	CONSTRAINT PK_Permiso
		PRIMARY KEY (Id)
)
