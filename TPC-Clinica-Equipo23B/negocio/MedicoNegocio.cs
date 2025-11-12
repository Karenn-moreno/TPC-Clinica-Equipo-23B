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
    }
}

