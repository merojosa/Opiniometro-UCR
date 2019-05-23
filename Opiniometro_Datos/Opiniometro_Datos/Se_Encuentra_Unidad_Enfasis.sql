CREATE TABLE [dbo].[Se_Encuentra_Unidad_Enfasis]
(
	[CodigoUnidadAcademica] NVARCHAR(10) NOT NULL,
	[NumeroEnfasis] TINYINT NOT NULL , 
    [SiglaCarrera] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Se_Encuentra_Unidad_Enfasis 
	PRIMARY KEY (CodigoUnidadAcademica, NumeroEnfasis, SiglaCarrera),
	CONSTRAINT FK_Se_Encuentra_Uni_Enf_UniAcad FOREIGN KEY (CodigoUnidadAcademica)
	references Unidad_Academica (Codigo),
	CONSTRAINT FK_Se_Encuentra_Uni_Enf_Endasis FOREIGN KEY (NumeroEnfasis, SiglaCarrera)
	references Enfasis (Numero, SiglaCarrera)
)
