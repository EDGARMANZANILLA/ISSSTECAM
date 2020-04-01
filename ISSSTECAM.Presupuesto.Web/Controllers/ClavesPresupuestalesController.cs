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
        
    }
}
