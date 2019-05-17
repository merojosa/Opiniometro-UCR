SELECT * FROM Persona
SELECT * FROM Usuario

ALTER TABLE Persona
ADD Direccion nvarchar(256);

DELETE FROM Persona WHERE Nombre = 'Barry';