using AutoMapper;
using ISSSTECAM.Presupuesto.Entidades;
using ISSSTECAM.Presupuesto.Entidades.DTO;
using ISSSTECAM.Presupuesto.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class ActividadesController : Controller
    {
        //
        // GET: /Actividades/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lista()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ObtenerActivosLista()
        {
            List<ActividadDTO> listado = Mapper.Map<IEnumerable<Actividades>, List<ActividadDTO>>(ActividadesNegocios.ObtenerActivosDelAnio(DateTime.Now.Year));
            return Json(new { results = listado.Select(a => new { id = a.Clave, text = a.DescripcionCompleta }) });
        }
    }
}
