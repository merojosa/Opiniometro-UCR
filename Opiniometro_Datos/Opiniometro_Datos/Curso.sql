
CREATE TABLE [dbo].[Curso]
(
	[Sigla] CHAR(6) NOT NULL , 
    [Nombre] VARCHAR(75) NOT NULL, 
    [Tipo] TINYINT NOT NULL, 
    [CodigoUnidad] NVARCHAR(10) NOT NULL,
	CONSTRAINT PK_Curso PRIMARY KEY (Sigla),
	CONSTRAINT FK_Cur_Uni FOREIGN KEY (CodigoUnidad)
	REFERENCES Unidad_Academica(Codigo)
)