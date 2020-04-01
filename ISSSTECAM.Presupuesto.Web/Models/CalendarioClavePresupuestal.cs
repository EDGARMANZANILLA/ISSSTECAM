using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISSSTECAM.Presupuesto.Web.Models
{
    public class CalendarioClavePresupuestal
    {
        public string Clave { get; set; }
        public string Concepto { get; set; }
        public string Partida
        {
            get
            {
                return Clave.Substring(38, 4);
            }
        }
        public decimal PresupuestoAutorizado {
            get
            {
                return MontoEnero + MontoFebrero + MontoMarzo + MontoAbril + MontoMayo + MontoJunio + 
                    MontoJulio + MontoAgosto + MontoSeptiembre + MontoOctubre + MontoNoviembre + MontoDiciembre; 
            }
        }
        public decimal MontoEnero { get; set; }
        public decimal MontoFebrero { get; set; }
        public decimal MontoMarzo { get; set; }
        public decimal MontoAbril { get; set; }
        public decimal MontoMayo { get; set; }
        public decimal MontoJunio { get; set; }
        public decimal MontoJulio { get; set; }
        public decimal MontoAgosto { get; set; }
        public decimal MontoSeptiembre { get; set; }
        public decimal MontoOctubre { get; set; }
        public decimal MontoNoviembre { get; set; }
        public decimal MontoDiciembre { get; set; }
    }
}