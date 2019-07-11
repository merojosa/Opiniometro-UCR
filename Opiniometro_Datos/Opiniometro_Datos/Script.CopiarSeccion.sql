Go
Create Procedure SP_CopiarSeccion @CodFormOrigen char(6), @TituloSec nvarchar(120), @CodFormDest char(6)
AS
	DECLARE @itemId nvarchar(10)
	DECLARE cursorCS CURSOR FOR
		Select ItemId from Conformado_Item_Sec_Form where CodigoFormulario =@CodFormOrigen and TituloSeccion = @TituloSec
	Open cursorCS
	Fetch Next From cursorCS INTO @itemId

	WHILE @@FETCH_STATUS = 0 BEGIN
		Insert Into Conformado_Item_Sec_Form (ItemId, CodigoFormulario, TituloSeccion)
		Values (@itemId, @CodFormDest, @TituloSec)


		FETCH NEXT FROM cursorCS INTO @itemID
	END 

	Insert Into Conformado_For_Sec (CodigoFormulario, TituloSeccion)
	Values (@CodFormDest, @TituloSec)

	CLOSE cursorCS
	DEALLOCATE cursorCS

;
