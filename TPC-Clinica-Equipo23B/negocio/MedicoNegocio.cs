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
        -- Este bloque de Especialidades está bien
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
    FROM 
        JornadaLaboral JL
    
    LEFT JOIN 
        TurnoDeTrabajo TT ON JL.IdTurnoTrabajo = TT.IdTurnoTrabajo
    WHERE 
        JL.IdMedico = M.IdMedico
    ) AS J ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico medico = new Medico();
                    medico.IdPersona = (int)datos.Lector["IdPersona"];
                    medico.Nombre = (string)datos.Lector["Nombre"];
                    medico.Apellido = (string)datos.Lector["Apellido"];
                    if (datos.Lector["Email"] != DBNull.Value)
                        medico.Email = (string)datos.Lector["Email"];
                    if (datos.Lector["Telefono"] != DBNull.Value)
                        medico.Telefono = (string)datos.Lector["Telefono"];

                    medico.EspecialidadesTexto = (string)datos.Lector["EspecialidadesTexto"];
                    medico.HorariosTexto = (string)datos.Lector["HorariosTexto"];


                    lista.Add(medico);
                }
                return lista;
            }
            catch (Exception ex) { throw new Exception("Error al listar médicos", ex); }
            finally { datos.cerrarConexion(); }
        }


        // agrega
        public void agregar(Medico nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // inserta primero en Persona
                datos.setearConsulta(@"
            INSERT INTO Persona (Nombre, Apellido, Email, Telefono)
            VALUES (@Nombre, @Apellido, @Email, @Telefono);
            SELECT SCOPE_IDENTITY() AS IdNuevo;");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Telefono", nuevo.Telefono);

                datos.ejecutarLectura();

                int idPersona = 0;
                if (datos.Lector.Read())
                {
                    idPersona = Convert.ToInt32(datos.Lector["IdNuevo"]);
                }

                datos.cerrarConexion();

                // inserta en medico usando el Id de Persona
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Medico (IdMedico, Matricula) VALUES (@IdMedico, @Matricula)");
                datos.setearParametro("@IdMedico", idPersona);
                datos.setearParametro("@Matricula", nuevo.Matricula);
                datos.ejecutarAccion();

                // inserta las Especialidades asociadas 
                if (nuevo.MedicoEspecialidades != null && nuevo.MedicoEspecialidades.Count > 0)
                {
                    foreach (var me in nuevo.MedicoEspecialidades)
                    {
                        datos = new AccesoDatos();
                        datos.setearConsulta("INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES (@IdMedico, @IdEspecialidad)");
                        datos.setearParametro("@IdMedico", idPersona);
                        datos.setearParametro("@IdEspecialidad", me.IdEspecialidad);
                        datos.ejecutarAccion();
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


        //modificar
        public void modificar(Medico medico)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // actualiza
                datos.setearConsulta(@"
                    UPDATE Persona 
                    SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Telefono = @Telefono
                    WHERE IdPersona = @Id");

                datos.setearParametro("@Nombre", medico.Nombre);
                datos.setearParametro("@Apellido", medico.Apellido);
                datos.setearParametro("@Email", medico.Email);
                datos.setearParametro("@Telefono", medico.Telefono);
                datos.setearParametro("@Id", medico.IdPersona);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // actualiza matricula en tabla medico
                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE Medico SET Matricula = @Matricula WHERE IdMedico = @Id");
                datos.setearParametro("@Matricula", medico.Matricula);
                datos.setearParametro("@Id", medico.IdPersona);
                datos.ejecutarAccion();
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


        // eliminar
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // primero elimina de Medico por FK
                datos.setearConsulta("DELETE FROM Medico WHERE IdMedico = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // luego elimina de Persona
                datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM Persona WHERE IdPersona = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar médico", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

