using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class ActividadesNegocios
    {
        public static IEnumerable<Actividades> ObtenerActivosDelAnio(int anio)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Actividades>(transaccion);
            return repositorio.ObtenerPorFiltro(a => a.Activo == true && a.Anio == anio);
        }

        public static Actividades ObtenerActivosDelAnioPorClave(int anio, string clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Actividades>(transaccion);
            return repositorio.ObtenerPorFiltro(a => a.Activo == true && a.Anio == anio && a.Clave == clave).FirstOrDefault();
        }
    }
}
