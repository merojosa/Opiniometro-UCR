CREATE PROCEDURE [dbo].[BuscarCursoPorNombre]
	@NombreCurso varchar(75)
AS
	SELECT * 
	FROM Curso c
	WHERE c.Nombre LIKE '%' + @NombreCurso + '%'
