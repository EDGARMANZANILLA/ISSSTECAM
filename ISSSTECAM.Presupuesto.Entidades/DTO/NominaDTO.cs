using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class NominaDTO
    {
        public int Id { get; set; }
        public System.DateTime FechaAplicacion { get; set; }
        public bool Activo { get; set; }

        public virtual ICollection<DetallesNominaDTO> DetallesNomina { get; set; }
    }
}
