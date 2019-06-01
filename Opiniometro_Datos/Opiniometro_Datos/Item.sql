CREATE TABLE [dbo].[Item]
(
	[ItemId] NVARCHAR(10) NOT NULL PRIMARY KEY,
	TextoPregunta varchar (120)	NOT NULL,
	Categoria varchar(30),
	TieneObservacion BIT,
	TipoPregunta TINYINT NOT NULL,
	NombreCategoria NVARCHAR(20) NOT NULL,
	CONSTRAINT FK_Ite_Cat FOREIGN KEY(NombreCategoria) REFERENCES Categoria(NombreCategoria)  

)
