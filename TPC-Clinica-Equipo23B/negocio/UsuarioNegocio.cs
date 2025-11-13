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
                // NOTA IMPORTANTE: En un entorno de producción, la validación de contraseña 
                // DEBE hacerse comparando el HASH almacenado en la DB con el HASH de la 
                // contraseña proporcionada, no con la contraseña en texto plano.

                datos.setearConsulta(@"
                    SELECT 
                        P.IdPersona, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono, P.Localidad, 
                        U.Password
                    FROM Persona P
                    INNER JOIN Usuario U ON P.IdPersona = U.IdUsuario
                    WHERE P.Email = @Email AND U.Password = @Password");

                datos.setearParametro("@Email", email);
                datos.setearParametro("@Password", password);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return MapearUsuario(datos.Lector);
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

      


}
}
