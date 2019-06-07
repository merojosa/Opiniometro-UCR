CREATE TABLE [dbo].[Escuela]
(
	[CodigoUnidadAcademica] NVARCHAR(10) NOT NULL , 
    [CodigoFacultad] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Escuela PRIMARY KEY (CodigoUnidadAcademica),
	CONSTRAINT FK_Esc_Uni FOREIGN KEY (CodigoUnidadAcademica)
	REFERENCES Unidad_Academica(Codigo),  --ON DELETE CASCADE ON UPDATE CASCADE
	CONSTRAINT FK_Esc_Fac FOREIGN KEY (CodigoFacultad) 
	REFERENCES Facultad(CodigoUnidadAcademica)  --ON DELETE CASCADE ON UPDATE CASCADE
)