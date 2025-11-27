CREATE DATABASE ClinicaDB;
GO

USE ClinicaDB;
GO

-- ======================================
-- TABLA: Persona (base de herencia)
-- ======================================
CREATE TABLE Persona (
    IdPersona INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Dni NVARCHAR(8) NOT NULL UNIQUE,
    Email NVARCHAR(100),
    Telefono NVARCHAR(15),
    Localidad NVARCHAR(100),
	Activo BIT NOT NULL DEFAULT 1
);

-- ======================================
-- TABLA: Usuario (hereda de Persona)
-- ======================================
CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY,
    Password NVARCHAR(100) NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Persona(IdPersona)
);

-- ======================================
-- TABLA: Medico (hereda de Usuario)
-- ======================================
CREATE TABLE Medico (
    IdMedico INT PRIMARY KEY,
    Matricula NVARCHAR(50) NOT NULL,
    FOREIGN KEY (IdMedico) REFERENCES Usuario(IdUsuario)
);

-- ======================================
-- TABLA: Paciente (hereda de Persona)
-- ======================================
CREATE TABLE Paciente (
    IdPaciente INT PRIMARY KEY,
    FechaNacimiento DATE NOT NULL,
    FOREIGN KEY (IdPaciente) REFERENCES Persona(IdPersona)
);

-- ======================================
-- TABLA: Rol
-- ======================================
CREATE TABLE Rol (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    TipoRol NVARCHAR(50) NOT NULL
);

-- ======================================
-- TABLA: Permiso
-- ======================================
CREATE TABLE Permiso (
    IdPermiso INT IDENTITY(1,1) PRIMARY KEY,
    NombrePermiso NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

-- ======================================
-- RELACIÓN: Rol ↔ Permiso (N:N)
-- ======================================
CREATE TABLE RolPermiso (
    IdRol INT NOT NULL,
    IdPermiso INT NOT NULL,
    PRIMARY KEY (IdRol, IdPermiso),
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol),
    FOREIGN KEY (IdPermiso) REFERENCES Permiso(IdPermiso)
);

-- ======================================
-- RELACIÓN: Usuario ↔ Rol (N:N)
-- ======================================
CREATE TABLE UsuarioRol (
    IdUsuario INT NOT NULL,
    IdRol INT NOT NULL,
    PRIMARY KEY (IdUsuario, IdRol),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);

-- ======================================
-- TABLA: Especialidad
-- ======================================
CREATE TABLE Especialidad (
    IdEspecialidad INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);

-- ======================================
-- RELACIÓN: Medico ↔ Especialidad (N:N)
-- ======================================
CREATE TABLE MedicoEspecialidad (
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    PRIMARY KEY (IdMedico, IdEspecialidad),
    FOREIGN KEY (IdMedico) REFERENCES Medico(IdMedico),
    FOREIGN KEY (IdEspecialidad) REFERENCES Especialidad(IdEspecialidad)
);

-- ======================================
-- TABLA: TurnoDeTrabajo (plantilla de horarios)
-- ======================================
CREATE TABLE TurnoDeTrabajo (
    IdTurnoTrabajo INT IDENTITY(1,1) PRIMARY KEY,
    TipoDeTurno NVARCHAR(50),
    HoraInicioDefault TIME NOT NULL,
    HoraFinDefault TIME NOT NULL
);

-- ======================================
-- TABLA: JornadaLaboral
-- ======================================
CREATE TABLE JornadaLaboral (
    IdJornadaLaboral INT IDENTITY(1,1) PRIMARY KEY,
    IdMedico INT NOT NULL,
    IdTurnoTrabajo INT NULL,
    HoraInicio TIME,
    HoraFin TIME,
    DiaLaboral NVARCHAR(20)NOT NULL
        CHECK (DiaLaboral IN ('Lunes','Martes','Miercoles','Jueves','Viernes','Sabado','Domingo')),
    FOREIGN KEY (IdMedico) REFERENCES Medico(IdMedico),
    FOREIGN KEY (IdTurnoTrabajo) REFERENCES TurnoDeTrabajo(IdTurnoTrabajo)
);

-- ======================================
-- TABLA: Turno (asociacion entre Medico y Paciente)
-- ======================================
CREATE TABLE Turno (
    IdTurno INT IDENTITY(1,1) PRIMARY KEY,
    IdMedico INT NOT NULL,
    IdPaciente INT NOT NULL,
    FechaHoraInicio DATETIME NOT NULL,
    FechaHoraFin DATETIME NOT NULL,
    MotivoDeConsulta NVARCHAR(255),
    Diagnostico NVARCHAR(255),
    EstadoTurno NVARCHAR(20)NOT NULL
	    DEFAULT 'Nuevo',
        CONSTRAINT CHK_EstadoTurno CHECK (EstadoTurno IN ('Nuevo','Reprogramado','Cancelado','No asistio','Cerrado')),       
    FOREIGN KEY (IdMedico) REFERENCES Medico(IdMedico),
    FOREIGN KEY (IdPaciente) REFERENCES Paciente(IdPaciente)
);

-- ======================================
-- ÍNDICES 
-- ======================================
CREATE INDEX IX_Turno_IdMedico ON Turno(IdMedico);
CREATE INDEX IX_Turno_IdPaciente ON Turno(IdPaciente);
CREATE INDEX IX_MedicoEspecialidad_IdMedico ON MedicoEspecialidad(IdMedico);
CREATE INDEX IX_Jornada_IdMedico ON JornadaLaboral(IdMedico);
CREATE INDEX IX_UsuarioRol_IdUsuario ON UsuarioRol(IdUsuario);

-- Insertar una persona completa
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Juan', 'Perez', '12345678', 'juan.perez@ejemplo.com', '1122334455', 'Buenos Aires');

INSERT INTO Persona (Nombre, Apellido, Dni, Email)
VALUES ('Maria', 'Gomez', '87654321', 'maria.gomez@recepcion.com');

-- Insertar una persona que podría ser un médico (luego se referenciará en Medico/Usuario)
INSERT INTO Persona (Nombre, Apellido, Dni, Email)
VALUES ('Carlos', 'Rodriguez', '20304050', 'carlos.rodriguez@clinica.com');

-- Insertar una persona que podría ser un paciente (luego se referenciará en Paciente)
INSERT INTO Persona (Nombre, Apellido, Dni, Telefono, Localidad)
VALUES ('Laura', 'Martinez', '30405060', '5544332211', 'Cordoba');

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Victoria', 'Caseres', '47083346', 'victoria.caseres2005@gmail.com', '1132328274', 'Tigre')

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Anahi', 'Gimenez', '47505693', 'gimenez.anahi@hotmail.com', '1158692377', 'Malvinas');

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Roberto', 'Gallo', '10111213', 'gallo@ejemplo.com', '1188776655', 'La Plata')   
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Carolina', 'Diaz', '14151617', 'diaz@ejemplo.com', '1199887766', 'San Isidro')
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Javier', 'Luna', '18192021', 'luna@ejemplo.com', '1122113344', 'Vicente Lopez')
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Sofia', 'Fernandez', '22232425', 'sofia.f@paciente.com', '1155667788', 'San Fernando')       
INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Ricardo', 'Nuñez', '26272829', 'ricardo.n@paciente.com', '1144556677', 'Tigre')             

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad) VALUES
('Pedro', 'Gimenez', '30001001', 'p.gimenez@mail.com', '1112223344', 'Tigre'),          
('Ana', 'Lopez', '31002002', 'a.lopez@mail.com', '1123344556', 'San Fernando'),         
('Martin', 'Perez', '32003003', 'm.perez@mail.com', '1134455667', 'Vicente Lopez'),     
('Julia', 'Vazquez', '33004004', 'j.vazquez@mail.com', '1145566778', 'San Isidro'),     
('Gabriel', 'Acosta', '34005005', 'g.acosta@mail.com', '1156677889', 'La Plata'),       
('Florencia', 'Blanco', '35006006', 'f.blanco@mail.com', '1167788990', 'Buenos Aires'), 
('Elias', 'Castro', '36007007', 'e.castro@mail.com', '1178899001', 'Cordoba'),          
('Marina', 'Duarte', '37008008', 'm.duarte@mail.com', '1189900112', 'Malvinas'),        
('Nicolas', 'Estevez', '38009009', 'n.estevez@mail.com', '1190011223', 'Tigre'),        
('Paula', 'Ferrer', '39010010', 'p.ferrer@mail.com', '1101122334', 'San Isidro');       

-- ======================================
-- Insertar datos en la tabla Usuario
-- ======================================
-- Asignamos a Carlos Rodriguez (IdPersona = 3) y Anahi Gimenez (IdPersona = 6) como Usuarios/Medicos
INSERT INTO Usuario (IdUsuario, Password)
VALUES (3, 'passmedico123');

INSERT INTO Usuario (IdUsuario, Password)
VALUES (6, 'passmedico456');

-- Asignamos a Juan Perez (IdPersona = 1) como un usuario administrativo
INSERT INTO Usuario (IdUsuario, Password)
VALUES (1, 'passadmin789');

-- Asignamos a Maria Gomez(IdPersona = 2) como un recepcionista
INSERT INTO Usuario (IdUsuario, Password) VALUES (2, 'passrecepcionista');

INSERT INTO Usuario (IdUsuario, Password) VALUES (7, '12345678')
INSERT INTO Usuario (IdUsuario, Password) VALUES (8, '12345678')
INSERT INTO Usuario (IdUsuario, Password) VALUES (9, '12345678')

-- ======================================
-- Insertar datos en la tabla Medico
-- ======================================
-- Carlos Rodriguez (IdUsuario/IdMedico = 3)
INSERT INTO Medico (IdMedico, Matricula)
VALUES (3, 'MN-45678');

-- Anahi Gimenez (IdUsuario/IdMedico = 6)
INSERT INTO Medico (IdMedico, Matricula)
VALUES (6, 'MN-87654');

INSERT INTO Medico (IdMedico, Matricula) VALUES (9, 'MN-90123')

-- ======================================
-- Insertar datos en la tabla Paciente
-- ======================================
-- Laura Martinez (IdPersona/IdPaciente = 4)
INSERT INTO Paciente (IdPaciente, FechaNacimiento)
VALUES (4, '1990-05-15');

-- Victoria Caseres (IdPersona/IdPaciente = 5)
INSERT INTO Paciente (IdPaciente, FechaNacimiento)
VALUES (5, '2005-08-20');

INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (11, '2000-01-01')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (10, '1985-11-20')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (12, '1995-01-10')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (13, '2001-02-20')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (14, '1980-03-30')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (15, '1992-04-15')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (16, '2010-05-25')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (17, '1975-06-05')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (18, '2005-07-15')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (19, '1988-08-28')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (20, '1999-09-09')
INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (21, '2003-10-18')


-- ======================================
-- Insertar datos en la tabla Rol
-- ======================================
INSERT INTO Rol (TipoRol) VALUES ('Administrador'); -- IdRol = 1
INSERT INTO Rol (TipoRol) VALUES ('Medico'); -- IdRol = 2
INSERT INTO Rol (TipoRol) VALUES ('Recepcionista'); -- IdRol = 3


-- ======================================
-- Insertar datos en la tabla Permiso
-- ======================================
INSERT INTO Permiso (NombrePermiso, Descripcion) VALUES ('GestionarUsuarios', 'Crear, modificar y eliminar usuarios.'); -- IdPermiso = 1
INSERT INTO Permiso (NombrePermiso, Descripcion) VALUES ('GestionarTurnos', 'Asignar, reprogramar y cancelar turnos.'); -- IdPermiso = 2
INSERT INTO Permiso (NombrePermiso, Descripcion) VALUES ('VerHistoriasClinicas', 'Acceso a registros de pacientes.'); -- IdPermiso = 3


-- ======================================
-- RELACIÓN: Rol ↔ Permiso - RolPermiso
-- ======================================
-- Administrador (1) tiene todos los permisos
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (1, 1);
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (1, 2);
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (1, 3);

-- Medico (2) tiene GestionarTurnos y VerHistoriasClinicas
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (2, 2);
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (2, 3);

-- Recepcionista (3) tiene GestionarTurnos
INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (3, 2);


-- ======================================
-- RELACIÓN: Usuario ↔ Rol - UsuarioRol
-- ======================================
-- Juan Perez (IdUsuario = 1) es Administrador (1)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (1, 1)

-- Carlos Rodriguez (IdUsuario = 3) es Medico (2)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (3, 2)

-- Anahi Gimenez (IdUsuario = 6) es Medico (2)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (6, 2)

-- Maria Gomez (IdUsuario = 2) es Recepcionista (3)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (2, 3)

INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (7, 3)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (8, 1)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (9, 2)


-- ======================================
-- Insertar datos en la tabla Especialidad
-- ======================================
INSERT INTO Especialidad (Nombre) VALUES ('Cardiología')
INSERT INTO Especialidad (Nombre) VALUES ('Pediatría') 
INSERT INTO Especialidad (Nombre) VALUES ('Dermatología')
INSERT INTO Especialidad (Nombre) VALUES ('Gastroenterología')

-- ======================================
-- RELACIÓN: Medico ↔ Especialidad 
-- ======================================
-- Medico 3 (Carlos Rodriguez) tiene Cardiología (1)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (3, 1);

-- Medico 6 (Anahi Gimenez) tiene Pediatría (2)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (6, 2);
-- Medico 6 (Anahi Gimenez) también tiene Dermatología (3)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (6, 3);

INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (9, 4); -- Gastroenterología

-- ======================================
-- Insertar datos en la tabla TurnoDeTrabajo
-- ======================================
INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Mañana', '08:00:00', '12:00:00'); -- IdTurnoTrabajo = 1

INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Tarde', '14:00:00', '18:00:00'); -- IdTurnoTrabajo = 2

INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Jornada Completa', '09:00:00', '17:00:00'); -- IdTurnoTrabajo = 3

INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Vespertino', '18:00:00', '21:00:00'); -- IdTurnoTrabajo = 4

-- ======================================
-- Insertar datos en la tabla JornadaLaboral
-- ======================================
-- Medico 3 (Carlos Rodriguez) - Lunes por la mañana (Turno 1)
INSERT INTO JornadaLaboral (IdMedico, IdTurnoTrabajo, DiaLaboral)
VALUES (3, 1, 'Lunes');

-- Medico 3 (Carlos Rodriguez) - Miércoles por la tarde (Turno 2) con horario ajustado
INSERT INTO JornadaLaboral (IdMedico, HoraInicio, HoraFin, DiaLaboral)
VALUES (3, '13:00:00', '17:00:00', 'Miercoles');

-- Medico 6 (Anahi Gimenez) - Viernes jornada completa (Turno 3)
INSERT INTO JornadaLaboral (IdMedico, IdTurnoTrabajo, DiaLaboral)
VALUES (6, 3, 'Viernes');

-- Medico 6 (Anahi Gimenez) - Sábado por la mañana (Turno 1)
INSERT INTO JornadaLaboral (IdMedico, IdTurnoTrabajo, DiaLaboral)
VALUES (6, 1, 'Sabado');

INSERT INTO JornadaLaboral (IdMedico, IdTurnoTrabajo, DiaLaboral) VALUES (9, 2, 'Martes'); -- Martes: 14:00-18:00
INSERT INTO JornadaLaboral (IdMedico, IdTurnoTrabajo, DiaLaboral) VALUES (9, 4, 'Jueves'); -- Jueves: 18:00-21:00


-- ======================================
-- Insertar datos en la tabla Turno
-- ======================================
-- Turno 1: Medico 3 (Cardiología) con Paciente 4 (Laura Martinez)
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno)
VALUES (
    3, 
    4, 
    '2025-12-01 09:00:00', 
    '2025-12-01 09:30:00', 
    'Control de rutina cardiológico', 
    'Nuevo'
);

-- Turno 2: Medico 6 (Pediatría/Dermatología) con Paciente 5 (Victoria Caseres)
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno)
VALUES (
    6, 
    5, 
    '2025-12-05 15:00:00', 
    '2025-12-05 15:45:00', 
    'Erupción cutánea', 
    'Nuevo'
);

-- Turno 3: Turno cerrado (ya realizado)
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, Diagnostico, EstadoTurno)
VALUES (
    3, 
    5, 
    '2025-11-01 14:00:00', 
    '2025-11-01 14:45:00', 
    'Dolor de pecho', 
    'Estrés, sin hallazgos cardiológicos', 
    'Cerrado'
);

-- Turno 4: Turno cancelado
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno)
VALUES (
    6, 
    4, 
    '2025-12-10 10:00:00', 
    '2025-12-10 10:30:00', 
    'Consulta general', 
    'Cancelado'
);

-- Turno 5: Turno reprogramado
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno)
VALUES (
    6, 
    5, 
    '2025-12-06 09:00:00', 
    '2025-12-06 09:45:00', 
    'Seguimiento de erupción', 
    'Reprogramado'
);

INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES 
(9, 10, '2025-12-02 14:30:00', '2025-12-02 15:00:00', 'Dolor abdominal crónico', 'Nuevo'); -- 6
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES 
(3, 11, '2025-12-03 16:00:00', '2025-12-03 16:30:00', 'Evaluación pre-deportiva', 'Nuevo'); -- 7
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES 
(6, 4, '2025-12-06 10:00:00', '2025-12-06 10:30:00', 'Control Pediatrico', 'No asistio'); -- 8

-- 10 Nuevos Turnos (para Pacientes 12 al 21):
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(3, 12, '2025-12-01 10:00:00', '2025-12-01 10:30:00', 'Control de presión arterial', 'Nuevo'); -- 9
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(3, 13, '2025-12-01 11:30:00', '2025-12-01 12:00:00', 'Consulta por palpitaciones', 'Nuevo'); -- 10
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(9, 14, '2025-12-02 15:00:00', '2025-12-02 15:30:00', 'Revisión por acidez estomacal', 'Nuevo'); -- 11
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(9, 15, '2025-12-02 17:00:00', '2025-12-02 17:45:00', 'Seguimiento de colonoscopia', 'Nuevo'); -- 12
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(3, 16, '2025-12-03 13:30:00', '2025-12-03 14:00:00', 'Exámen cardiológico pediátrico', 'Nuevo'); -- 13
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(3, 17, '2025-12-03 16:30:00', '2025-12-03 17:00:00', 'Electrocardiograma de control', 'Nuevo'); -- 14
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(9, 18, '2025-12-04 19:00:00', '2025-12-04 19:30:00', 'Problemas de tránsito intestinal', 'Nuevo'); -- 15
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(6, 19, '2025-12-05 10:00:00', '2025-12-05 10:30:00', 'Control de lunar sospechoso', 'Nuevo'); -- 16
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(6, 20, '2025-12-05 13:00:00', '2025-12-05 14:00:00', 'Chequeo anual pediátrico', 'Nuevo'); -- 17
INSERT INTO Turno (IdMedico, IdPaciente, FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, EstadoTurno) VALUES
(6, 21, '2025-12-06 08:00:00', '2025-12-06 08:30:00', 'Consulta por alergia cutánea', 'Nuevo'); -- 18
