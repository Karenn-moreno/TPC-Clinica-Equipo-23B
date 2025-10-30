using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class JornadaLaboral
    {
        public int IdJornadaLaboral { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public DiaLaboral DiaLaboral { get; set; }
    }

    public enum DiaLaboral
    {
        Lunes,
        Martes,
        Miercoles,
        Jueves,
        Viernes,
        Sabado,
        Domingo
    }


}
