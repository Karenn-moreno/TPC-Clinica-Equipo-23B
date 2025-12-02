using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
     public class Medico : Usuario
    {
        public string Matricula { get; set; }

        public ICollection<MedicoEspecialidad> MedicoEspecialidades { get; set; } = new List<MedicoEspecialidad>();
        public ICollection<JornadaLaboral> JornadasLaborales { get; set; } = new List<JornadaLaboral>();
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
        public string EspecialidadesTexto { get; set; }
        private string _horariosTexto;

        public string HorariosTexto
        {
            get
            {

                //    usamos la lógica inteligente de agrupación
                if (JornadasLaborales != null && JornadasLaborales.Count > 0)
                {
                    try
                    {
                        // agrupa por coincidencia de horas de Inicio y Fin
                        var grupos = JornadasLaborales
                            .GroupBy(j => new { j.HorarioInicio, j.HoraFin })
                            .Select(g => new
                            {
                                // une los días con comas
                                Dias = string.Join(", ", g.Select(x => x.DiaLaboral.ToString())),

                                // formatea la hora 
                                Horario = $"{g.Key.HorarioInicio.ToString(@"hh\:mm")} - {g.Key.HoraFin.ToString(@"hh\:mm")}"
                            });


                        return string.Join("<br/>", grupos.Select(x => $"<b>{x.Dias}</b>: {x.Horario}"));
                    }
                    catch
                    {
                        return "Error al procesar horarios";
                    }
                }
                return _horariosTexto ?? "Sin horarios asignados";
            }
            set
            {
                // Permite asignar el valor manualmente si fuera necesario
                _horariosTexto = value;
            }
        }
    



    }

}
