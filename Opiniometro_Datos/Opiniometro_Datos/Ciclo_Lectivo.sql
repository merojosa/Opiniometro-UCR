CREATE TABLE [dbo].[Ciclo_Lectivo]
(
	[Anno] SMALLINT NOT NULL, 
    [Semestre] TINYINT NOT NULL,
	CONSTRAINT PK_Ciclo_Lectivo PRIMARY KEY (Anno,Semestre)
)
