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
    public class ProyectosController : Controller
    {
        //
        // GET: /Proyectos/

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
            List<ProyectoDTO> listado = Mapper.Map<IEnumerable<Proyectos>, List<ProyectoDTO>>(ProyectosNegocios.ObtenerActivosDelAnio(DateTime.Now.Year));
            return Json(new { results = listado.Select(p => new { id = p.Clave, text = p.DescripcionCompleta }) });
        }
    }
}
