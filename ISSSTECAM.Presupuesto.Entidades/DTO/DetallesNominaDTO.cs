using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class DetallesNominaDTO
    {
        public int Id { get; set; }
        //public int IdNomina { get; set; }
        public int IdEmpleado { get; set; }
        public int IdTipoEmpleado { get; set; }
        public int IdConcepto { get; set; }
        public decimal Monto { get; set; }
        public string CentroCosto { get; set; }
    }
}
