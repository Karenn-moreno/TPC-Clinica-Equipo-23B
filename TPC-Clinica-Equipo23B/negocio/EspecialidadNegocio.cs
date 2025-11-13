using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class EspecialidadNegocio
    {
        //listar
        public List<Especialidad> listar()
        {
            List<Especialidad> lista = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT IdEspecialidad, Nombre
                    FROM Especialidad");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Especialidad especialidad = new Especialidad();
                    especialidad.IdEspecialidad = (int)datos.Lector["IdEspecialidad"];
                    especialidad.Nombre = (string)datos.Lector["Nombre"];

                    lista.Add(especialidad);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar las especialidades", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //agregar
        public void agregar(Especialidad nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Especialidad (Nombre) VALUES (@Nombre)");
                datos.setearParametro("@Nombre", nueva.Nombre);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la especialidad", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //modificar

        public void modificar(Especialidad especialidad)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Especialidad SET Nombre = @Nombre WHERE IdEspecialidad = @Id");
                datos.setearParametro("@Nombre", especialidad.Nombre);
                datos.setearParametro("@Id", especialidad.IdEspecialidad);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar la especialidad", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        //eliminar
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM Especialidad WHERE IdEspecialidad = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la especialidad", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
    

