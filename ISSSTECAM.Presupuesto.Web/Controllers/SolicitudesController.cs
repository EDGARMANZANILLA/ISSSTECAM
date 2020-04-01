using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class SolicitudesController : Controller
    {
        //
        // GET: /Solicitudes/

        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Operaciones()
        {
            return PartialView();
        }
    }
}
