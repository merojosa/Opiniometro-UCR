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

MERGE INTO Preguntas AS Target
USING (VALUES
(1, 'Pregunta1', 'SiNo', 'Profesor'),
(2, 'Pregunta2', 'SeleccionUnica', 'Profesor'),
(3, 'Pregunta3', 'SeleccionMultiple', 'Curso')
)
AS Source ([Numero], Planteamiento, TipoPregunta, Categoria)
ON Target.Planteamiento = Source.Planteamiento
WHEN NOT MATCHED BY TARGET THEN
INSERT(Planteamiento, TipoPregunta, Categoria)
VALUES(Planteamiento, TipoPregunta, Categoria);
