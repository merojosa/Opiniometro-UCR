CREATE TABLE [dbo].[Seleccion_Unica]
(
	[ItemId] INT NOT NULL,
	IsaLikeDislike BIT,
	CONSTRAINT FK_Sel_Uni_Ite FOREIGN KEY (ItemId) REFERENCES Item(ItemId), 
	CONSTRAINT PK_Seleccion_Unica PRIMARY KEY(ItemId)
)
