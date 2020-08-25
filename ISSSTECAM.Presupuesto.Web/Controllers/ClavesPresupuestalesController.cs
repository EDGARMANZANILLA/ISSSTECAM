using ISSSTECAM.Presupuesto.Entidades;
using ISSSTECAM.Presupuesto.Web.Models;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISSSTECAM.Presupuesto.Entidades.DTO;
using System.Collections;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class ClavesPresupuestalesController : Controller
    {

        public ActionResult Captura()
        {
            return PartialView();
        }

        public ActionResult Transaccion()
        {
            //Esta lista proviene de la DB
            List<string> claves = new List<string>();


            //extrae la tabla ClavesPresupuestales filtrada por anio
            var listaDeClaves = Negocios.ClavesPresupuestalesNegocios.ObtenerClavesActivasPorAnio(2019);

            //filtramos solo las claves de lo extraido
            foreach (var item in listaDeClaves)
            {
                claves.Add(item.Clave);

            }
            /*
            claves.Add("21120283626211C016000J186038910780L415A4211");
            claves.Add("21120283626311C016000J187039010790L415A4511");
            claves.Add("21120283626211C016000J187039010790L415A4521");
            claves.Add("007");
            */

            return PartialView(claves);
        }

        public ActionResult Calendario()
        {
            List<string> clavesC = new List<string>();

            //clavesC.Add("14470459");
            //clavesC.Add("14470458");
            //clavesC.Add("14470457");
            //clavesC.Add("14470456");
            //clavesC.Add("14470455");
            //clavesC.Add("14470454");
            //clavesC.Add("14470453");
            //Esta lista proviene de la DB


            //extrae la tabla ClavesPresupuestales filtrada por anio
            var listaDeClaves = Negocios.ClavesPresupuestalesNegocios.ObtenerClavesActivasPorAnio(2019);

            //filtramos solo las claves de lo extraido
            foreach (var item in listaDeClaves)
            {
                clavesC.Add(item.Clave);

            }

            return PartialView(clavesC);
            //return PartialView();
        }


        //Metodo para la autoayuda
        [HttpPost]
        public List<string> AutoayudaDeClaves()
        {
            List<string> claves = new List<string>();

            var clavesEnDB = Negocios.ClavesPresupuestalesNegocios.ExistenClavesParaAnio(2019);

            return claves;
        }

        public JsonResult Reducciones(decimal reduccionMonto, string reduccionClave, int reduccionMes, string reduccionMotivo/*el mes debe venir en int */) {
            bool exitoDeTransferencias = false;
            int anio = 2019;

            // var a = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator; // Separador miles: ,
            //var b = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator; // Separador decimal: 


            exitoDeTransferencias = Negocios.ClavesPresupuestalesNegocios.Reducir( reduccionClave, reduccionMes, reduccionMonto, reduccionMotivo, anio);



            return Json(exitoDeTransferencias, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Transferencia(string origenClave, int origenMes, decimal origenMonto, string destinoClave, int destinoMes, string motivoTransfer)

        {
            //var a = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator; // Separador miles: ,
          //  var b = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator; // Separador decimal: 

            bool exitoDeTransferencias = false;         
            int anio = 2019;

            Guid identificadorDeOperacion = Guid.NewGuid();

            //La bandera sirve como indicador para saber si fue correcta y todo salio bien en el metodo para poder seguir al siguiente paso    
            exitoDeTransferencias = Negocios.ClavesPresupuestalesNegocios.Transferir(origenClave, origenMes, origenMonto, destinoClave, destinoMes, motivoTransfer,  anio, identificadorDeOperacion);


            return Json(exitoDeTransferencias, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult TransferenciaDeClavesAClave(List<DatosDeClaves> cuentasRemitentes, DatosDeClaves cuentaDestino, string motivo) {

            bool exitoDeTransferencias = false;
            int anio = 2019 ;
            Decimal sumaAtransferir = cuentasRemitentes.Sum(x => x.monto);

            Guid identificadorDeOperacion = Guid.NewGuid();

            if (ModelState.IsValid)
            {
              
                try
                {
                    foreach (DatosDeClaves remitente in cuentasRemitentes)
                    {
                        //Ejecutar varias transferencias
                        //La bandera sirve como indicador para saber si fue correcta y todo salio bien en el metodo para poder seguir al siguiente paso    
                        exitoDeTransferencias = Negocios.ClavesPresupuestalesNegocios.Transferir(remitente.clave, remitente.mes, remitente.monto, cuentaDestino.clave, cuentaDestino.mes, motivo, anio, identificadorDeOperacion);

                    }
                }
                catch (Exception ex) 
                {
                    return Json("Hubo un problema intentelo de nuevo", JsonRequestBehavior.AllowGet);
                }

            }



            return Json(exitoDeTransferencias, JsonRequestBehavior.AllowGet);
        }





        #region ObtenerMontoDelaDB aquí se supone q debo recibir el mes y la clave para obtener el monto y mostrarlo.
        public JsonResult MostrarMonto(string clave, int mes)
        {

            decimal montoDisponible= 0;

            var claveObtenida = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(2019, clave);


            switch (mes)
            {
                case 1:
                    montoDisponible = claveObtenida.PresupuestoEnero;
                    break;
                case 2:
                    montoDisponible = claveObtenida.PresupuestoFebrero;
                    break;
                case 3:
                    montoDisponible = claveObtenida.PresupuestoMarzo;
                    break;
                case 4:
                    montoDisponible = claveObtenida.PresupuestoAbril;
                    break;
                case 5:
                    montoDisponible = claveObtenida.PresupuestoMayo;
                    break;
                case 6:
                    montoDisponible = claveObtenida.PresupuestoJunio;
                    break;

                case 7:
                    montoDisponible = claveObtenida.PresupuestoJulio;
                    break;
                case 8:
                    montoDisponible = claveObtenida.PresupuestoAgosto;
                    break;
                case 9:
                    montoDisponible = claveObtenida.PresupuestoSeptiembre;
                    break;
                case 10:
                    montoDisponible = claveObtenida.PresupuestoOctubre;
                    break;
                case 11:
                    montoDisponible = claveObtenida.PresupuestoNoviembre;
                    break;
                case 12:
                    montoDisponible = claveObtenida.PresupuestoDiciembre;
                    break;

                default:
                    montoDisponible = 0.00m;
                    break;
            }





            return Json(montoDisponible, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }


    public class DatosDeClaves
    {
        [Required]
        public string clave { get; set; }
        [Required]
        public int mes { get; set; }
        [Required]
        public decimal monto { get; set; }
    }





}



