
CREATE TABLE [dbo].[Grupo]
(
	[SiglaCurso] CHAR(6) NOT NULL , 
    [Numero] TINYINT NOT NULL, 
    [AnnoGrupo] SMALLINT NOT NULL, 
    [SemestreGrupo] TINYINT NOT NULL,
	CONSTRAINT PK_Grupo PRIMARY KEY ([SiglaCurso],[Numero],[AnnoGrupo],[SemestreGrupo]),
	CONSTRAINT FK_Gru_Cur FOREIGN KEY ([SiglaCurso])
	REFERENCES Curso(Sigla), --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT FK_Gru_Cic FOREIGN KEY ([AnnoGrupo],[SemestreGrupo]) 
	REFERENCES Ciclo_Lectivo(Anno,Semestre)  --ON DELETE CASCADE ON UPDATE CASCADE
)