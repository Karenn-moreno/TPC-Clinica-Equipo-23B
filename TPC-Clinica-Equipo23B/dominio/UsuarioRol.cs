﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioRol
    {
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int IdRol { get; set; }
        public Rol Rol { get; set; }
    }
}
