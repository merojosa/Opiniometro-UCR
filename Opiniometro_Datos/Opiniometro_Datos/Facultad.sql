

CREATE TABLE [dbo].[Facultad]
(
	[CodigoUnidadAcademica] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Facultad PRIMARY KEY (CodigoUnidadAcademica),
	CONSTRAINT FK_Fac_Uni FOREIGN KEY (CodigoUnidadAcademica)
	REFERENCES Unidad_Academica(Codigo)  --ON DELETE CASCADE ON UPDATE CASCADE
)
