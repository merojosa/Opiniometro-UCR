
CREATE TABLE [dbo].[Carrera]
(
	[Sigla] NVARCHAR(10) NOT NULL, 
    [Nombre] NVARCHAR(125) NOT NULL, 
    [CodigoUnidadAcademica] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Carrera PRIMARY KEY (Sigla),
	CONSTRAINT FK_Car_Uni FOREIGN KEY (CodigoUnidadAcademica) 
	REFERENCES Unidad_Academica(Codigo) --ON DELETE CASCADE ON UPDATE CASCADE
)
