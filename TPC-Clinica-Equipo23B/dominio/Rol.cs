using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string TipoRol { get; set; }
        public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
        public ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    }
}
