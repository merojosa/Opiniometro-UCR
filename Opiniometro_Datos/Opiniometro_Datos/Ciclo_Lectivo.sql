CREATE TABLE [dbo].[Ciclo_Lectivo]
(
	[Año] SMALLINT NOT NULL, 
    [Semestre] TINYINT NOT NULL,
	CONSTRAINT PK_Ciclo_Lectivo PRIMARY KEY (Año,Semestre)
)
