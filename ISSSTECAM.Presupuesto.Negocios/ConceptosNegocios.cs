using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class ConceptosNegocios
    {
        public static IEnumerable<Conceptos> ObtenerActivos()
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Conceptos>(transaccion);
            return repositorio.ObtenerPorFiltro(a => a.Activo == true);
        }
    }
}
