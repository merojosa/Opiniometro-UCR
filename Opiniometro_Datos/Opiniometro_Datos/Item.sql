CREATE TABLE [dbo].[Item]
(
	[ItemID] INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	Texto_Pregunta varchar (120),
	Categoria varchar(30),
	Tiene_Observacion bit

)
