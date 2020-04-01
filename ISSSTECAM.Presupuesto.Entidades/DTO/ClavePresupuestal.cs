using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class ClavePresupuestal
    {
        public int IdClavePresupuestal { get; set; }
        public string Clave { get; set; }
        public decimal Disponible { get; set; }
    }
}
