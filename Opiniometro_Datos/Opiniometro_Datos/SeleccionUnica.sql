CREATE TABLE [dbo].[SeleccionUnica]
(
	[ItemId] INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	IsaLikeDislike BIT,

	CONSTRAINT FK_Sel_Ite FOREIGN KEY (ItemId) REFERENCES Item(ItemId)
)
