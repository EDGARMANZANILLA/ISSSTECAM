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

        //[HttpPost]
        //#region obtiene fecha y luego guarda, (modificar a un solo metodo)
        //public JsonResult RecibirFechaParaGuardarNomina(int guardaQuincena, string guardaMes, int guardaAnio, int tipoNomina)
        //{
        //    string dato;
        //    try
        //    {
        //        Nomina nueva = new Nomina();

        //        nueva = (Nomina)Session["nueva"];
        //        nueva.FechaAplicacion = DateTime.Now;

        //        //La fecha (yyyy/mm/dd)
        //        string fecha = guardaAnio + "/" + guardaMes + "/" + guardaQuincena;
        //        nueva.FechaQuincena = Convert.ToDateTime(fecha);
        //        nueva.Observacion = "-";
        //        nueva.Activo = true;

        //        //int tipoNomina es 1 o 2 que se encuentran asi en la DB
        //        nueva.idTiposNominas = tipoNomina;

        //        Negocios.NominaNegocios.GuardarNomina(nueva);
        //        dato = "Guardado Existoso";
        //    }
        //    catch (Exception e)
        //    {
        //        dato = "NO se pudo guardar la nomina, verifique e intente de nuevo por favor";
        //    }
        //    return Json(dato, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
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



