using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string MotivoDeConsulta { get; set; }
        public string Diagnostico { get; set; }
        public EstadoTurno EstadoTurno { get; set; }


    }
    public enum EstadoTurno
    {
        Nuevo,
        Reprogramado,
        Cancelado,
        NoAsistio,
        Cerrado
    }
}
