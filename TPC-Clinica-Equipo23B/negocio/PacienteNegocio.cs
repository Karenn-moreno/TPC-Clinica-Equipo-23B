using dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    internal class PacienteNegocio
    {

        public List<Paciente> ListarPaciente()//Vicky se confundio es para la otra etapa, no prestarle atencion ahora
        {
            List<Paciente> lista = new List<Paciente>();

            // 1. CONEXIÓN: Apuntando a la base de datos ClinicaDB.
            SqlConnection conexion = new SqlConnection("Server=.\\SQLEXPRESS; Initial Catalog= ClinicaDB; Integrated Security=true;");
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector = null;

            try
            {
             
                comando.CommandText = @"
                SELECT 
                    P.IdPersona, P.Nombre, P.Apellido, P.Dni, P.Email, P.Telefono, P.Localidad, 
                    PA.FechaNacimiento 
                FROM 
                    Persona P 
                INNER JOIN 
                    Paciente PA ON P.IdPersona = PA.IdPaciente"; 
                comando.Connection = conexion;

             
                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
              
                    Paciente paciente = new Paciente();

                    // Mapeo de campos de la tabla Persona
                    paciente.IdPersona = (int)lector["IdPersona"];
                    // Como Paciente hereda de Persona, mapeamos los campos base
                    paciente.Nombre = (string)lector["Nombre"];
                    paciente.Apellido = (string)lector["Apellido"];
                    paciente.Dni = (string)lector["Dni"];

                    if (lector["Email"] != DBNull.Value)
                        paciente.Email = (string)lector["Email"];

                    if (lector["Telefono"] != DBNull.Value)
                        paciente.Telefono = (string)lector["Telefono"];

                    if (lector["Localidad"] != DBNull.Value)
                        paciente.Localidad = (string)lector["Localidad"];

                    paciente.FechaNacimiento = (DateTime)lector["FechaNacimiento"];

                    lista.Add(paciente);
                }

                return lista;
            }
            catch (Exception ex)
            {
                // Se lanza una excepción ante cualquier error
                throw new Exception("Error al listar pacientes", ex);
            }
            finally
            {
    
                if (lector != null)
                    lector.Close();

                if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                    conexion.Close();
            }
        }

    }
}
