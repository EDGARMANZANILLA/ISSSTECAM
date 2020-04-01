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
    public class CentrosCostosController : Controller
    {
        //
        // GET: /CentrosCostos/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]        
        public ActionResult ObtenerActivos()
        {
            List<CentroCostoDTO> listado = Mapper.Map<IEnumerable<CentrosCostos>,List<CentroCostoDTO>>(CentrosCostosNegocios.ObtenerActivos());
            return Json(new { data = listado });
        }
        
        public ActionResult Lista()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ObtenerActivosLista()
        {
            List<CentroCostoDTO> listado = Mapper.Map<IEnumerable<CentrosCostos>, List<CentroCostoDTO>>(CentrosCostosNegocios.ObtenerActivos());
            return Json(new { results = listado.Select(cc => new { id = cc.Id, text = cc.DescripcionCompleta}) });
        }
    }
}
