CREATE TABLE [dbo].[Persona]
(
	[Cedula]	CHAR(9)			NOT NULL,
	[Nombre1]	NVARCHAR(50)	NOT NULL,
	[Nombre2]	NVARCHAR(50)	NULL,
	[Apellido1]	NVARCHAR(50)	NOT NULL,
	[Apellido2]	NVARCHAR(50)	NOT NULL,
	CONSTRAINT PK_Persona
		PRIMARY KEY	(Cedula)
)
