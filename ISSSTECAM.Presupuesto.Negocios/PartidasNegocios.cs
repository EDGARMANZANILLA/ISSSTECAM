using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class PartidasNegocios
    {
        public static IEnumerable<Partidas> ObtenerActivas(string filtro = "")
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Partidas>(transaccion);
            if(string.IsNullOrEmpty(filtro))
                return repositorio.ObtenerPorFiltro(p => p.Activo == true);
            else
                return repositorio.ObtenerPorFiltro(p => p.Activo == true && (p.Descripcion.Contains(filtro) || p.Clave.Contains(filtro)));
        }

        public static Partidas ObtenerActivaPorClave(string clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<Partidas>(transaccion);
            return repositorio.ObtenerPorFiltro(p => p.Activo == true && p.Clave.Contains(clave)).FirstOrDefault();
        }

    }
}
