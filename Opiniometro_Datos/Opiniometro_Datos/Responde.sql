CREATE TABLE [dbo].[Responde]
(
	Observacion NVARCHAR(120),
	Respuesta NVARCHAR(120) NOT NULL,
	RespuestaProfesor NVARCHAR(120),
	TituloSeccion NVARCHAR(120) NOT NULL FOREIGN KEY REFERENCES Seccion(Titulo)

)
