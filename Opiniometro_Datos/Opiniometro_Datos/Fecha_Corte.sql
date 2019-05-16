CREATE TABLE [dbo].[Fecha_Corte]
(
	[Fecha_Inicio] date NOT NULL, 
	Fecha_Final date NOT NULL
	CONSTRAINT PKFecha_Corte PRIMARY KEY(Fecha_Inicio, Fecha_Final)

)
