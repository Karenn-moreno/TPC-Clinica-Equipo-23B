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
                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Email, P.Telefono,
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
                            STRING_AGG(
                                CONCAT(
                                    JL.DiaLaboral,  
                                    ' ', 
                                    CONVERT(VARCHAR(5), ISNULL(JL.HoraInicio, TT.HoraInicioDefault), 108),
                                    '-', 
                                    CONVERT(VARCHAR(5), ISNULL(JL.HoraFin, TT.HoraFinDefault), 108)
                                ), 
                                ', '
                            ) 
                            WITHIN GROUP (ORDER BY 
                                CASE JL.DiaLaboral
                                    WHEN 'Lunes' THEN 1
                                    WHEN 'Martes' THEN 2
                                    WHEN 'Miercoles' THEN 3  
                                    WHEN 'Jueves' THEN 4
                                    WHEN 'Viernes' THEN 5
                                    WHEN 'Sabado' THEN 6    
                                    WHEN 'Domingo' THEN 7
                                END
                            ) AS ListaHorarios
                        FROM (
                            SELECT DISTINCT DiaLaboral, HoraInicio, HoraFin, IdTurnoTrabajo
                            FROM JornadaLaboral
                            WHERE IdMedico = M.IdMedico
                        ) AS JL
                        LEFT JOIN TurnoDeTrabajo TT ON JL.IdTurnoTrabajo = TT.IdTurnoTrabajo
                    ) AS J
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

                //  Insertar Especialidades
                if (!string.IsNullOrEmpty(nuevo.EspecialidadesTexto))
                {
                    string[] especialidades = nuevo.EspecialidadesTexto
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var esp in especialidades)
                    {
                        AgregarEspecialidad(idPersona, esp.Trim());
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

        // Nuevo método agregado
        public void AgregarEspecialidad(int idMedico, string nombreEspecialidad)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                    INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad)
                    VALUES (@IdMedico, (SELECT IdEspecialidad FROM Especialidad WHERE Nombre = @Nombre))
                ");
                datos.setearParametro("@IdMedico", idMedico);
                datos.setearParametro("@Nombre", nombreEspecialidad);
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
    }
}