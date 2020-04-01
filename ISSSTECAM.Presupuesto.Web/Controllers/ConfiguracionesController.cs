using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class ConfiguracionesController : Controller
    {
        //
        // GET: /Configuraciones/

        public ActionResult Index()
        {
            return PartialView();
        }

    }
}
