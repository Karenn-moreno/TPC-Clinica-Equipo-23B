using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class PacienteNegocio
    {

        public List<Paciente> Listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Dni, 
                        P.Email, P.Telefono, P.Localidad, 
                        PA.FechaNacimiento 
                    FROM 
                        Persona P
                    INNER JOIN 
                        Paciente PA ON P.IdPersona = PA.IdPaciente");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente paciente = new Paciente();
                    paciente.IdPersona = (int)datos.Lector["IdPersona"];
                    paciente.Nombre = (string)datos.Lector["Nombre"];
                    paciente.Apellido = (string)datos.Lector["Apellido"];

                    // Manejo de valores nulos o DBNull
                    if (datos.Lector["Dni"] != DBNull.Value)
                        paciente.Dni = (string)datos.Lector["Dni"];

                    if (datos.Lector["Email"] != DBNull.Value)
                        paciente.Email = (string)datos.Lector["Email"];

                    if (datos.Lector["Telefono"] != DBNull.Value)
                        paciente.Telefono = (string)datos.Lector["Telefono"];

                    if (datos.Lector["Localidad"] != DBNull.Value)
                        paciente.Localidad = (string)datos.Lector["Localidad"];

                    // Fecha de Nacimiento (específica del Paciente)
                    if (datos.Lector["FechaNacimiento"] != DBNull.Value)
                        paciente.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];

                    lista.Add(paciente);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pacientes", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // AGREGAR
        public void Agregar(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // insertar en la tabla Persona y obtener el ID
                datos.setearConsulta(@"
                    INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
                    VALUES (@Nombre, @Apellido, @Dni, @Email, @Telefono, @Localidad);
                    SELECT SCOPE_IDENTITY() AS IdNuevo;");

                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Apellido", nuevo.Apellido);
                datos.setearParametro("@Dni", nuevo.Dni);
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Telefono", nuevo.Telefono);
                datos.setearParametro("@Localidad", nuevo.Localidad);

                datos.ejecutarLectura();

                int idPersona = 0;
                if (datos.Lector.Read())
                {
                    idPersona = Convert.ToInt32(datos.Lector["IdNuevo"]);
                }

                datos.cerrarConexion();

                //Insertar en la tabla Paciente usando el Id de Persona
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Paciente (IdPaciente, FechaNacimiento) VALUES (@IdPaciente, @FechaNacimiento)");
                datos.setearParametro("@IdPaciente", idPersona);
                datos.setearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar paciente", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // MODIFICAR
        public void Modificar(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Actualizar la tabla Persona
                datos.setearConsulta(@"
                    UPDATE Persona 
                    SET Nombre = @Nombre, Apellido = @Apellido, Dni = @Dni, 
                        Email = @Email, Telefono = @Telefono, Localidad = @Localidad
                    WHERE IdPersona = @Id");

                datos.setearParametro("@Nombre", paciente.Nombre);
                datos.setearParametro("@Apellido", paciente.Apellido);
                datos.setearParametro("@Dni", paciente.Dni);
                datos.setearParametro("@Email", paciente.Email);
                datos.setearParametro("@Telefono", paciente.Telefono);
                datos.setearParametro("@Localidad", paciente.Localidad);
                datos.setearParametro("@Id", paciente.IdPersona);
                datos.ejecutarAccion();

                datos.cerrarConexion();

                // 2. Actualizar la tabla Paciente
                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE Paciente SET FechaNacimiento = @FechaNacimiento WHERE IdPaciente = @Id");
                datos.setearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.setearParametro("@Id", paciente.IdPersona);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar paciente", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // 4. ELIMINAR
        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Eliminar de Paciente (por la FK)
                datos.setearConsulta("DELETE FROM Paciente WHERE IdPaciente = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // Eliminar de Persona
                datos = new AccesoDatos();
                datos.setearConsulta("DELETE FROM Persona WHERE IdPersona = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar paciente", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
