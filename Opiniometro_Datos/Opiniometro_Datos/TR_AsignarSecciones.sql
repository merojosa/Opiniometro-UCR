CREATE TRIGGER [dbo].[TR_AsignarSecciones] ON [dbo].[Conformado_For_Sec]
AFTER INSERT
AS 
BEGIN 
	set implicit_transactions off;--------------------------------------------------------------Transaccion revisar-------------------------------------------------------------------
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE; --------------------------------------------------------------Transaccion revisar-------------------------------------------------------------------

	DECLARE @Titulo nvarchar(120)--titulo de la seccion
	DECLARE @Codigo CHAR(6) -- codigo del formulario
	DECLARE @OrdenSeccion int
	DECLARE @MaxSeccion int 
	DECLARE Cursor_Seccion CURSOR FOR--Declaramos nuestro cursor. 
	SELECT CodigoFormulario, TituloSeccion, Orden_Seccion
	FROM inserted

	OPEN Cursor_Seccion
	FETCH NEXT FROM Cursor_Seccion into @Codigo, @Titulo, @OrdenSeccion
	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN TRANSACTION transaccionAnnadirPregunta;
			IF(SELECT MAX(Orden_Seccion)--Comprobamos cual es la seccion que tiene el numero de indice mas grande.
			FROM Conformado_For_Sec c) IS NULL
			BEGIN--Si la seccion es la primera que debemos añadir. 
				--I			
				UPDATE Conformado_For_Sec -- La metemos como la seccion numero 1 
				SET Orden_Seccion = 1
				WHERE CodigoFormulario = @Codigo AND TituloSeccion = @Titulo			
			END
			ELSE--Tenemos que recuperar la seccion con el numero mas grande
			BEGIN
				SELECT @MaxSeccion = MAX(cf.Orden_Seccion)--recuperamos la seccion con el indice mas grande que haya. 
				FROM Conformado_For_Sec CF
				WHERE CF.CodigoFormulario = @Codigo

				UPDATE Conformado_For_Sec -- La metemos como la seccion numero 1 
				SET Orden_Seccion = @MaxSeccion + 1 
				WHERE CodigoFormulario = @Codigo AND TituloSeccion = @Titulo
			END
		COMMIT TRANSACTION transaccionAnnadirPregunta;
		FETCH NEXT FROM Cursor_Seccion into @Codigo, @Titulo, @OrdenSeccion
	END 
	CLOSE Cursor_Seccion
	DEALLOCATE Cursor_Seccion
END