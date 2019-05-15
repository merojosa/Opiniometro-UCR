CREATE TABLE [dbo].[Escuela]
(
	[CodigoUnidadAcademica] NVARCHAR(10) NOT NULL , 
    [CodigoFacultad] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Escuela PRIMARY KEY (CodigoUnidadAcademica),
	CONSTRAINT FK_Escu_UniAcad FOREIGN KEY (CodigoUnidadAcademica)
	REFERENCES Unidad_Academica(Codigo),  --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT FK_Escu_Facu FOREIGN KEY (CodigoFacultad) 
	REFERENCES Facultad(CodigoUnidadAcademica)  --ON DELETE CASCADE ON UPDATE CASCADE
)