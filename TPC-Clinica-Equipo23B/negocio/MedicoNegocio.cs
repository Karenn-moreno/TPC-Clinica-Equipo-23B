using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MedicoNegocio
    {
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Se modificó la consulta para incluir WHERE P.Activo = 1
                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Email, P.Telefono,M.matricula,
                        ISNULL(E.ListaEspecialidades, 'Sin Asignar') AS EspecialidadesTexto,
                        ISNULL(J.ListaHorarios, 'Sin Horarios') AS HorariosTexto
                    FROM 
                        Persona P
                    INNER JOIN 
                        Medico M ON P.IdPersona = M.IdMedico
                    OUTER APPLY (
                        SELECT 
                            STRING_AGG(Esp.Nombre, ', ') AS ListaEspecialidades
                        FROM 
                            MedicoEspecialidad ME
                        INNER JOIN 
                            Especialidad Esp ON ME.IdEspecialidad = Esp.IdEspecialidad
                        WHERE 
                            ME.IdMedico = M.IdMedico
                    ) AS E
                    OUTER APPLY (
                       SELECT 
            STRING_AGG(GrupoHorario, ' <br> ') WITHIN GROUP (ORDER BY MinDiaIdx) AS ListaHorarios
        FROM (
            SELECT 
                STRING_AGG(DiaLaboral, ', ') WITHIN GROUP (ORDER BY DiaIdx) + ' ' +
                CONVERT(VARCHAR(5), ISNULL(HoraInicio, '00:00'), 108) + '-' + 
                CONVERT(VARCHAR(5), ISNULL(HoraFin, '00:00'), 108) as GrupoHorario,
                MIN(DiaIdx) as MinDiaIdx
            FROM (
                SELECT DISTINCT
                    JL.DiaLaboral,
                    ISNULL(JL.HoraInicio, TT.HoraInicioDefault) as HoraInicio,
                    ISNULL(JL.HoraFin, TT.HoraFinDefault) as HoraFin,
                    CASE JL.DiaLaboral
                        WHEN 'Lunes' THEN 1
                        WHEN 'Martes' THEN 2
                        WHEN 'Miercoles' THEN 3
                        WHEN 'Jueves' THEN 4
                        WHEN 'Viernes' THEN 5
                        WHEN 'Sabado' THEN 6
                        WHEN 'Domingo' THEN 7
                    END as DiaIdx
                FROM JornadaLaboral JL
                LEFT JOIN TurnoDeTrabajo TT ON JL.IdTurnoTrabajo = TT.IdTurnoTrabajo
                WHERE JL.IdMedico = M.IdMedico
            ) T1
            GROUP BY HoraInicio, HoraFin
        ) T2
    ) AS J
    WHERE P.Activo = 1
    ORDER BY P.Apellido ASC, P.Nombre ASC
");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    int id = (int)datos.Lector["IdPersona"];
                    Medico medico = lista.Find(m => m.IdPersona == id);
                    if (medico == null)
                    {
                        medico = new Medico
                        {
                            IdPersona = id,
                            Nombre = (string)datos.Lector["Nombre"],
                            Apellido = (string)datos.Lector["Apellido"],
                            EspecialidadesTexto = (string)datos.Lector["EspecialidadesTexto"],
                            HorariosTexto = (string)datos.Lector["HorariosTexto"]
                        };

                        if (datos.Lector["Email"] != DBNull.Value)
                            medico.Email = (string)datos.Lector["Email"];
                        if (datos.Lector["Telefono"] != DBNull.Value)
                            medico.Telefono = (string)datos.Lector["Telefono"];

                        lista.Add(medico);
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar médicos", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public Medico ObtenerPorId(int idMedico)
        {
            Medico medico = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // CARGA DE DATOS PRINCIPALES (Persona y Médico)
                datos.setearConsulta(@"
            SELECT P.IdPersona, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono,
                   M.Matricula
            FROM Persona P
            INNER JOIN Medico M ON P.IdPersona = M.IdMedico
            WHERE P.IdPersona = @id
        ");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    medico = new Medico
                    {
                        IdPersona = (int)datos.Lector["IdPersona"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Apellido = (string)datos.Lector["Apellido"],
                        // Asegurar la carga de DNI y Matrícula:
                        Dni = datos.Lector["Dni"].ToString(),
                        Matricula = datos.Lector["Matricula"] != DBNull.Value ? datos.Lector["Matricula"].ToString() : "",
                        Email = datos.Lector["Email"] != DBNull.Value ? datos.Lector["Email"].ToString() : "",
                        Telefono = datos.Lector["Telefono"] != DBNull.Value ? datos.Lector["Telefono"].ToString() : "",
                    };
                }

                datos.cerrarConexion();

                if (medico == null)
                    return null;

                // CARGAR ESPECIALIDADES


                datos = new AccesoDatos();
                datos.setearConsulta(@"
            SELECT E.IdEspecialidad, E.Nombre 
            FROM MedicoEspecialidad ME
            INNER JOIN Especialidad E ON ME.IdEspecialidad = E.IdEspecialidad
            WHERE ME.IdMedico = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    MedicoEspecialidad me = new MedicoEspecialidad();
                    me.IdMedico = idMedico;
                    me.IdEspecialidad = (int)datos.Lector["IdEspecialidad"];

                    me.Especialidad = new Especialidad();
                    me.Especialidad.IdEspecialidad = me.IdEspecialidad;
                    me.Especialidad.Nombre = (string)datos.Lector["Nombre"];

                    medico.MedicoEspecialidades.Add(me);
                }
                datos.cerrarConexion();


                //CARGAR HORARIOS
                datos = new AccesoDatos();
                datos.setearConsulta(@"
    SELECT IdJornadaLaboral, DiaLaboral, HoraInicio, HoraFin 
    FROM JornadaLaboral
    WHERE IdMedico = @id
");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    JornadaLaboral j = new JornadaLaboral();
                    j.IdJornadaLaboral = (int)datos.Lector["IdJornadaLaboral"];

                    string diaTexto = (string)datos.Lector["DiaLaboral"];
                    j.DiaLaboral = (DiaLaboral)Enum.Parse(typeof(DiaLaboral), diaTexto);

                    if (datos.Lector["HoraInicio"] != DBNull.Value)
                    {
                        j.HorarioInicio = (TimeSpan)datos.Lector["HoraInicio"];
                    }

                    if (datos.Lector["HoraFin"] != DBNull.Value)
                    {
                        j.HoraFin = (TimeSpan)datos.Lector["HoraFin"];
                    }
                    medico.JornadasLaborales.Add(j);
                }
                datos.cerrarConexion();

                if (medico.MedicoEspecialidades != null && medico.MedicoEspecialidades.Count > 0)
                {
                    medico.EspecialidadesTexto = string.Join(", ", medico.MedicoEspecialidades
                        .Where(me => me.Especialidad != null)
                        .Select(me => me.Especialidad.Nombre));
                }
                else
                {
                    medico.EspecialidadesTexto = "Sin asignar";
                }


                if (medico.JornadasLaborales != null && medico.JornadasLaborales.Count > 0)
                {
                    medico.HorariosTexto = string.Join("<br>", medico.JornadasLaborales.Select(j =>
                        $"{j.DiaLaboral}: {j.HorarioInicio:hh\\:mm} - {j.HoraFin:hh\\:mm}"));
                }
                else
                {
                    medico.HorariosTexto = "Sin horarios";
                }

                return medico;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener médico por ID.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Medico nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Insertar en Persona
                datos.setearConsulta(@"
            INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono)
            VALUES (@Nombre, @Apellido, @Dni, @Email, @Telefono);
            SELECT SCOPE_IDENTITY() AS IdNuevo;
        ");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Dni", nuevo.Dni);
                datos.setearParametro("@Email", string.IsNullOrEmpty(nuevo.Email) ? DBNull.Value : (object)nuevo.Email);
                datos.setearParametro("@Telefono", string.IsNullOrEmpty(nuevo.Telefono) ? DBNull.Value : (object)nuevo.Telefono);

                datos.ejecutarLectura();
                int idPersona = 0;
                if (datos.Lector.Read())
                    idPersona = Convert.ToInt32(datos.Lector["IdNuevo"]);


                nuevo.IdPersona = idPersona;

                datos.cerrarConexion();

                // Insertar en Usuario
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Usuario (IdUsuario, Password) VALUES (@IdUsuario, @Password)");
                datos.setearParametro("@IdUsuario", idPersona);
                datos.setearParametro("@Password", "default123");
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Insertar en Medico
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Medico (IdMedico, Matricula) VALUES (@IdMedico, @Matricula)");
                datos.setearParametro("@IdMedico", idPersona);
                datos.setearParametro("@Matricula", string.IsNullOrEmpty(nuevo.Matricula) ? DBNull.Value : (object)nuevo.Matricula);
                datos.ejecutarAccion();

                // Insertar Especialidades usando Id en lugar de nombre
                if (!string.IsNullOrEmpty(nuevo.EspecialidadesTexto))
                {
                    string[] especialidades = nuevo.EspecialidadesTexto
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var idEsp in especialidades)
                    {
                        AgregarEspecialidad(idPersona, Convert.ToInt32(idEsp.Trim()));
                    }
                }
                if (nuevo.JornadasLaborales != null && nuevo.JornadasLaborales.Count > 0)
                {
                    foreach (JornadaLaboral jornada in nuevo.JornadasLaborales)
                    {
                        datos = new AccesoDatos();

                        datos.setearConsulta("INSERT INTO JornadaLaboral (IdMedico, DiaLaboral, HoraInicio, HoraFin) VALUES (@IdMedico, @Dia, @Inicio, @Fin)");

                        datos.setearParametro("@IdMedico", idPersona);
                        datos.setearParametro("@Dia", jornada.DiaLaboral.ToString());
                        datos.setearParametro("@Inicio", jornada.HorarioInicio);
                        datos.setearParametro("@Fin", jornada.HoraFin);

                        datos.ejecutarAccion();
                        datos.cerrarConexion();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar médico", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Medico medico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Actualizar Persona
                datos.setearConsulta(@"
                    UPDATE Persona
                    SET Nombre = @Nombre,
                        Apellido = @Apellido,
                        Dni = @Dni,
                        Email = @Email,
                        Telefono = @Telefono
                    WHERE IdPersona = @IdPersona
                ");
                datos.setearParametro("@Nombre", medico.Nombre);
                datos.setearParametro("@Apellido", medico.Apellido);
                datos.setearParametro("@Dni", medico.Dni);
                datos.setearParametro("@Email", string.IsNullOrEmpty(medico.Email) ? DBNull.Value : (object)medico.Email);
                datos.setearParametro("@Telefono", string.IsNullOrEmpty(medico.Telefono) ? DBNull.Value : (object)medico.Telefono);
                datos.setearParametro("@IdPersona", medico.IdPersona);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Actualizar Medico
                datos = new AccesoDatos();
                datos.setearConsulta(@"
                    UPDATE Medico
                    SET Matricula = @Matricula
                    WHERE IdMedico = @IdMedico
                ");
                datos.setearParametro("@Matricula", string.IsNullOrEmpty(medico.Matricula) ? DBNull.Value : (object)medico.Matricula);
                datos.setearParametro("@IdMedico", medico.IdPersona);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Eliminar especialidades existentes
                datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM MedicoEspecialidad WHERE IdMedico = @IdMedico");
                datos.setearParametro("@IdMedico", medico.IdPersona);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Insertar nuevas especialidades
                if (!string.IsNullOrEmpty(medico.EspecialidadesTexto))
                {
                    string[] especialidades = medico.EspecialidadesTexto.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var idEsp in especialidades)
                    {
                        AgregarEspecialidad(medico.IdPersona, Convert.ToInt32(idEsp.Trim()));
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar médico", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método modificado para recibir IdEspecialidad
        public void AgregarEspecialidad(int idMedico, int idEspecialidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
            INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad)
            VALUES (@IdMedico, @IdEspecialidad)
        ");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@IdEspecialidad", idEspecialidad);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar especialidad al médico", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void EliminarFisico(int idMedico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM MedicoEspecialidad WHERE IdMedico = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarAccion();

                datos.setearConsulta("DELETE FROM JornadaLaboral WHERE IdMedico = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarAccion();

                datos.setearConsulta("DELETE FROM Medico WHERE IdMedico = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarAccion();

                datos.setearConsulta("DELETE FROM Usuario WHERE IdUsuario = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarAccion();

                datos.setearConsulta("DELETE FROM Persona WHERE IdPersona = @id");
                datos.setearParametro("@id", idMedico);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar médico FÍSICAMENTE", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void EliminarLogico(int idMedico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.setearConsulta(@"SELECT COUNT(*) FROM Turno 
                               WHERE IdMedico = @Id 
                               AND FechaHoraInicio >= CAST(GETDATE() AS DATE)
                               AND EstadoTurno NOT IN ('Cancelado', 'Cerrado', 'NoAsistio')");

                datos.setearParametro("@Id", idMedico);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    int turnosPendientes = (int)datos.Lector[0];

                    if (turnosPendientes > 0)
                    {
                        throw new Exception("No se puede dar de baja: El médico tiene turnos futuros pendientes.");
                    }
                }
                datos.cerrarConexion();


                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE Persona SET Activo = 0 WHERE IdPersona = @Id");
                datos.setearParametro("@Id", idMedico);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Reactivar(int idMedico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Persona SET Activo = 1 WHERE IdPersona = @Id");
                datos.setearParametro("@Id", idMedico);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


    }
}