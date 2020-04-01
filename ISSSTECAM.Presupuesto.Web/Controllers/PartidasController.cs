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
    public class PartidasController : Controller
    {
        //
        // GET: /Partidas/

        public ActionResult Lista()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ObtenerActivosLista(string q)
        {
            List<PartidaDTO> partidas = AutoMapper.Mapper.Map<IEnumerable<Partidas>, List<PartidaDTO>>(PartidasNegocios.ObtenerActivas(q));
            return Json(new { results = partidas.Select(p => new { id = p.Clave, text = p.DescripcionCompleta }) });
        }

    }
}
