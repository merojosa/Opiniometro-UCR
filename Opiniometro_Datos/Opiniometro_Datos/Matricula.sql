CREATE TABLE [dbo].[Matricula]
(
	[CedulaEstudiante]	CHAR(9)	NOT NULL,
	[Numero]		TINYINT	NOT NULL,
	[Sigla]				CHAR(6)	NOT NULL,
	[Anno]				CHAR(4)	NOT NULL,
	[Semestre]			TINYINT	NOT NULL,
	CONSTRAINT PK_Matricula
		PRIMARY KEY (CedulaEstudiante, Numero, Sigla, Anno, Semestre),
	CONSTRAINT FK_Matricula_Profesor
		FOREIGN KEY (CedulaEstudiante) REFERENCES Estudiante(CedulaEstudiante)
			ON DELETE NO ACTION
			ON UPDATE CASCADE/*,
			--Lo siguiente tiene dependencias:
	CONSTRAINT FK_Matricula_Grupo
		FOREIGN KEY (Numero, Sigla, Anno, Semestre) REFERENCES Grupo(Numero, Sigla, Anno, Semestre)
			ON DELETE NO ACTION
			ON UPDATE CASCADE*/
)
