--Eliminar todos los datos para que quede limpia la base a la hora de insertar nuevamente los datos establecidos.

EXEC sp_MSForEachTable 'DISABLE TRIGGER ALL ON ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'DELETE FROM ?'
GO
EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'
GO
EXEC sp_MSForEachTable 'ENABLE TRIGGER ALL ON ?'
GO

--Inserciones de datos

INSERT INTO Persona VALUES 
 ('111111', 'Juan', 'Carillo', 'Jimenez')
,('222222', 'María', 'Mejías', 'Guierrez')
,('333333', 'Ellie', 'Rodríguez', 'Rojas')
,('444444', 'Abel', 'Hernández', 'Pérez');
INSERT INTO Estudiante VALUES 
 ('111111', 'B11111')
,('222222', 'B22222')
,('333333', 'B33333') 
,('444444', 'B44444');

--Procedimientos almacenados

IF OBJECT_ID('MostrarEstudiantes', 'P') IS NOT NULL 
DROP PROC MostrarEstudiantes

IF OBJECT_ID('NombrePersona', 'P') IS NOT NULL 
DROP PROC NombrePersona

IF OBJECT_ID('DatosEstudiante', 'P') IS NOT NULL 
DROP PROC DatosEstudiante

go
--Pantalla 1, Home
CREATE PROCEDURE MostrarEstudiantes
AS 
SELECT Nombre, Apellido1, Apellido2, Carne
FROM Persona JOIN Estudiante ON Persona.Cedula = Estudiante.Cedula_Estudiante;


--Pantalla 2 solo el nombre para bienvenida
go
CREATE PROCEDURE NombrePersona 
@Cedula VARCHAR(9)
AS
SELECT Nombre
FROM Persona
WHERE Cedula = @Cedula;


--Pantalla 3, informacion de un estudiante
GO
CREATE PROCEDURE DatosEstudiante
@Cedula VARCHAR(9)
AS
SELECT CONCAT(Nombre, ' ' ,Apellido1, ' ', Apellido2) as 'Nombre Completo', Carne, Cedula
FROM Persona JOIN Estudiante ON Persona.Cedula = Estudiante.Cedula_Estudiante
WHERE Cedula = @Cedula;