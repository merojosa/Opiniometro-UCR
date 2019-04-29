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

INSERT INTO Persona
VALUES ('111111', 'Juan', 'Carillo', 'Jimenez');
INSERT INTO Estudiante
VALUES ('111111', 'BXXXXX');

/* PENDING: PROCEDIMIENTOS ALMACENADOS
SELECT P.Nombre, P.Apellido1, P.Apellido2, E.Carne
FROM Persona P JOIN Estudiante E ON E.Cedula_Estudiante = P.Cedula
/*