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


        //metodos para transacciones
        public static ClavesPresupuestales ObtenerPorUnicaClave(int anio, string clave)
        {
            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);

           
            return repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clave && c.Activo == true).FirstOrDefault();
        }

        public static bool GuardarTransaccion(Transacciones nueva)
        {
            bool bandera;
                var transaccion = new Transaccion();
                try
                {
                    var repositorio = new Repositorio<Transacciones>(transaccion);
                    repositorio.Agregar(nueva);
                    transaccion.GuardarCambios();
                    bandera = true;
                }
                catch (Exception ex)
                {
                    transaccion.Dispose();
                    bandera = false;
                }
            return bandera;
        }




        public static bool Reducir(string clavePresupuestal, int mes, decimal monto, int anio)
        {
            //se ocupa para hacer los metodos en cascada
            bool bandera = false;

            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);

            //Se obtiene la clave a la que se va a reducir
            ClavesPresupuestales claves = 
                repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clavePresupuestal && c.Activo == true).FirstOrDefault();

            //Verifica que la clave que vine de la DB sea la que nos da el usuario
            //Cambia el monto en el mes indicado de la clave 
            if (claves.Clave == clavePresupuestal)
            {

                switch (mes)
                {
                    case 1:
                        claves.PresupuestoEnero -= monto;
                        break;
                    case 2:
                        claves.PresupuestoFebrero -= monto;
                        break;
                    case 3:
                        claves.PresupuestoMarzo -= monto;
                        break;
                    case 4:
                        claves.PresupuestoAbril -= monto;
                        break;
                    case 5:
                        claves.PresupuestoMayo -= monto;
                        break;
                    case 6:
                        claves.PresupuestoJunio -= monto;
                        break;
                    case 7:
                        claves.PresupuestoJulio -= monto;
                        break;
                    case 8:
                        claves.PresupuestoAgosto -= monto;
                        break;
                    case 9:
                        claves.PresupuestoSeptiembre -= monto;
                        break;
                    case 10:
                        claves.PresupuestoOctubre -= monto;
                        break;
                    case 11:
                        claves.PresupuestoNoviembre -= monto;
                        break;
                    case 12:
                        claves.PresupuestoDiciembre -= monto;
                        break;
                }


            }


            //Se utiliza el mismo repositorio para hacer el update por medio de (Modificar)    
            try
            {
              
                repositorio.Modificar(claves);
                transaccion.GuardarCambios();
                bandera = true;
            }
            catch (Exception ex)
            {
                transaccion.Dispose();
                bandera = false;
            }

            return bandera;
        }

        public static  bool Transferir(string clavePresupuestal, int mes, decimal monto, int anio)
        {
            bool bandera= false;


            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);

            //Se obtiene la clave a la que se va a transferir
            ClavesPresupuestales claves =
                repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clavePresupuestal && c.Activo == true).FirstOrDefault();

            //Verifica que la clave que vine de la DB sea la que nos da el usuario
            //Cambia el monto en el mes indicado de la clave 
            if (claves.Clave == clavePresupuestal)
            {

                switch (mes)
                {
                    case 1:
                        claves.PresupuestoEnero     += monto;
                        break;
                    case 2:
                        claves.PresupuestoFebrero   += monto;
                        break;
                    case 3:
                        claves.PresupuestoMarzo     += monto;
                        break;
                    case 4:
                        claves.PresupuestoAbril     += monto;
                        break;
                    case 5:
                        claves.PresupuestoMayo      += monto;
                        break;
                    case 6:
                        claves.PresupuestoJunio     += monto;
                        break;
                    case 7:
                        claves.PresupuestoJulio     += monto;
                        break;
                    case 8:
                        claves.PresupuestoAgosto    += monto;
                        break;
                    case 9:
                        claves.PresupuestoSeptiembre += monto;
                        break;
                    case 10:
                        claves.PresupuestoOctubre   += monto;
                        break;
                    case 11:
                        claves.PresupuestoNoviembre  += monto;
                        break;
                    case 12:
                        claves.PresupuestoDiciembre  += monto;
                        break;
                }


            }

                    //Se utiliza el mismo repositorio para hacer el update por medio de (Modificar)    
                    try
                    {

                        repositorio.Modificar(claves);
                        transaccion.GuardarCambios();
                        bandera = true;
                    }
                    catch (Exception ex)
                    {
                        transaccion.Dispose();
                        bandera = false;
                    }

           



            return bandera;
        }

    }
}