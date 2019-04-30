CREATE TABLE [dbo].[Preguntas]
(
	[Planteamiento] NVARCHAR(50) NOT NULL PRIMARY KEY  /*creacion de la llave de la tabla preguntas*/
	/*CONSTRAINT [FK_dbo.Preguntas_dbo.TipoPreguntas_Nombre] FOREIGN KEY ([Nombre])
	REFERENCES [dbo].[TipoPreguntas] ([Nombre]) ON DELETE CASCADE   ESTA PARTE ES PARA CUANDO SE HAGA LA TABLA DE TIPO DE PREGUNTAS, 
	PORQUE ESTA TABLA TIENE FOREIGN KEY A NOMBRE, QUE ES UN ATRIBUTO DE TIPO DE PREGUNTA*/
	)
