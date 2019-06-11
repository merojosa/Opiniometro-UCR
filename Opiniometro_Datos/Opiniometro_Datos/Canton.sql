CREATE TABLE [dbo].[Canton]
(
	[NombreProvincia]	VARCHAR(50)	NOT NULL,
	[NombreCanton]		VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Canton
		PRIMARY KEY (NombreProvincia, NombreCanton),
	CONSTRAINT FK_Can_Pro
		FOREIGN KEY (NombreProvincia) REFERENCES Provincia(NombreProvincia)
)
