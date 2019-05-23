CREATE TABLE [dbo].[Fecha_Corte]
(
	[FechaInicio] datetime NOT NULL,
	[FechaFinal] datetime NOT NULL,
	constraint PK_Fecha_Corte PRIMARY KEY (FechaInicio, FechaFinal)
)
