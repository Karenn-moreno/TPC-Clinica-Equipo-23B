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
VALUES ('Victoria', 'Caseres', '47083346', 'victoria.caseres2005@gmail.com', '1132328274', 'Tigre');

INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
VALUES ('Anahi', 'Gimenez', '47505693', 'gimenez.anahi@hotmail.com', '1158692377', 'Malvinas');

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
INSERT INTO Usuario (IdUsuario, Password)
VALUES (2, 'passrecepcionista');

-- ======================================
-- Insertar datos en la tabla Medico
-- ======================================
-- Carlos Rodriguez (IdUsuario/IdMedico = 3)
INSERT INTO Medico (IdMedico, Matricula)
VALUES (3, 'MN-45678');

-- Anahi Gimenez (IdUsuario/IdMedico = 6)
INSERT INTO Medico (IdMedico, Matricula)
VALUES (6, 'MN-87654');


-- ======================================
-- Insertar datos en la tabla Paciente
-- ======================================
-- Laura Martinez (IdPersona/IdPaciente = 4)
INSERT INTO Paciente (IdPaciente, FechaNacimiento)
VALUES (4, '1990-05-15');

-- Victoria Caseres (IdPersona/IdPaciente = 5)
INSERT INTO Paciente (IdPaciente, FechaNacimiento)
VALUES (5, '2005-08-20');


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
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (1, 1);

-- Carlos Rodriguez (IdUsuario = 3) es Medico (2)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (3, 2);

-- Anahi Gimenez (IdUsuario = 6) es Medico (2)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (6, 2);

-- Maria Gomez (IdUsuario = 2) es Recepcionista (3)
INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (2, 3);


-- ======================================
-- Insertar datos en la tabla Especialidad
-- ======================================
INSERT INTO Especialidad (Nombre) VALUES ('Cardiología'); -- IdEspecialidad = 1
INSERT INTO Especialidad (Nombre) VALUES ('Pediatría'); -- IdEspecialidad = 2
INSERT INTO Especialidad (Nombre) VALUES ('Dermatología'); -- IdEspecialidad = 3


-- ======================================
-- RELACIÓN: Medico ↔ Especialidad 
-- ======================================
-- Medico 3 (Carlos Rodriguez) tiene Cardiología (1)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (3, 1);

-- Medico 6 (Anahi Gimenez) tiene Pediatría (2)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (6, 2);
-- Medico 6 (Anahi Gimenez) también tiene Dermatología (3)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (6, 3);


-- ======================================
-- Insertar datos en la tabla TurnoDeTrabajo
-- ======================================
INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Mañana', '08:00:00', '12:00:00'); -- IdTurnoTrabajo = 1

INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Tarde', '14:00:00', '18:00:00'); -- IdTurnoTrabajo = 2

INSERT INTO TurnoDeTrabajo (TipoDeTurno, HoraInicioDefault, HoraFinDefault)
VALUES ('Jornada Completa', '09:00:00', '17:00:00'); -- IdTurnoTrabajo = 3


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
