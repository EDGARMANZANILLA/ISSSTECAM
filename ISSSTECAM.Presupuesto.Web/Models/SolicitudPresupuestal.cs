using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ISSSTECAM.Presupuesto.Web.Models
{
    public class SolicitudPresupuestal
    {
        [DisplayName("Fecha de la solicitud")]
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        [DisplayName("Quién solicita")]
        public string Solicita { get; set; }
        public List<ClavesPresupuestalesSolicitudes> DetallesClaves { get; set; }
    }
}