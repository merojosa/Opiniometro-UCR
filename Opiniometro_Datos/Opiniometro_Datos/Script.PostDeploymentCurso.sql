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