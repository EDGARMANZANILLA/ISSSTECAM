using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class ProyectosNegocios
    {
        public static IEnumerable<Proyectos> ObtenerActivosDelAnio(int anio)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Proyectos>(transaccion);
            return repositorio.ObtenerPorFiltro(p => p.Activo == true && p.Anio == anio);
        }

        public static Proyectos ObtenerActivosDelAnioPorClave(int anio, string clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Proyectos>(transaccion);
            return repositorio.ObtenerPorFiltro(p => p.Activo == true && p.Anio == anio && p.Clave == clave).FirstOrDefault();
        }
    }
}
