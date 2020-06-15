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

        public JsonResult Reducciones(/* */) {
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




        public JsonResult Transferencia(/*Racibir parametros*/)
        {
            bool bandera;

            JsonResult RespuestaJson = Reducciones();
            




            return Json(bandera, JsonRequestBehavior.AllowGet);
        }












        //Aquí es donde planeaba asignar la recepción de los datos de la vista de Transacción.

        [HttpPost]
        public JsonResult ObtenerTransferenciaData(int origenMonto, string origenClave, string origenMes, int destinoMonto, string destinoClave, string destinoMes, string motivoTransfer)
        {
            string origen = origenMonto + "/" + origenClave + "/" + origenMes;
            string destino = destinoMonto + "/" + destinoClave + "/" + destinoMes;
            string motivo = motivoTransfer;
            String dato;

            if (origen != null && destino != null && motivo != null)
            {
                //Session["fechaInicial"] = fechaInicial;
                //Session["fechaFinal"] = fechaFinal;
                dato = "Transferencia exitosa";
            }
            else
            {
                dato = "Transferencia fallida, por favor vuelva a intentarlo";
            }

                return Json(dato, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerReduccionData(int reduccionMonto, string reduccionClave, string reduccionMes, string reduccionMotivo)
        {
            string origen = reduccionMonto + "/" + reduccionClave + "/" + reduccionMes;
            string motivo = reduccionMotivo;
            String dato;

            if (origen != null && motivo != null)
            {
                //Session["fechaInicial"] = fechaInicial;
                //Session["fechaFinal"] = fechaFinal;
                dato = "Reducción exitosa";
            }
            else
            {
                dato = "Reducción fallida, por favor vuelva a intentarlo";
            }

            return Json(dato, JsonRequestBehavior.AllowGet);
        }
    }




    public class Transaccion{

        public bool Reduccion(decimal monto, string clavePresupuestal, int mes) {
            bool bandera = false;

            int anio = 2019;
          
            //Actualiza el registro de la reduccion en la tabla ClavesPresupuestales
             bandera = Negocios.ClavesPresupuestalesNegocios.Reducir( monto, clavePresupuestal,  mes, anio );



            //Si todo salio bien regresa true
            return bandera;
        }





        public bool Transferencia(decimal monto, int clavePresupuestal, string mes, string Motivo)
        {
            bool bandera = false;







            return bandera;
        }




    }

}



