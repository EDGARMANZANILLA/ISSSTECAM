using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class TransaccionesController : Controller
    {
        //
        // GET: /Transaccion/

        public ActionResult Index()
        {


            return View();

        }

        public JsonResult DevolverClaves()
        {

              List<string> claves = new List<string>();
            var clavesDB = Negocios.ClavesPresupuestalesNegocios.ObtenerClavesActivas();
         return Json(dato, JsonRequestBehavior.AllowGet);
        }








    }
}