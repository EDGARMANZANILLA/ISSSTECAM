//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISSSTECAM.Presupuesto.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposEmpleados
    {
        public TiposEmpleados()
        {
            this.Conceptos = new HashSet<Conceptos>();
        }
    
        public int Id { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }
    
        public virtual ICollection<Conceptos> Conceptos { get; set; }
    }
}
