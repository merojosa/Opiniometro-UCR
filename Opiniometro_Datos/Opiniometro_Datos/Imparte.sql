CREATE TABLE [dbo].[Imparte]
(
	[CedulaProfesor]	CHAR(9)	NOT NULL,
	[Numero]		TINYINT	NOT NULL,
	[Sigla]				CHAR(6)	NOT NULL,
	[Anno]				CHAR(4)	NOT NULL,
	[Semestre]			TINYINT	NOT NULL,
	CONSTRAINT PK_Imparte
		PRIMARY KEY (CedulaProfesor, Numero, Sigla, Anno, Semestre),
	CONSTRAINT FK_Imparte_Profesor
		FOREIGN KEY (CedulaProfesor) REFERENCES Profesor(CedulaProfesor)
			ON DELETE NO ACTION
			ON UPDATE CASCADE/*,
			--Lo siguiente tiene dependencias:
	CONSTRAINT FK_Imparte_Grupo
		FOREIGN KEY (Numero, Sigla, Anno, Semestre) REFERENCES Grupo(Numero, Sigla, Año, Semestre)
			ON DELETE NO ACTION
			ON UPDATE CASCADE*/
)
