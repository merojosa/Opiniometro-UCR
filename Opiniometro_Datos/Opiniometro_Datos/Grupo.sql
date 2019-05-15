
CREATE TABLE [dbo].[Grupo]
(
	[Numero] TINYINT NOT NULL , 
    [SiglaCurso] CHAR(6) NOT NULL, 
    [Año] CHAR(4) NOT NULL, 
    [Semestre] TINYINT NOT NULL,
	CONSTRAINT PK_Grupo PRIMARY KEY (Numero,SiglaCurso,Año,Semestre),
	CONSTRAINT FK_Grupo_Curso FOREIGN KEY (SiglaCurso)
	REFERENCES Curso(Sigla), --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT FK_Grupo_Ciclo FOREIGN KEY (Año,Semestre) 
	REFERENCES Ciclo_Lectivo(Año,Semestre)  --ON DELETE CASCADE ON UPDATE CASCADE
)