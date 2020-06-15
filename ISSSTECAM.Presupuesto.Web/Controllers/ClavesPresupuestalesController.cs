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
}



