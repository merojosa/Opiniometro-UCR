/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
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

MERGE INTO Seccion AS Target  
USING (VALUES          
	('Información del o la estudiante'),          
	('Evaluación del desempeño docente'),          
	('Temáticas transversales de la Universidad de Costa Rica')  
)
AS Source (Titulo)
ON Target.Titulo = Source.Titulo 
WHEN NOT MATCHED BY TARGET THEN
INSERT (Titulo)
VALUES (Titulo);

--Inserciones de datos

INSERT INTO Persona VALUES 
 ('111111', 'Juan', 'Carillo', 'Jimenez')
,('222222', 'María', 'Mejías', 'Guierrez')
,('333333', 'Ellie', 'Rodríguez', 'Rojas')
,('444444', 'Abel', 'Hernández', null);
INSERT INTO Estudiante VALUES 
 ('111111', 'B11111')
,('222222', 'B22222')
,('333333', 'B33333') 
,('444444', 'B44444');

MERGE INTO Curso AS Target
USING (VALUES
 ('CI1213', 'Ingenieria de Software'),
 ('CI1223', 'Bases de Datos'),
 ('CI1211', 'Proyecto Integrador')
)
AS Source ([CodigoCurso], NombreCurso)
ON Target.CodigoCurso = Source.CodigoCurso
WHEN NOT MATCHED BY TARGET THEN
INSERT (CodigoCurso, NombreCurso)
VALUES (CodigoCurso, NombreCurso);

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