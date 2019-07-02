CREATE TABLE [dbo].[Perfil]
(
	[Nombre]	VARCHAR(30)	NOT NULL, 
	[Descripcion]	VARCHAR(80)	NOT NULL,
	CONSTRAINT PK_Perfil
		PRIMARY KEY ([Nombre])
)
