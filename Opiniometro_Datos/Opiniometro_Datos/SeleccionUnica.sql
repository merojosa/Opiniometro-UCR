CREATE TABLE [dbo].[SeleccionUnica]
(
	[ItemID] INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	IsaLikeDislike bit

	CONSTRAINT FK_Sel_Ite FOREIGN KEY (ItemId) REFERENCES Item(ItemID)
)
