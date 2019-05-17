CREATE TABLE [dbo].[Persona]
(
	[Cedula]	CHAR(9)			NOT NULL,
	[Nombre]	NVARCHAR(50)	NOT NULL,
	[Apellido1]	NVARCHAR(50)	NOT NULL,
	[Apellido2]	NVARCHAR(50)	NOT NULL,
	[Direccion]	NVARCHAR(200)	NOT NULL,
	CONSTRAINT PK_Persona
		PRIMARY KEY	(Cedula)
)
