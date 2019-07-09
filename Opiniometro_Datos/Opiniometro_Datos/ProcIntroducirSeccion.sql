CREATE PROCEDURE [dbo].[ProcIntroducirSeccion]
	@titulo nvarchar (120),
	@descripcion varchar(300)
AS
	BEGIN
	INSERT INTO Seccion(Titulo, Descripcion)
	VALUES (@titulo, @descripcion)
	END
