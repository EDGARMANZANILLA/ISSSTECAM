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
using System.Runtime;

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
            foreach (var item  in listaDeClaves)
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
            bool bandera;
            int anio = 2019;

            //datos que se obtienen como parametros
            decimal monto= 9984.03M;
            string clavePresupuestal = "21120283626211C016000J186038910780L415A4211";
            int mes = 1;
            string motivo = "";



            //1-se realiza la transaccion y si no hay problemas se continua
             //Transaccion nuevaTransaccion = new Transaccion();
             //bandera = nuevaTransaccion.Reduccion(monto, clavePresupuestal, mes);

            bandera = Negocios.ClavesPresupuestalesNegocios.Reducir(monto, clavePresupuestal, mes, anio);


            if (bandera)
            {
                //obtener el id de la clave remitente
                var clave = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal);


                Transacciones nueva = new Transacciones();
                nueva.Fecha = DateTime.Now;
                nueva.IdClavePresupuestalRemitente = clave.Id;
                nueva.IdMesRemitente = 1;
                nueva.IdClavePresupuestalDestinataria = null;
                nueva.IdMesDestinataria = null;
                nueva.Monto = monto;
                nueva.Motivo = "";
                nueva.IdTipoDeTransaccion = 1;
                nueva.Activo = true;


                bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);

            }



            return Json(bandera, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Transferencia(decimal origenMonto, string origenClave, int origenMes, /*decimal destinoMonto,*/ string destinoClave, string destinoMes, string motivoTransfer/*el mes debe venir en int*/)
        {
            bool bandera= false;
            int anio = 2019;
            //datos para reducir que se obtienen como parametros
            decimal monto1 = 1000000;
            string clavePresupuestal1 = "21120283626211C016000J186038910780L415A4211";
            int mes1 = 1;

            //datos hacia donde se va a transferir
            decimal monto2 = 9984.03M; //no hay segundo monto
            string clavePresupuestal2 = "21120283626311C016000J187039010790L415A4511";
            int mes2 = 12;

            string motivo = "";



            //La bandera sirve como indicador para saber si fue correcta y todo salio bien en el metodo para poder seguir al siguiente paso
            bandera = Negocios.ClavesPresupuestalesNegocios.Reducir(monto1, clavePresupuestal1, mes1, anio);

            //Se procede a la transferencia si todo salio bien en la reduccion
            if (bandera)
            {   
                bandera = Negocios.ClavesPresupuestalesNegocios.Transferir(monto1, clavePresupuestal2, mes2, anio); 
            }
            if (bandera)
            {
                //obtener el id de la clave remitente
                var claveRemitente = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal1);

                //obtener el id de la clave remitente
                var claveDestino = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal2);


                Transacciones nueva = new Transacciones();
                nueva.Fecha = DateTime.Now;
                nueva.IdClavePresupuestalRemitente = claveRemitente.Id;
                nueva.IdMesRemitente = 1;
                nueva.IdClavePresupuestalDestinataria = claveDestino.Id;
                nueva.IdMesDestinataria = 12;
                nueva.Monto = monto1;
                nueva.Motivo = "Compras";
                nueva.IdTipoDeTransaccion = 2;
                nueva.Activo = true;


                bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);

            }
            else 
            {
                 Negocios.ClavesPresupuestalesNegocios.Reducir(monto1, clavePresupuestal2, mes2, anio);
                 bandera = false;
            }

            return Json(bandera, JsonRequestBehavior.AllowGet);
        }

  
        #region ObtenerMontoDelaDB aquí se supone q debo recibir el mes y la clave para obtener el monto y mostrarlo.
        public JsonResult MostrarMonto(string clave, int mes)
        {
            string claveM = clave;
            int mesM = mes;

            int Respuesta = 12000;

            //bandera = Negocios.ClavesPresupuestalesNegocios.Reducir(monto, clavePresupuestal, mes, anio);
            //if (bandera)
            //{
            //    //obtener el id de la clave remitente
            //    var clave = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal);
            //    Transacciones nueva = new Transacciones();
            //    nueva.Fecha = DateTime.Now;
            //    nueva.IdClavePresupuestalRemitente = clave.Id;
            //    nueva.IdMesRemitente = 1;
            //    nueva.IdClavePresupuestalDestinataria = null;
            //    nueva.IdMesDestinataria = null;
            //    nueva.Monto = monto;
            //    nueva.Motivo = "";
            //    nueva.IdTipoDeTransaccion = 1;
            //    nueva.Activo = true;
            //    bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);
            //}
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }




}



