using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class TurnoNegocio
    {

        private Turno MapearTurno(SqlDataReader lector)
        {
            Turno turno = new Turno();

            // Mapeo de campos directos de la tabla Turno
            turno.IdTurno = (int)lector["IdTurno"];
            turno.FechaHoraInicio = (DateTime)lector["FechaHoraInicio"];
            turno.FechaHoraFin = (DateTime)lector["FechaHoraFin"];

            // Mapeo de campos que pueden ser DBNull
            if (lector["MotivoDeConsulta"] != DBNull.Value)
                turno.MotivoDeConsulta = (string)lector["MotivoDeConsulta"];

            if (lector["Diagnostico"] != DBNull.Value)
                turno.Diagnostico = (string)lector["Diagnostico"];

            // Mapeo de EstadoTurno (Enum)
            if (lector["EstadoTurno"] != DBNull.Value)
            {
                string estadoString = lector["EstadoTurno"].ToString();
                EstadoTurno estado;
                if (Enum.TryParse(estadoString, true, out estado))
                {
                    turno.EstadoTurno = estado;
                }
            }

            turno.IdMedico = (int)lector["IdMedico"];
            turno.Medico = new Medico
            {
                Nombre = (string)lector["NombreMedico"],
                Apellido = (string)lector["ApellidoMedico"]
            };

            turno.IdPaciente = (int)lector["IdPaciente"];
            turno.Paciente = new Paciente
            {
                Nombre = (string)lector["NombrePaciente"],
                Apellido = (string)lector["ApellidoPaciente"]
            };

            return turno;
        }

        private string ObtenerNombreDiaExacto(DayOfWeek diaIngles)
        {
            // Tu base de datos tiene un CHECK que obliga a usar Mayúscula inicial
            switch (diaIngles)
            {
                case DayOfWeek.Monday: return "Lunes";
                case DayOfWeek.Tuesday: return "Martes";
                case DayOfWeek.Wednesday: return "Miercoles"; // Sin tilde según tu script SQL
                case DayOfWeek.Thursday: return "Jueves";
                case DayOfWeek.Friday: return "Viernes";
                case DayOfWeek.Saturday: return "Sabado";     // Sin tilde según tu script SQL
                case DayOfWeek.Sunday: return "Domingo";
                default: return "";
            }
        }

        public List<string> ObtenerHorariosDisponibles(int idMedico, DateTime fecha)
        {
            List<string> horariosDisponibles = new List<string>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string nombreDia = ObtenerNombreDiaExacto(fecha.DayOfWeek);
                TimeSpan horaInicio = TimeSpan.Zero;
                TimeSpan horaFin = TimeSpan.Zero;
                bool trabaja = false;

                // 1. Buscamos el horario de trabajo para ese día específico
                datos.setearConsulta(@"
                SELECT 
                    COALESCE(JL.HoraInicio, TT.HoraInicioDefault) AS Inicio,
                    COALESCE(JL.HoraFin, TT.HoraFinDefault) AS Fin
                FROM JornadaLaboral JL
                LEFT JOIN TurnoDeTrabajo TT ON JL.IdTurnoTrabajo = TT.IdTurnoTrabajo
                WHERE JL.IdMedico = @IdMedico AND JL.DiaLaboral = @Dia");

                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@Dia", nombreDia);

                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    trabaja = true;
                    horaInicio = (TimeSpan)datos.Lector["Inicio"];
                    horaFin = (TimeSpan)datos.Lector["Fin"];
                }
                datos.cerrarConexion(); // Cerramos para poder ejecutar otra consulta

                if (!trabaja) return horariosDisponibles; // Lista vacía

                // 2. Buscamos qué horas ya están ocupadas
                // (Hacemos la consulta manual aquí para no depender de llamar a otro método y abrir/cerrar conexiones extra)
                List<TimeSpan> ocupados = new List<TimeSpan>();
                datos = new AccesoDatos(); // Nueva instancia limpia
                datos.setearConsulta(@"
                SELECT CAST(FechaHoraInicio AS TIME) as Hora 
                FROM Turno 
                WHERE IdMedico = @IdMedico 
                  AND CAST(FechaHoraInicio AS DATE) = CAST(@Fecha AS DATE)
                  AND EstadoTurno != 'Cancelado'");

                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@Fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ocupados.Add((TimeSpan)datos.Lector["Hora"]);
                }
                datos.cerrarConexion();

                // 3. Calculamos los huecos (Lógica del While)
                TimeSpan horaActual = horaInicio;
                while (horaActual < horaFin)
                {
                    // Si la hora NO está en la lista de ocupados, la agregamos
                    if (!ocupados.Contains(horaActual))
                    {
                        horariosDisponibles.Add(horaActual.ToString(@"hh\:mm"));
                    }
                    // Sumamos 1 hora
                    horaActual = horaActual.Add(new TimeSpan(1, 0, 0));
                }

                return horariosDisponibles;
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
    
        //LISTAR TODOS 
        public List<Turno> Listar()
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Consulta JOIN para obtener los nombres de Médico y Paciente
                datos.setearConsulta(@"
                    SELECT 
                        T.IdTurno, T.FechaHoraInicio, T.FechaHoraFin, T.MotivoDeConsulta, 
                        T.Diagnostico, T.EstadoTurno, 
                        T.IdMedico, M.Nombre AS NombreMedico, M.Apellido AS ApellidoMedico,
                        T.IdPaciente, P.Nombre AS NombrePaciente, P.Apellido AS ApellidoPaciente
                    FROM Turno T
                    INNER JOIN Persona M ON T.IdMedico = M.IdPersona
                    INNER JOIN Persona P ON T.IdPaciente = P.IdPersona");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(MapearTurno(datos.Lector));
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //AGREGAR 
        public void Agregar(Turno nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO Turno (
                        FechaHoraInicio, FechaHoraFin, MotivoDeConsulta, 
                        Diagnostico, EstadoTurno, IdMedico, IdPaciente
                    )
                    VALUES (
                        @FechaHoraInicio, @FechaHoraFin, @MotivoDeConsulta, 
                        @Diagnostico, @EstadoTurno, @IdMedico, @IdPaciente
                    )");

                // Seteo de parámetros
                datos.setearParametro("@FechaHoraInicio", nuevo.FechaHoraInicio);
                datos.setearParametro("@FechaHoraFin", nuevo.FechaHoraFin);

                // Manejo de posibles nulos
                datos.setearParametro("@MotivoDeConsulta", string.IsNullOrEmpty(nuevo.MotivoDeConsulta) ? (object)DBNull.Value : nuevo.MotivoDeConsulta);
                datos.setearParametro("@Diagnostico", string.IsNullOrEmpty(nuevo.Diagnostico) ? (object)DBNull.Value : nuevo.Diagnostico);

                // Guardar el enum como string
                datos.setearParametro("@EstadoTurno", nuevo.EstadoTurno.ToString());

                datos.setearParametro("@IdMedico", nuevo.IdMedico);
                datos.setearParametro("@IdPaciente", nuevo.IdPaciente);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el turno", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // LISTAR POR FECHA (Para cargar la grilla principal por día)
        public List<Turno> ListarPorFecha(DateTime fecha)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Consulta similar a Listar(), pero filtrando por la fecha de inicio
                datos.setearConsulta(@"
                    SELECT 
                        T.IdTurno, T.FechaHoraInicio, T.FechaHoraFin, T.MotivoDeConsulta, 
                        T.Diagnostico, T.EstadoTurno, 
                        T.IdMedico, M.Nombre AS NombreMedico, M.Apellido AS ApellidoMedico,
                        T.IdPaciente, P.Nombre AS NombrePaciente, P.Apellido AS ApellidoPaciente
                    FROM Turno T
                    INNER JOIN Persona M ON T.IdMedico = M.IdPersona
                    INNER JOIN Persona P ON T.IdPaciente = P.IdPersona
                    WHERE CONVERT(DATE, T.FechaHoraInicio) = CONVERT(DATE, @Fecha)
                    ORDER BY T.FechaHoraInicio");

                datos.setearParametro("@Fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(MapearTurno(datos.Lector));
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar turnos por fecha", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //LISTAR TURNOS OCUPADOS 
        public List<Turno> ListarTurnosOcupados(int idMedico, DateTime fecha)
        {
            List<Turno> lista = new List<Turno>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Solo se necesitan las horas para chequear solapamiento
                datos.setearConsulta(@"
                    SELECT 
                        FechaHoraInicio, FechaHoraFin 
                    FROM Turno 
                    WHERE IdMedico = @IdMedico 
                      AND CONVERT(DATE, FechaHoraInicio) = CONVERT(DATE, @Fecha)
                      AND EstadoTurno != 'Cancelado'"); // Se considera "ocupado" si no está cancelado

                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@Fecha", fecha);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Turno turno = new Turno();
                    turno.FechaHoraInicio = (DateTime)datos.Lector["FechaHoraInicio"];
                    turno.FechaHoraFin = (DateTime)datos.Lector["FechaHoraFin"];
                    lista.Add(turno);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar turnos ocupados", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Turno ListarPorId(int idTurno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
            SELECT 
                T.IdTurno, T.FechaHoraInicio, T.FechaHoraFin, T.MotivoDeConsulta, 
                T.Diagnostico, T.EstadoTurno, 
                T.IdMedico, M.Nombre AS NombreMedico, M.Apellido AS ApellidoMedico,
                T.IdPaciente, P.Nombre AS NombrePaciente, P.Apellido AS ApellidoPaciente
            FROM Turno T
            INNER JOIN Persona M ON T.IdMedico = M.IdPersona
            INNER JOIN Persona P ON T.IdPaciente = P.IdPersona
            WHERE T.IdTurno = @IdTurno");

                datos.setearParametro("@IdTurno", idTurno);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return MapearTurno(datos.Lector);
                }

                return null; // Retorna null si no se encuentra
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el turno por ID", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Turno turno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta(@"
            UPDATE Turno SET
                FechaHoraInicio = @FechaHoraInicio, 
                FechaHoraFin = @FechaHoraFin, 
                MotivoDeConsulta = @MotivoDeConsulta, 
                Diagnostico = @Diagnostico,
                EstadoTurno = @EstadoTurno, 
                IdMedico = @IdMedico, 
                IdPaciente = @IdPaciente
            WHERE IdTurno = @IdTurno"); // El IdTurno es la clave para saber qué fila actualizar

               
                datos.setearParametro("@FechaHoraInicio", turno.FechaHoraInicio);
                datos.setearParametro("@FechaHoraFin", turno.FechaHoraFin);


                datos.setearParametro("@MotivoDeConsulta", string.IsNullOrEmpty(turno.MotivoDeConsulta) ? (object)DBNull.Value : turno.MotivoDeConsulta);
                datos.setearParametro("@Diagnostico", string.IsNullOrEmpty(turno.Diagnostico) ? (object)DBNull.Value : turno.Diagnostico);

              
                datos.setearParametro("@EstadoTurno", turno.EstadoTurno.ToString());

                datos.setearParametro("@IdMedico", turno.IdMedico);
                datos.setearParametro("@IdPaciente", turno.IdPaciente);
                datos.setearParametro("@IdTurno", turno.IdTurno); // Parámetro del WHERE

          
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el turno en la base de datos", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
