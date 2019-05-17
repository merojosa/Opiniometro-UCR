CREATE TABLE [dbo].[TelefonoPersona]
(
	[CedulaPersona]		CHAR(9)		NOT NULL,
	[NumeroTelefono]	VARCHAR(11)	NOT NULL,
	/*CHECK(ISNUMERIC(NumeroTelefono)),*/
	CONSTRAINT PK_TelefonoPersona
		PRIMARY KEY (CedulaPersona, NumeroTelefono),
	CONSTRAINT FK_Tel_Per
		FOREIGN KEY (CedulaPersona) REFERENCES Persona(Cedula)
)
