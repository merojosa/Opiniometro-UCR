CREATE TABLE [dbo].[Formulario]
(
	[CodigoFormulario] char(6) NOT NULL PRIMARY KEY,
	Nombre varchar(25),
	[CodigoUnidadAca] NVARCHAR(10) NOT NULL,
	CONSTRAINT FK_For_Uni FOREIGN KEY(CodigoUnidadAca) REFERENCES Unidad_Academica(Codigo)
	ON UPDATE CASCADE
)
