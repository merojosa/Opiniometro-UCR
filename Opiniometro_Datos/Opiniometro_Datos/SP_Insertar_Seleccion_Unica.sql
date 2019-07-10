CREATE PROCEDURE [dbo].[SP_Insertar_Seleccion_Unica]
	@itemid NVARCHAR(10),
	@isalikedislike bit
AS
BEGIN 
	SET IMPLICIT_TRANSACTIONS OFF; 
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	BEGIN TRANSACTION Insertar_Seleccion_Unica;
		INSERT INTO Seleccion_Unica( ItemId, IsaLikeDislike )
		VALUES	( @itemid, @isalikedislike );
	COMMIT TRANSACTION Insertar_Seleccion_Unica;
END 