CREATE TABLE [dbo].[Empadronado]
(
	[CedulaEstudiante]	CHAR(9)	NOT NULL,
	[NumeroEnfasis]	TINYINT			NOT NULL,
	[SiglaCarrera]	NVARCHAR(10)	NOT NULL,
	CONSTRAINT PK_Empadronado
		PRIMARY KEY (CedulaEstudiante, NumeroEnfasis, SiglaCarrera),
	CONSTRAINT FK_Empadronado_Estudiante
		FOREIGN KEY (CedulaEstudiante) REFERENCES Estudiante(CedulaEstudiante)
			ON DELETE NO ACTION
			ON UPDATE CASCADE/*,
			--Lo siguiente tiene dependencias:
	CONSTRAINT FK_Empadronado_Enfasis
		FOREIGN KEY (NumeroEnfasis, SiglaCarrera) REFERENCES Enfasis(Numero, SiglaCarrera)
			ON DELETE NO ACTION
			ON UPDATE CASCADE*/
)
