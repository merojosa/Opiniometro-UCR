CREATE TABLE [dbo].[Tiene_Grupo_Formulario]
(
	[SiglaCurso] CHAR(6) NOT NULL , 
    [Numero] TINYINT NOT NULL, 
    [Año] SMALLINT NOT NULL, 
    [Ciclo] TINYINT NOT NULL,
	[Codigo] CHAR(6) NOT NULL,
	[FechaInicio] datetime NOT NULL,
	[FechaFinal] datetime NOT NULL,
	CONSTRAINT FK_Tie_Gru FOREIGN KEY ([SiglaCurso], [Numero], Año, Ciclo)
	REFERENCES Grupo([SiglaCurso], [Numero], [AñoGrupo], [SemestreGrupo]),
	CONSTRAINT FK_Tie_Fec FOREIGN KEY (FechaInicio, FechaFinal) 
	REFERENCES Fecha_Corte(FechaInicio, FechaFinal),
	--CONSTRAINT FK_Tie_For FOREIGN KEY (Codigo) REFERENCES Formulario(CodigoFormulario)
	CONSTRAINT PK_Tiene PRIMARY KEY ([SiglaCurso], [Numero], Año, Ciclo, Codigo, FechaInicio, FechaFinal)

)
