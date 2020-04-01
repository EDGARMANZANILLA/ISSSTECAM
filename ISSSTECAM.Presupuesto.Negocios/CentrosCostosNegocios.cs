using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class CentrosCostosNegocios
    {
        public static IEnumerable<CentrosCostos> ObtenerActivos()
        {            
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<CentrosCostos>(transaccion);
            return repositorio.ObtenerPorFiltro(cc => cc.Activo == true);
        }

        public static CentrosCostos ObtenerPorClave(string clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<CentrosCostos>(transaccion);
            return repositorio.ObtenerPorFiltro(cc => cc.Activo == true && cc.Clave == clave).FirstOrDefault();
        }
    
    }
}
