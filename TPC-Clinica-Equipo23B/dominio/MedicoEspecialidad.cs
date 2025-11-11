using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class MedicoEspecialidad
    {
        public int IdMedico { get; set; }
        public Medico Medico { get; set; }
        public int IdEspecialidad { get; set; }
        public Especialidad Especialidad { get; set; }
    }
}
