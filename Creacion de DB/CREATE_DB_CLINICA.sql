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
    Localidad NVARCHAR(100)
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
    DiaLaboral NVARCHAR(20)
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
    EstadoTurno NVARCHAR(20)
        CONSTRAINT CHK_EstadoTurno CHECK (EstadoTurno IN ('Nuevo','Reprogramado','Cancelado','No asistio','Cerrado'))
        DEFAULT 'Nuevo',
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



