CREATE TABLE [dbo].[Estudiante]
(
	[CedulaEstudiante]	CHAR(9)			NOT NULL,
	[Carne]				CHAR(6)	UNIQUE	NOT NULL
	CONSTRAINT PK_Estudiante
		PRIMARY KEY (CedulaEstudiante),
		CONSTRAINT FK_Est_Per
		FOREIGN KEY (CedulaEstudiante) REFERENCES Persona(Cedula)
			ON DELETE NO ACTION
			ON UPDATE CASCADE
)
