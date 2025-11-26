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
        public string HorariosTexto { get; set; }

        public string Email { get; set; }//agregado ani
        public string Telefono { get; set; }//agregado ani


    }

}
