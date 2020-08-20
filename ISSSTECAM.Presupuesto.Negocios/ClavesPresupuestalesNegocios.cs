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

        public static bool GuardarRegistroTransaccion(Transacciones nueva)
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




        public static bool Reducir(string clavePresupuestal, int mes, decimal monto, string motivo, int anio)
        {
            //se ocupa para hacer los metodos en cascada
            bool bandera = false;

            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);

            //Se obtiene la clave a la que se va a reducir
            ClavesPresupuestales claveAReducir = 
                repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clavePresupuestal && c.Activo == true).FirstOrDefault();

            //Verifica que la clave que vine de la DB sea la que nos da el usuario
            //Cambia el monto en el mes indicado de la clave 
            if (claveAReducir.Clave == clavePresupuestal)
            {
                switch (mes)
                {
                    case 1:
                        claveAReducir.PresupuestoEnero -= monto;
                        break;
                    case 2:
                        claveAReducir.PresupuestoFebrero -= monto;
                        break;
                    case 3:
                        claveAReducir.PresupuestoMarzo -= monto;
                        break;
                    case 4:
                        claveAReducir.PresupuestoAbril -= monto;
                        break;
                    case 5:
                        claveAReducir.PresupuestoMayo -= monto;
                        break;
                    case 6:
                        claveAReducir.PresupuestoJunio -= monto;
                        break;
                    case 7:
                        claveAReducir.PresupuestoJulio -= monto;
                        break;
                    case 8:
                        claveAReducir.PresupuestoAgosto -= monto;
                        break;
                    case 9:
                        claveAReducir.PresupuestoSeptiembre -= monto;
                        break;
                    case 10:
                        claveAReducir.PresupuestoOctubre -= monto;
                        break;
                    case 11:
                        claveAReducir.PresupuestoNoviembre -= monto;
                        break;
                    case 12:
                        claveAReducir.PresupuestoDiciembre -= monto;
                        break;
                }
            }


            //guarda un registro de la Reduccion
            Transacciones nuevaReduccion = new Transacciones();
            nuevaReduccion.FechaOperacion = DateTime.Now;
            nuevaReduccion.IdClavePresupuestalRemitente = claveAReducir.Id;
            nuevaReduccion.IdMesRemitente = mes;
            nuevaReduccion.IdClavePresupuestalDestinataria = null;
            nuevaReduccion.IdMesDestinataria = null;
            nuevaReduccion.Monto = monto;
            nuevaReduccion.Motivo = motivo;
            nuevaReduccion.IdTipoDeTransaccion = 1;

            //Solicitar servicio para traernos el id del empleado y nombre de empleado 
            nuevaReduccion.IdEmpleadoOperacion = 0000000;
            nuevaReduccion.NombreEmpleadoOperacion = "Nombre De Prueba";
            nuevaReduccion.IdentificadorOperacion = Guid.NewGuid();
            nuevaReduccion.Activo = true;

        

            //Se utiliza el mismo repositorio para hacer el update por medio de (Modificar)    
            try
            {   
               
                //Guarda la entidad modificada con los nuevosvalores
                repositorio.Modificar(claveAReducir);
                //Guarda el registro de la transaccion
                GuardarRegistroTransaccion(nuevaReduccion);
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



        public static bool Transferir(string clavePresupuestalAReducir, int mesAReducir, decimal montoAtransferir, string clavePresupuestalAtransferir, int mesAtransferir, string motivoDeTransferencia, int anio, Guid identificadorDeOperacion)
        {
            bool bandera = false;


            var transaccion = new Transaccion();
            var repositorio = new Repositorio<ClavesPresupuestales>(transaccion);


            //Se realiza la reduccion del montoATransferir a la clave y mes indicado

            //Se obtiene la clave a la que se va a reducir
            ClavesPresupuestales claveAReducir =
                repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clavePresupuestalAReducir && c.Activo == true).FirstOrDefault();

            //Verifica que la clave que vine de la DB sea la que nos da el usuario
            // Y cambia el monto en el mes indicado de la clave 

            if (claveAReducir.Clave == clavePresupuestalAReducir)
            {

                switch (mesAReducir)
                {
                    case 1:
                        claveAReducir.PresupuestoEnero -= montoAtransferir;
                        break;
                    case 2:
                        claveAReducir.PresupuestoFebrero -= montoAtransferir;
                        break;
                    case 3:
                        claveAReducir.PresupuestoMarzo -= montoAtransferir;
                        break;
                    case 4:
                        claveAReducir.PresupuestoAbril -= montoAtransferir;
                        break;
                    case 5:
                        claveAReducir.PresupuestoMayo -= montoAtransferir;
                        break;
                    case 6:
                        claveAReducir.PresupuestoJunio -= montoAtransferir;
                        break;
                    case 7:
                        claveAReducir.PresupuestoJulio -= montoAtransferir;
                        break;
                    case 8:
                        claveAReducir.PresupuestoAgosto -= montoAtransferir;
                        break;
                    case 9:
                        claveAReducir.PresupuestoSeptiembre -= montoAtransferir;
                        break;
                    case 10:
                        claveAReducir.PresupuestoOctubre -= montoAtransferir;
                        break;
                    case 11:
                        claveAReducir.PresupuestoNoviembre -= montoAtransferir;
                        break;
                    case 12:
                        claveAReducir.PresupuestoDiciembre -= montoAtransferir;
                        break;
                }

            }



            //Se realiza la Suma del MontoAtransferir a la clave y mes indicada 
            //Se obtiene la clave a la que se va a transferir
            ClavesPresupuestales claveAtransferir =
            repositorio.ObtenerPorFiltro(c => c.Anio == anio && c.Clave == clavePresupuestalAtransferir && c.Activo == true).FirstOrDefault();

            //Verifica que la clave que vine de la DB sea la que nos da el usuario
            //Cambia el monto en el mes indicado de la clave 
            if (claveAtransferir.Clave == clavePresupuestalAtransferir)
            {

                switch (mesAtransferir)
                {
                    case 1:
                        claveAtransferir.PresupuestoEnero += montoAtransferir;
                        break;
                    case 2:
                        claveAtransferir.PresupuestoFebrero += montoAtransferir;
                        break;
                    case 3:
                        claveAtransferir.PresupuestoMarzo += montoAtransferir;
                        break;
                    case 4:
                        claveAtransferir.PresupuestoAbril += montoAtransferir;
                        break;
                    case 5:
                        claveAtransferir.PresupuestoMayo += montoAtransferir;
                        break;
                    case 6:
                        claveAtransferir.PresupuestoJunio += montoAtransferir;
                        break;
                    case 7:
                        claveAtransferir.PresupuestoJulio += montoAtransferir;
                        break;
                    case 8:
                        claveAtransferir.PresupuestoAgosto += montoAtransferir;
                        break;
                    case 9:
                        claveAtransferir.PresupuestoSeptiembre += montoAtransferir;
                        break;
                    case 10:
                        claveAtransferir.PresupuestoOctubre += montoAtransferir;
                        break;
                    case 11:
                        claveAtransferir.PresupuestoNoviembre += montoAtransferir;
                        break;
                    case 12:
                        claveAtransferir.PresupuestoDiciembre += montoAtransferir;
                        break;
                }


            }




            //Guarda el registro de la transaccion
            Transacciones nuevaTrasferencia = new Transacciones();
            nuevaTrasferencia.FechaOperacion = DateTime.Now;
            nuevaTrasferencia.IdClavePresupuestalRemitente = claveAReducir.Id;
            nuevaTrasferencia.IdMesRemitente = mesAReducir;
            nuevaTrasferencia.IdClavePresupuestalDestinataria = claveAtransferir.Id;
            nuevaTrasferencia.IdMesDestinataria = mesAtransferir;
            nuevaTrasferencia.Monto = montoAtransferir;
            nuevaTrasferencia.Motivo = motivoDeTransferencia;
            nuevaTrasferencia.IdTipoDeTransaccion = 2;
            //Solicitar servicio para traernos el id del empleado y nombre de empleado 
            nuevaTrasferencia.IdEmpleadoOperacion = 0000000;
            nuevaTrasferencia.NombreEmpleadoOperacion = "Nombre De Prueba";
            nuevaTrasferencia.IdentificadorOperacion = identificadorDeOperacion;
            nuevaTrasferencia.Activo = true;


            //Guid g = Guid.Empty;

            //Guid g2 = new Guid();



            //Se utiliza el mismo repositorio para hacer el update por medio de (Modificar)    
                    try
                    {
                        //guarda el registro de la transferencia
                        GuardarRegistroTransaccion(nuevaTrasferencia);
                        //guarda las entidades que fueron modificadas
                        repositorio.Modificar(claveAReducir);
                        repositorio.Modificar(claveAtransferir);
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