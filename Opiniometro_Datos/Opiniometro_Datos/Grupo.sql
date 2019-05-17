
CREATE TABLE [dbo].[Grupo]
(
	[SiglaCurso] CHAR(6) NOT NULL , 
    [Numero] TINYINT NOT NULL, 
    [AñoGrupo] SMALLINT NOT NULL, 
    [SemestreGrupo] TINYINT NOT NULL,
	CONSTRAINT PK_Grupo PRIMARY KEY ([SiglaCurso],[Numero],[AñoGrupo],[SemestreGrupo]),
	CONSTRAINT FK_Gru_Cur FOREIGN KEY ([SiglaCurso])
	REFERENCES Curso(Sigla), --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT FK_Gru_Cic FOREIGN KEY ([AñoGrupo],[SemestreGrupo]) 
	REFERENCES Ciclo_Lectivo(Año,Semestre)  --ON DELETE CASCADE ON UPDATE CASCADE
)