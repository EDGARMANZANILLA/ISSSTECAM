using ISSSTECAM.Presupuesto.Datos;
using ISSSTECAM.Presupuesto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISSSTECAM.Presupuesto.Negocios
{
    public class ClavesPresupuestalesNegocios
    {
        public static ClavesPresupuestales Agregar(ClavesPresupuestales clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
            repositorio.Agregar(clave);
            transaccion.GuardarCambios();
            return clave;
        }

        public static List<ClavesPresupuestales> Agregar(List<ClavesPresupuestales> claves)
        {
            var transaccion = new Transaccion();
            try
            {
                var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
                foreach (var clave in claves)
                {
                    repositorio.Agregar(clave);
                }
                transaccion.GuardarCambios();
            }
            catch (Exception)
            {
                transaccion.Dispose();
            }
            return claves;
        }

        public static bool ExistenClavesParaAnio(int anio)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
            return repositorio.ObtenerPorFiltro(c => c.Anio == anio).Count() > 0;
        }

        public static IEnumerable<ClavesPresupuestales> ObtenerClavesActivasPorAnio(int anio)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
            return repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Activo == true);
        }

        public static IEnumerable<ClavesPresupuestales> ObtenerClavesActivas(string claveRamo, string claveUnidad, string claveCentroCosto, DateTime fecha)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
            return repositorio.ObtenerPorFiltro(c => c.CentrosCostos.Clave == claveCentroCosto && c.Anio == fecha.Year && c.Activo == true);
        }

        public static decimal ObtenerDisponibleClaveParaAnio(int idClavePresupuestal, DateTime fecha)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);
            var clave =
                repositorio.ObtenerPorFiltro(c => c.Anio == fecha.Year && c.Activo == true && c.Id == idClavePresupuestal).FirstOrDefault();
            if (clave != null)
                return clave.ObtenerMontoPorMes(fecha.Month);
            else
                throw new ApplicationException("No existe dicha clave");
        }
    }
}