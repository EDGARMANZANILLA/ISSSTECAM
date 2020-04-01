using ISSSTECAM.RH.Entidades;
using System;

namespace ISSSTECAM.RH.ServiciosWeb
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServicioEmpleados : IServicioEmpleados
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public vstEmpleadoDTO ObtenerPorNumeroEmpleado(string numeroEmpleado)
        {
            var emp = Negocios.EmpleadosNegocios.ObtenerPorNumeroEmpleado(numeroEmpleado);
            if(emp == null)
                return new vstEmpleadoDTO { IdEmpleado = 0, NombreCosto = "", IdTipoEmpleado = 0, IdCosto = 0 , Oficina =""};
            return new vstEmpleadoDTO { IdEmpleado = emp.IdEmpleado, NombreCosto = emp.NombreCosto, IdTipoEmpleado = emp.IdTipoEmpleado, IdCosto = emp.IdCosto, Oficina= emp.Oficina };
        }
    }
}