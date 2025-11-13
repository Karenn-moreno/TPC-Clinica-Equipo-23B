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

    }
}
