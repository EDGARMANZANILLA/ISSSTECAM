using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSSTECAM.Presupuesto.Web.Models
{
    public class ClavesPresupuestalesSolicitudes
    {
        public int IdSolicitud { get; set; }
        public string Clave { get; set; }
        public decimal MontoAUsar { get; set; }
    }
}