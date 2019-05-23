CREATE TABLE [dbo].[Item]
(
	[ItemId] INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	TextoPregunta varchar (120)	NOT NULL,
	Categoria varchar(30),
	TieneObservacion BIT,
	TipoPregunta TINYINT NOT NULL

)
