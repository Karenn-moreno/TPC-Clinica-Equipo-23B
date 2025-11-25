using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class UsuarioNegocio
    {
        private Usuario MapearUsuario(SqlDataReader lector)
        {
            Usuario usuario = new Usuario();

            // Mapeo de Persona
            usuario.IdPersona = (int)lector["IdPersona"];
            usuario.Nombre = (string)lector["Nombre"];
            usuario.Apellido = (string)lector["Apellido"];
            usuario.Email = (string)lector["Email"];

            // Campos que pueden ser DBNull en Persona
            if (lector["Dni"] != DBNull.Value)
                usuario.Dni = (string)lector["Dni"];
            if (lector["Telefono"] != DBNull.Value)
                usuario.Telefono = (string)lector["Telefono"];
            if (lector["Localidad"] != DBNull.Value)
                usuario.Localidad = (string)lector["Localidad"];

            // Mapeo de Usuario
            usuario.Password = (string)lector["Password"];



            return usuario;
        }

        public Usuario ValidarUsuario(string email, string password)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {


                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono, P.Localidad, 
                        U.Password,
                        R.IdRol,
                        R.TipoRol
                    FROM Persona P
                    INNER JOIN Usuario U ON P.IdPersona = U.IdUsuario
                    INNER JOIN UsuarioRol UR ON U.IdUsuario = UR.IdUsuario
                    INNER JOIN Rol R ON UR.IdRol = R.IdRol
                    WHERE P.Email = @Email AND U.Password = @Password");

                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Usuario usuario = MapearUsuario(datos.Lector);

                    UsuarioRol usuarioRol = new UsuarioRol();
                    usuarioRol.Rol = new Rol();

                    if (!(datos.Lector["IdRol"] is DBNull))
                    usuarioRol.Rol.IdRol = (int)datos.Lector["IdRol"];
                    if (!(datos.Lector["TipoRol"] is DBNull))
                    usuarioRol.Rol.TipoRol = (string)datos.Lector["TipoRol"];

                    usuario.UsuarioRoles.Add(usuarioRol);
                    return usuario;
                }

                return null; // Credenciales inválidas
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el usuario.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Método auxiliar para buscar un usuario por su ID (útil para la sesión)
        public Usuario BuscarPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono, P.Localidad, 
                        U.Password
                    FROM Persona P
                    INNER JOIN Usuario U ON P.IdPersona = U.IdUsuario
                    WHERE P.IdPersona = @Id");

                datos.setearParametro("@Id", id);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return MapearUsuario(datos.Lector);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el usuario por ID.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Verifica si ya existe una Persona con ese Email o Dni.
        public bool ExisteUsuario(string email, string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT IdPersona FROM Persona WHERE Email = @Email OR Dni = @Dni");
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Dni", dni);

                datos.ejecutarLectura();

                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la existencia del usuario.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // MÉTODO DE REGISTRO UNIFICADO PARA ROLES ADMINISTRATIVOS
        public void RegistrarNuevoUsuarioConRol(Persona nuevaPersona, string password, int idRol, string matricula = null)
        {
            AccesoDatos datos = new AccesoDatos();
            int idPersona = 0;

            try
            {
                // 1. Insertar en Persona y obtener el ID
                datos.setearConsulta(@"
                    INSERT INTO Persona (Nombre, Apellido, Dni, Email, Telefono, Localidad)
                    VALUES (@Nombre, @Apellido, @Dni, @Email, @Telefono, @Localidad);
                    SELECT SCOPE_IDENTITY() AS IdNuevo;
                ");

                datos.setearParametro("@Nombre", nuevaPersona.Nombre);
                datos.setearParametro("@Apellido", nuevaPersona.Apellido);
                datos.setearParametro("@Dni", nuevaPersona.Dni);
                datos.setearParametro("@Email", nuevaPersona.Email);
                datos.setearParametro("@Telefono", string.IsNullOrEmpty(nuevaPersona.Telefono) ? DBNull.Value : (object)nuevaPersona.Telefono);
                datos.setearParametro("@Localidad", string.IsNullOrEmpty(nuevaPersona.Localidad) ? DBNull.Value : (object)nuevaPersona.Localidad);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    idPersona = Convert.ToInt32(datos.Lector["IdNuevo"]);
                }

                datos.cerrarConexion();

                // 2. Insertar en Usuario
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO Usuario (IdUsuario, Password) VALUES (@IdUsuario, @Password)");
                datos.setearParametro("@IdUsuario", idPersona);
                datos.setearParametro("@Password", password);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                // 3. Insertar en tabla específica (Medico) si el rol es Médico (ID = 2)
                if (idRol == 2)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta("INSERT INTO Medico (IdMedico, Matricula) VALUES (@IdMedico, @Matricula)");
                    datos.setearParametro("@IdMedico", idPersona);
                    datos.setearParametro("@Matricula", matricula);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();
                }

                // 4. Asignar Rol
                datos = new AccesoDatos();
                datos.setearConsulta("INSERT INTO UsuarioRol (IdUsuario, IdRol) VALUES (@IdUsuario, @IdRol)");
                datos.setearParametro("@IdUsuario", idPersona);
                datos.setearParametro("@IdRol", idRol);
                datos.ejecutarAccion();
                datos.cerrarConexion();

            }
            catch (Exception ex)
            {
                throw new Exception("Error al completar el registro del usuario con rol.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Rol> ListarRoles()
        {
            List<Rol> lista = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.setearConsulta("SELECT IdRol, TipoRol FROM Rol WHERE IdRol IN (1, 2, 3)");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Rol rol = new Rol();
                    rol.IdRol = (int)datos.Lector["IdRol"];
                    rol.TipoRol = (string)datos.Lector["TipoRol"];
                    lista.Add(rol);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar roles.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    
}
}
