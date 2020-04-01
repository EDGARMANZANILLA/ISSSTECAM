using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades.DTO
{
    public class CentroCostoDTO
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public bool Activo { get; set; }
        public string DescripcionCompleta { 
            get { 
                return Clave + " " + Nombre + 
                    (string.IsNullOrEmpty(Abreviatura) ? "" : " (" + Abreviatura + ")"); 
            } 
        }
    }
}
