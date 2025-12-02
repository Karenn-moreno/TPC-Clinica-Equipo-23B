using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class TurnoDeTrabajoNegocio
    {
        public List<TurnoDeTrabajo> listar()
        {
            List<TurnoDeTrabajo> lista = new List<TurnoDeTrabajo>();
            AccesoDatos datos = new AccesoDatos();
            
            try
            {
                datos.setearConsulta(@"
                    SELECT IdTurnoTrabajo, TipoDeTurno, HoraInicioDefault, HoraFinDefault 
                    FROM TurnoDeTrabajo");

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    TurnoDeTrabajo turno = new TurnoDeTrabajo();
                    turno.IdTurnoTrabajo = (int)datos.Lector["IdTurnoTrabajo"];
                    turno.TipoDeTurno = (string)datos.Lector["TipoDeTurno"];
                    turno.HoraInicioDefault = (TimeSpan)datos.Lector["HoraInicioDefault"];
                    turno.HoraFinDefault = (TimeSpan)datos.Lector["HoraFinDefault"];

                    lista.Add(turno);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los turnos de trabajo", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public TurnoDeTrabajo obtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT IdTurnoTrabajo, TipoDeTurno, HoraInicioDefault, HoraFinDefault FROM TurnoDeTrabajo WHERE IdTurnoTrabajo = @id");
                datos.setearParametro("@id", id);

                datos.ejecutarLectura();

                TurnoDeTrabajo turno = null;

                if (datos.Lector.Read())
                {
                    turno = new TurnoDeTrabajo();

                    // Mapeo
                    turno.IdTurnoTrabajo = (int)datos.Lector["IdTurnoTrabajo"];

                    if (!(datos.Lector["TipoDeTurno"] is DBNull))
                        turno.TipoDeTurno = (string)datos.Lector["TipoDeTurno"];

                    turno.HoraInicioDefault = (TimeSpan)datos.Lector["HoraInicioDefault"];
                    turno.HoraFinDefault = (TimeSpan)datos.Lector["HoraFinDefault"];
                }

                return turno;
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
