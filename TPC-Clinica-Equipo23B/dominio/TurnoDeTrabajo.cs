using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class TurnoDeTrabajo
    {
        public int IdTurnoDeTrabajo { get; set; }
        public string TipoDeTurno { get; set; }
        public TimeSpan HoraInicioDefault { get; set; }
        public TimeSpan HoraFinDefault { get; set; }
        public ICollection<JornadaLaboral> JornadasLaborales { get; set; } = new List<JornadaLaboral>();
    }
}
