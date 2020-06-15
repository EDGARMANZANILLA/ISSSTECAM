using ISSSTECAM.Presupuesto.Web.Models;
using OfficeOpenXml;
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
}



