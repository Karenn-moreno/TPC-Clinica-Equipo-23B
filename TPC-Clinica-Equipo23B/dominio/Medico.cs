using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
     public class Medico
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; }

        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
    }
}
