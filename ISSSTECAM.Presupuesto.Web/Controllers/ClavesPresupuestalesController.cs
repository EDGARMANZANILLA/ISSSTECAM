using ISSSTECAM.Presupuesto.Entidades;
using ISSSTECAM.Presupuesto.Web.Models;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISSSTECAM.Presupuesto.Entidades.DTO;

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

        public ActionResult Calendario()
        {
            ViewBag.ExisteCalendario =
              Negocios.ClavesPresupuestalesNegocios.ExistenClavesParaAnio(DateTime.Now.Year);
            return PartialView();
        }


        //Metodo para la autoayuda
        [HttpPost]
        public List<string> AutoayudaDeClaves()
        {
            List<string> claves = new List<string>();

            var clavesEnDB = Negocios.ClavesPresupuestalesNegocios.ExistenClavesParaAnio(2019);

            return claves;
        }

        public JsonResult Reducciones(decimal reduccionMonto, string reduccionClave, int reduccionMes, string reduccionMotivo/*el mes debe venir en int */) {
            bool bandera;
            int anio = 2019;

            //datos que se obtienen como parametros
            decimal monto= 9984.03M;
            string clavePresupuestal = "21120283626211C016000J186038910780L415A4211";
            int mes = 1;
            string motivo = "";



            //1-se realiza la transaccion y si no hay problemas se continua
             //Transaccion nuevaTransaccion = new Transaccion();
             //bandera = nuevaTransaccion.Reduccion(monto, clavePresupuestal, mes);

            bandera = Negocios.ClavesPresupuestalesNegocios.Reducir(monto, clavePresupuestal, mes, anio);


            if (bandera)
            {
                //obtener el id de la clave remitente
                var clave = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal);


                Transacciones nueva = new Transacciones();
                nueva.Fecha = DateTime.Now;
                nueva.IdClavePresupuestalRemitente = clave.Id;
                nueva.IdMesRemitente = 1;
                nueva.IdClavePresupuestalDestinataria = null;
                nueva.IdMesDestinataria = null;
                nueva.Monto = monto;
                nueva.Motivo = "";
                nueva.IdTipoDeTransaccion = 1;
                nueva.Activo = true;


                bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);

            }



            return Json(bandera, JsonRequestBehavior.AllowGet);
        }



        public JsonResult Transferencia(decimal origenMonto, string origenClave, int origenMes, /*decimal destinoMonto,*/ string destinoClave, string destinoMes, string motivoTransfer/*el mes debe venir en int*/)
        {
            bool bandera= false;
            int anio = 2019;
            //datos para reducir que se obtienen como parametros
            decimal monto1 = 1000000;
            string clavePresupuestal1 = "21120283626211C016000J186038910780L415A4211";
            int mes1 = 1;

            //datos hacia donde se va a transferir
            decimal monto2 = 9984.03M; //no hay segundo monto
            string clavePresupuestal2 = "21120283626311C016000J187039010790L415A4511";
            int mes2 = 12;

            string motivo = "";



            //La bandera sirve como indicador para saber si fue correcta y todo salio bien en el metodo para poder seguir al siguiente paso
            bandera = Negocios.ClavesPresupuestalesNegocios.Reducir(monto1, clavePresupuestal1, mes1, anio);

            //Se procede a la transferencia si todo salio bien en la reduccion
            if (bandera)
            {   
                bandera = Negocios.ClavesPresupuestalesNegocios.Transferir(monto1, clavePresupuestal2, mes2, anio); 
            }
            if (bandera)
            {
                //obtener el id de la clave remitente
                var claveRemitente = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal1);

                //obtener el id de la clave remitente
                var claveDestino = Negocios.ClavesPresupuestalesNegocios.ObtenerPorUnicaClave(anio, clavePresupuestal2);


                Transacciones nueva = new Transacciones();
                nueva.Fecha = DateTime.Now;
                nueva.IdClavePresupuestalRemitente = claveRemitente.Id;
                nueva.IdMesRemitente = 1;
                nueva.IdClavePresupuestalDestinataria = claveDestino.Id;
                nueva.IdMesDestinataria = 12;
                nueva.Monto = monto1;
                nueva.Motivo = "Compras";
                nueva.IdTipoDeTransaccion = 2;
                nueva.Activo = true;


                bandera = Negocios.ClavesPresupuestalesNegocios.GuardarTransaccion(nueva);

            }
            else 
            {
                 Negocios.ClavesPresupuestalesNegocios.Reducir(monto1, clavePresupuestal2, mes2, anio);
                 bandera = false;
            }

            return Json(bandera, JsonRequestBehavior.AllowGet);
        }




        #region Obtener Claves
        [HttpPost]
        public JsonResult ObtenerClaves()
        {
            List<ClavesPresupuestales> claves =
                Negocios.ClavesPresupuestalesNegocios.ObtenerClavesActivasPorAnio(DateTime.Now.Year).ToList();

            var clavesDTO = AutoMapper.Mapper.Map<IEnumerable<ClavesPresupuestales>, IEnumerable<ClavePresupuestalDTO>>(claves);
            Session["ClavesPresupuestales"] = clavesDTO;

            //var clavesDTO = Session["ClavesPresupuestales"] as IEnumerable<ClavePresupuestalDTO>;

            return Json(new
            {
                exitoso = true,
                mensaje = "Se cargó correctamente",
                claves = clavesDTO.Select(c => new
                {
                    clave = c.Clave + "| |" + c.PresupuestoEnero + "|" + c.PresupuestoFebrero + "|" + c.PresupuestoMarzo +
                    "|" + c.PresupuestoAbril + "|" + c.PresupuestoMayo + "|" + c.PresupuestoJunio + "|" + c.PresupuestoJulio + "|" + c.PresupuestoAgosto + "|" + c.PresupuestoSeptiembre +
                    "|" + c.PresupuestoOctubre + "|" + c.PresupuestoNoviembre + "|" + c.PresupuestoDiciembre
                }),
                clavesPresupuestales = clavesDTO
            });
        }
        #endregion

        #region monto mensual y monto mensual por capitulo
        [HttpPost]
        public JsonResult MontoMensual()
        {
            List<ClavePresupuestalDTO> claves = Session["ClavesPresupuestales"] as List<ClavePresupuestalDTO>;

            var agrupado = claves.GroupBy(c => c.CentroCosto);

            return Json(agrupado.Select(g => new
            {
                name = g.Key.ToString(),
                data = new decimal[] {
                    g.Sum(c => c.PresupuestoEnero),
                    g.Sum(c => c.PresupuestoFebrero),
                    g.Sum(c => c.PresupuestoMarzo),
                    g.Sum(c => c.PresupuestoAbril),
                    g.Sum(c => c.PresupuestoMayo),
                    g.Sum(c => c.PresupuestoJunio),
                    g.Sum(c => c.PresupuestoJulio),
                    g.Sum(c => c.PresupuestoAgosto),
                    g.Sum(c => c.PresupuestoSeptiembre),
                    g.Sum(c => c.PresupuestoOctubre),
                    g.Sum(c => c.PresupuestoNoviembre),
                    g.Sum(c => c.PresupuestoDiciembre),
                }
            }));
        }

        [HttpPost]
        public JsonResult MontoMensualCapitulo()
        {
            List<ClavePresupuestalDTO> claves = Session["ClavesPresupuestales"] as List<ClavePresupuestalDTO>;

            var agrupado = claves.GroupBy(c => c.PartidaGeneral).OrderBy(g => g.Key.ToString());

            return Json(new
            {
                capitulos = agrupado.Select(g => g.Key),
                datos = claves.GroupBy(g => g.CentroCosto).Select(g => new
                {
                    name = g.Key.ToString(),
                    data = agrupado.Select(cl => cl.Where(c => c.CentroCosto == g.Key.ToString()).Sum(a => a.PresupuestoAnual))
                })
            });
        }
        #endregion

        #region Obtener Centro de costos por actividad
        //private int ObtenerCentroDeCostoPorActividad(string claveActividad)
        //{
        //    switch (claveActividad)
        //    {
        //        case "1078":
        //            return CentrosCostosNegocios.ObtenerPorClave("11").Id;

        //        case "1079":
        //            return CentrosCostosNegocios.ObtenerPorClave("01").Id;

        //        case "1080":
        //            return CentrosCostosNegocios.ObtenerPorClave("02").Id;

        //        case "1081":
        //            return CentrosCostosNegocios.ObtenerPorClave("02").Id;

        //        case "1082":
        //            return CentrosCostosNegocios.ObtenerPorClave("02").Id;

        //        case "1083":
        //            return CentrosCostosNegocios.ObtenerPorClave("02").Id;

        //        case "1084":
        //            return CentrosCostosNegocios.ObtenerPorClave("03").Id;

        //        case "1085":
        //            return CentrosCostosNegocios.ObtenerPorClave("04").Id;

        //        case "1086":
        //            return CentrosCostosNegocios.ObtenerPorClave("04").Id;

        //        case "1087":
        //            return CentrosCostosNegocios.ObtenerPorClave("05").Id;

        //        case "1088":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1089":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1090":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1091":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1092":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1093":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1094":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1095":
        //            return CentrosCostosNegocios.ObtenerPorClave("06").Id;

        //        case "1096":
        //            return CentrosCostosNegocios.ObtenerPorClave("08").Id;

        //        case "1097":
        //            return CentrosCostosNegocios.ObtenerPorClave("08").Id;

        //        case "1098":
        //            return CentrosCostosNegocios.ObtenerPorClave("08").Id;

        //        case "1099":
        //            return CentrosCostosNegocios.ObtenerPorClave("07").Id;

        //        case "1100":
        //            return CentrosCostosNegocios.ObtenerPorClave("09").Id;

        //        case "1101":
        //            return CentrosCostosNegocios.ObtenerPorClave("10").Id;

        //        default:
        //            return 0;
        //    }
        //}

        //Clave centro de costo por clave actividad
        private string ObtenerClaveCentroDeCostoPorClaveActividad(string claveActividad)
        {
            switch (claveActividad)
            {
                case "1078":
                    return "011";

                case "1079":
                    return "001";

                case "1080":
                    return "002";

                case "1081":
                    return "002";

                case "1082":
                    return "002";

                case "1083":
                    return "002";

                case "1084":
                    return "003";

                case "1085":
                    return "004";

                case "1086":
                    return "004";

                case "1087":
                    return "005";

                case "1088":
                    return "006";

                case "1089":
                    return "006";

                case "1090":
                    return "006";

                case "1091":
                    return "006";

                case "1092":
                    return "006";

                case "1093":
                    return "006";

                case "1094":
                    return "006";

                case "1095":
                    return "006";

                case "1096":
                    return "008";

                case "1097":
                    return "008";

                case "1098":
                    return "008";

                case "1099":
                    return "007";

                case "1100":
                    return "009";

                case "1101":
                    return "010";

                default:
                    return "0";
            }
        }
        #endregion

    }




}



