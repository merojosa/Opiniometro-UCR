CREATE TABLE [dbo].[Imparte]
(
	[CedulaProfesor]	CHAR(9)	NOT NULL,
	[Numero]		TINYINT	NOT NULL,
	[Sigla]				CHAR(6)	NOT NULL,
	[Anno]				SMALLINT	NOT NULL,
	[Semestre]			TINYINT	NOT NULL,
	CONSTRAINT PK_Imparte
		PRIMARY KEY (CedulaProfesor, Numero, Sigla, Anno, Semestre),

	CONSTRAINT FK_Imparte_Profesor
		FOREIGN KEY (CedulaProfesor) REFERENCES Profesor(CedulaProfesor)
			ON DELETE NO ACTION
			ON UPDATE CASCADE,

	CONSTRAINT FK_Imparte_Grupo
		FOREIGN KEY (Sigla, Numero, Anno, Semestre) REFERENCES Grupo(SiglaCurso, Numero, AnnoGrupo, SemestreGrupo)
			ON DELETE NO ACTION
			ON UPDATE CASCADE
)
