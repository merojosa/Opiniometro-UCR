CREATE TABLE [dbo].[Profesor]
(
	[CedulaProfesor]	CHAR(9)	NOT NULL,
	CONSTRAINT PK_Profesor
		PRIMARY KEY (CedulaProfesor),
	CONSTRAINT FK_Pro_Per
		FOREIGN KEY (CedulaProfesor) REFERENCES Persona(Cedula)
			ON DELETE NO ACTION
			ON UPDATE CASCADE
)
