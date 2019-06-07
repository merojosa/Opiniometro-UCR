CREATE TABLE [dbo].[Se_Encuentra_Curso_Enfasis]
(
	[SiglaCurso] CHAR(6) NOT NULL ,
	[CodigoEnfasis] TINYINT NOT NULL , 
    [SiglaCarrera] NVARCHAR(10) NOT NULL,
	CONSTRAINT FK_SeE_Cur FOREIGN KEY (SiglaCurso) REFERENCES Curso(Sigla),
	CONSTRAINT FK_SeE_Enf FOREIGN KEY (CodigoEnfasis, SiglaCarrera) REFERENCES Enfasis(Numero, SiglaCarrera),
	CONSTRAINT PK_SeE_Curso_Enfasis PRIMARY KEY (SiglaCurso, CodigoEnfasis, SiglaCarrera)
)
