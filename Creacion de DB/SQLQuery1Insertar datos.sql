use ClinicaDB
go
-- Insertar una persona completa
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Juan', 'Perez', '12345678', 'juan.perez@ejemplo.com', '1122334455', 'Buenos Aires');

-- Insertar otra persona solo con los campos obligatorios
INSERT INTO Persona (Nombre, Apellido, Dni)
VALUES ('Maria', 'Gomez', '87654321');

-- Insertar una persona que podría ser un médico (luego se referenciará en Medico/Usuario)
INSERT INTO Persona (Nombre, Apellido, Dni, Email)
VALUES ('Carlos', 'Rodriguez', '20304050', 'carlos.rodriguez@clinica.com');

-- Insertar una persona que podría ser un paciente (luego se referenciará en Paciente)
INSERT INTO Persona (Nombre, Apellido, Dni, Telefono, Localidad)
VALUES ('Laura', 'Martinez', '30405060', '5544332211', 'Cordoba');

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Victoria', 'Caseres', '47083346', 'victoria.caseres2005@gmail.com', '1132328274', 'Tigre');

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Anahi', 'Gimenez', '47505693', 'gimenez.anahi@hotmail.com', '1158692377', 'Malvinas');

Select * from PERSONA