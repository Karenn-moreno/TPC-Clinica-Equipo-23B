using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class JornadaLaboralNegocio
    {
        public List<JornadaLaboral> ListarPorMedico(int idMedico)

        {

            List<JornadaLaboral> lista = new List<JornadaLaboral>();

            AccesoDatos datos = new AccesoDatos();

            try

            {

                datos.setearConsulta(@"

                    SELECT 

                        IdJornadaLaboral, HorarioInicio, HoraFin, 

                        DiaLaboral, IdMedico, IdTurnoTrabajo 

                    FROM JornadaLaboral 

                    WHERE IdMedico = @IdMedico

                    ORDER BY DiaLaboral, HorarioInicio");


                datos.setearParametro("@IdMedico", idMedico);


                datos.ejecutarLectura();


                while (datos.Lector.Read())

                {

                    JornadaLaboral jornada = new JornadaLaboral();

                    jornada.IdJornadaLaboral = (int)datos.Lector["IdJornadaLaboral"];

                    jornada.IdMedico = (int)datos.Lector["IdMedico"];

                    if (datos.Lector["HorarioInicio"] != DBNull.Value)

                        jornada.HorarioInicio = (TimeSpan)datos.Lector["HorarioInicio"];



                    if (datos.Lector["HoraFin"] != DBNull.Value)

                        jornada.HoraFin = (TimeSpan)datos.Lector["HoraFin"];


                    if (datos.Lector["DiaLaboral"] != DBNull.Value)

                    {

                        string diaString = (string)datos.Lector["DiaLaboral"];

                        DiaLaboral dia;


                        if (Enum.TryParse(diaString, true, out dia)) 

                        {

                            jornada.DiaLaboral = dia;

                        }

                        else

                        {

                            throw new Exception("Valor de DiaLaboral no reconocido en DB: " + diaString);

                        }

                    }


                    if (datos.Lector["IdTurnoTrabajo"] != DBNull.Value)

                        jornada.IdTurnoTrabajo = (int)datos.Lector["IdTurnoTrabajo"];



                    lista.Add(jornada);

                }

                return lista;

            }

            catch (Exception ex)

            {

                throw new Exception("Error al listar las jornadas del médico", ex);

            }

            finally

            {

                datos.cerrarConexion();

            }

        }


        public void AgregarJornada(JornadaLaboral nuevaJornada)

        {

            AccesoDatos datos = new AccesoDatos();

            try

            {

                datos.setearConsulta(@"

                    INSERT INTO JornadaLaboral 

                        (HorarioInicio, HoraFin, DiaLaboral, IdMedico, IdTurnoTrabajo) 

                    VALUES 

                        (@HorarioInicio, @HoraFin, @DiaLaboral, @IdMedico, @IdTurnoTrabajo)");



                datos.setearParametro("@HorarioInicio", nuevaJornada.HorarioInicio);

                datos.setearParametro("@HoraFin", nuevaJornada.HoraFin);


                datos.setearParametro("@DiaLaboral", nuevaJornada.DiaLaboral.ToString()); // Guarda "Lunes", "Martes", etc.


                datos.setearParametro("@IdMedico", nuevaJornada.IdMedico);


                object idTurno = nuevaJornada.IdTurnoTrabajo.HasValue ?

                                 (object)nuevaJornada.IdTurnoTrabajo.Value :

                                 DBNull.Value;

                datos.setearParametro("@IdTurnoTrabajo", idTurno);



                datos.ejecutarAccion();

            }

            catch (Exception ex)

            {

                throw new Exception("Error al agregar la jornada laboral", ex);

            }

            finally

            {

                datos.cerrarConexion();

            }

        }





        public bool ModificarJornada(JornadaLaboral jornadaModificada)

        {

            AccesoDatos datos = new AccesoDatos();

            try

            {

                datos.setearConsulta(@"

                    UPDATE JornadaLaboral SET

                        HorarioInicio = @HorarioInicio,

                        HoraFin = @HoraFin,

                        DiaLaboral = @DiaLaboral,

                        IdMedico = @IdMedico,

                        IdTurnoTrabajo = @IdTurnoTrabajo

                    WHERE 

                        IdJornadaLaboral = @IdJornadaLaboral");



                datos.setearParametro("@HorarioInicio", jornadaModificada.HorarioInicio);

                datos.setearParametro("@HoraFin", jornadaModificada.HoraFin);


                datos.setearParametro("@DiaLaboral", jornadaModificada.DiaLaboral.ToString());

                datos.setearParametro("@IdMedico", jornadaModificada.IdMedico);



                object idTurno = jornadaModificada.IdTurnoTrabajo.HasValue ? (object)jornadaModificada.IdTurnoTrabajo.Value : DBNull.Value;

                datos.setearParametro("@IdTurnoTrabajo", idTurno);





                datos.setearParametro("@IdJornadaLaboral", jornadaModificada.IdJornadaLaboral);



                int filasAfectadas = datos.ejecutarAccionInt();



                return filasAfectadas > 0;

            }

            catch (Exception ex)

            {

                throw new Exception("Error al modificar la jornada laboral", ex);

            }



        }

        public bool EliminarJornada(int idJornada)

        {

            AccesoDatos datos = new AccesoDatos();

            try

            {

                datos.setearConsulta("DELETE FROM JornadaLaboral WHERE IdJornadaLaboral = @IdJornada");

                datos.setearParametro("@IdJornada", idJornada);


                int filasAfectadas = datos.ejecutarAccionInt();



                return filasAfectadas > 0;

            }

            catch (Exception ex)

            {

                throw new Exception("Error al eliminar la jornada laboral", ex);

            }
        }
    }
}
