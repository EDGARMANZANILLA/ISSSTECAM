using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class ProyectoDTO
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public int Anio { get; set; }
        public bool Activo { get; set; }
        public int IdConfiguracionProyecto { get; set; }

        public string DescripcionCompleta { get { return Clave + " " + Nombre; } }
    }
}
