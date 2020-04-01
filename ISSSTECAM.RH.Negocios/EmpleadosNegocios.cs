using ISSSTECAM.RH.Datos;
using ISSSTECAM.RH.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISSSTECAM.RH.Negocios
{
    public class EmpleadosNegocios
    {
        public static vst_EmpleadosRH ObtenerPorNumeroEmpleado(string numeroEmpleado)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<vst_EmpleadosRH>(transaccion);
            return repositorio.ObtenerPorFiltro(e => e.NumeroEmpleado == numeroEmpleado).FirstOrDefault();
        }

    }
}
