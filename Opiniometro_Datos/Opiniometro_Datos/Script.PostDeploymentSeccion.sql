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