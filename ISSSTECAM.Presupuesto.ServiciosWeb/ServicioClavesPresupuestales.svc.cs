using ISSSTECAM.Presupuesto.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISSSTECAM.Presupuesto.ServiciosWeb
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IServicioClavesPresupuestales
    {
        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public IEnumerable<ClavePresupuestal> ObtenerClavesPresupuestalesComprasPorRamoUnidad(string cvRamo, string cvUnidad, string cvCentroCosto, DateTime fechaExtraordinaria)
        {
            return
                Negocios.ClavesPresupuestalesNegocios.ObtenerClavesActivas(cvRamo, cvUnidad, cvCentroCosto, fechaExtraordinaria)
                .Select(c => new ClavePresupuestal { Clave = c.Clave, IdClavePresupuestal = c.Id, Disponible = c.ObtenerMontoPorMes(fechaExtraordinaria.Month) })
                .ToList();
        }

        public decimal ObtenerDisponibleClavePresupuestal(int idClavePresupuestal, DateTime fechaExtraordinaria)
        {
            return Negocios.ClavesPresupuestalesNegocios.ObtenerDisponibleClaveParaAnio(idClavePresupuestal, fechaExtraordinaria);
        }
    }
}