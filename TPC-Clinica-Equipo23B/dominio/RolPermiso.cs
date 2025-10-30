using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class RolPermiso
    {
        public int IdRol { get; set; }
        public Rol Rol { get; set; }
        public int IdPermiso { get; set; }
        public Permiso Permiso { get; set; }

    }
}
