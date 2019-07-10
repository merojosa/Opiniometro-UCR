GO 
CREATE TRIGGER [dbo].[TR_AsignarPreguntas] ON [dbo].[Conformado_Item_Sec_Form]
AFTER INSERT 
AS 
BEGIN
	set implicit_transactions off --------------------------------------------------------------Transaccion revisar-------------------------------------------------------------------
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE --------------------------------------------------------------Transaccion revisar-------------------------------------------------------------------

	DECLARE @Titulo nvarchar(120)
	DECLARE @Codigo CHAR(6)
	DECLARE @IdPregunta nvarchar(10)
	DECLARE @OrdenItem int
	DECLARE @MaxPregunta int 

	DECLARE Cursor_Pregunta CURSOR FOR--Declaramos nuestro cursor. 
	SELECT ItemId, CodigoFormulario, TituloSeccion, Orden_Item
	FROM inserted
	OPEN Cursor_Pregunta 
	FETCH NEXT FROM Cursor_Pregunta into @IdPregunta, @Codigo, @Titulo, @OrdenItem
	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN TRANSACTION transaccionAnnadirPregunta
			IF(SELECT MAX(Orden_Item)--Comprobamos si la seccion tiene una pregunta en el formulario. 
			   FROM Conformado_Item_Sec_Form c
			   WHERE   c.TituloSeccion = @Titulo and c.CodigoFormulario = @Codigo ) IS NULL
			   BEGIN--Añadimos la seccion con el 
					UPDATE Conformado_Item_Sec_Form 
					SET Orden_Item = 1 
					WHERE ItemId = @IdPregunta AND CodigoFormulario = @Codigo and TituloSeccion = @Titulo
			   END
			   ELSE--Sino quiere decir que 
			   BEGIN
					SELECT @MaxPregunta = MAX(cf.Orden_Item)--recuperamos el orden de la pregunta más grande que hay.
					FROM Conformado_Item_Sec_Form CF
					WHERE CF.CodigoFormulario = @Codigo AND CF.TituloSeccion = @Titulo

					UPDATE Conformado_Item_Sec_Form 
					SET Orden_Item = @MaxPregunta + 1
					WHERE ItemId = @IdPregunta AND CodigoFormulario = @Codigo and TituloSeccion = @Titulo
			   END 
		COMMIT TRANSACTION transaccionAnnadirPregunta
		FETCH NEXT FROM Cursor_Pregunta into @IdPregunta, @Codigo, @Titulo, @OrdenItem 
	END
	--Si es la primera pregunta que se va a añadir a la seccion. 
	CLOSE Cursor_Pregunta
	DEALLOCATE Cursor_Pregunta
END 
GO