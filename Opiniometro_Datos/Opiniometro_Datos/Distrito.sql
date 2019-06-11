CREATE TABLE [dbo].[Distrito]
(
	[NombreProvincia]	VARCHAR(50)	NOT NULL,
	[NombreCanton]		VARCHAR(50) NOT NULL,
	[NombreDistrito]	VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Distrito
		PRIMARY KEY (NombreProvincia, NombreCanton, NombreDistrito),
	CONSTRAINT FK_Dis_Can
		FOREIGN KEY (NombreProvincia, NombreCanton) REFERENCES Canton(NombreProvincia, NombreCanton)
)
