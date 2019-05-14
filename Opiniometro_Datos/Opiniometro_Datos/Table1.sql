CREATE TABLE [dbo].[Table1]
(
	[Cedula] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Nombre] NVARCHAR(50) NOT NULL, 
    [Apellido1] NVARCHAR(50) NOT NULL, 
    [Apellido2] NVARCHAR(50) NULL, 
    [Direccion] NVARCHAR(256) NOT NULL 
)
