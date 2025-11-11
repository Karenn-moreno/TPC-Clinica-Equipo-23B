using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Paciente : Persona
    {
        
        public DateTime FechaNacimiento { get; set; } //date?
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();

    }
}
