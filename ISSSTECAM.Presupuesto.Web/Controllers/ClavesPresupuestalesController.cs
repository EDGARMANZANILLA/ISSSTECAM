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
            return PartialView();
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
                nueva.IdMes = mes;
                nueva.IdTipoDeTransaccion = 1;
                nueva.Monto = monto;
                nueva.Motivo = "";
                nueva.Activo = true;


                bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);

            }



            return Json(bandera, JsonRequestBehavior.AllowGet);
        }




        public JsonResult Transferencia(decimal origenMonto, string origenClave, int origenMes, decimal destinoMonto, string destinoClave, int destinoMes, string motivoTransfer/*el mes debe venir en int*/)
        {
            bool bandera= false;

           // JsonResult RespuestaJson = Reducciones();
            




            return Json(bandera, JsonRequestBehavior.AllowGet);
        }




    }




}



