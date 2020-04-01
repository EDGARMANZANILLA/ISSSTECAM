using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Entidades
{
    public partial class CentrosCostos
    {
        public string ClaveNombre
        {
            get
            {
                return string.IsNullOrEmpty(Abreviatura) ? Nombre : Abreviatura + " " + Nombre;
            }
        }
    }
}
