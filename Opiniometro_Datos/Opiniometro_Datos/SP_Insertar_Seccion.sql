GO
CREATE PROCEDURE SP_Insertar_Seccion
	@titulo NVARCHAR(120),
	@descripcion VARCHAR(300)
AS
	INSERT INTO Seccion ( Titulo, Descripcion )
	VALUES	( @titulo, @descripcion );
