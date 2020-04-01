using System.Web;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Registrar los filtros EO.Pdf MVCToPDF 
            //EO.Pdf.Mvc.MVCToPDF.RegisterFilter(typeof(GlobalFilters));
        }
    }
}