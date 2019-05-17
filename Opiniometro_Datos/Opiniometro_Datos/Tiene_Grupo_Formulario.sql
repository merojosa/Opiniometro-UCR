CREATE TABLE [dbo].[Tiene_Grupo_Formulario]
(
	[Numero] TINYINT NOT NULL , 
    [SiglaCurso] CHAR(6) NOT NULL, 
    [Año] CHAR(4) NOT NULL, 
    [Ciclo] TINYINT NOT NULL,
	[Codigo] CHAR(6) NOT NULL,
	[FechaInicio] datetime NOT NULL,
	[FechaFinal] datetime NOT NULL,
	CONSTRAINT FK_Tie_Gru FOREIGN KEY (Numero, SiglaCurso, Año, Ciclo)
	REFERENCES Grupo(Numero, SiglaCurso, Año, Semestre),
	CONSTRAINT FK_Tie_Fec FOREIGN KEY (FechaInicio, FechaFinal) 
	REFERENCES Fecha_Corte(FechaInicio, FechaFinal),
	--CONSTRAINT FK_Tie_For FOREIGN KEY (Codigo) REFERENCES Formulario(CodigoFormulario)
	CONSTRAINT PK_Tiene PRIMARY KEY (Numero, SiglaCurso, Año, Ciclo, Codigo, FechaInicio, FechaFinal)

)
