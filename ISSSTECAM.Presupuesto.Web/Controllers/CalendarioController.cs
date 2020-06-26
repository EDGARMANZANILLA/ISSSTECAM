using ISSSTECAM.Presupuesto.Entidades;
using ISSSTECAM.Presupuesto.Entidades.DTO;
using ISSSTECAM.Presupuesto.Negocios;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class CalendarioController : Controller
    {
        //
        // GET: /Calendario/

        public ActionResult Index()
        {
            ViewBag.ExisteCalendario =
                Negocios.ClavesPresupuestalesNegocios.ExistenClavesParaAnio(DateTime.Now.Year);
            return PartialView();
        }

        [HttpPost]
        public JsonResult ImportarArchivo()
        {
            //List<CalendarioClavePresupuestal> claves = new List<CalendarioClavePresupuestal>();

            List<ClavesPresupuestales> claves = new List<ClavesPresupuestales>();
            List<ClavesPresupuestales> clavesAgrupadas = new List<ClavesPresupuestales>();



            var archivo = Request.Files[0];
            using (ExcelPackage package = new ExcelPackage(archivo.InputStream))
            {
                //validar que solo tenga una hoja
                if (package.Workbook.Worksheets.Count != 1)
                    return Json(new { exitoso = false, mensaje = "El archivo tiene más de una hoja" });

                var hoja = package.Workbook.Worksheets[1];

                int filas = hoja.Dimension.End.Row;


                #region Valida que la cada celda de la columna 1 tenga 43 caracteres (valida la clave presupuestal sea correcta en digitos) 
                Dictionary<int, int> verificarCeldasDiferentesA43 = new Dictionary<int, int>();

                if (hoja.Dimension != null) {
                    for (int i = 2; i <= filas; i++)
                    {
                        string celda = hoja.Cells[i, 1].Value.ToString();
                        //busca error

                        int caracteresEnCelda = celda.Length;

                        if (caracteresEnCelda != 43)
                        {
                            verificarCeldasDiferentesA43.Add(i, caracteresEnCelda);
                        }
                    }

                    if (verificarCeldasDiferentesA43.Count() != 0 ) 
                    {
                        return Json(new { exitoso = false, errores = string.Join(Environment.NewLine, verificarCeldasDiferentesA43.ToArray()), });
                    }
                    
                }
                #endregion
                #region verificar que ninguno de los elementos de la clave presupuestas este vacio
                //Valida que la cada elemento
                Dictionary<int, string> verificacion = new Dictionary<int, string>();

                for (int i = 2; i <= filas; i++)
                {
                   
                    string cadenaClave = hoja.Cells[i, 1].Value.ToString();

                    var programa = ProgramasNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(22, 3));
                    var projecto = ProyectosNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(25, 4));
                    var actividad = ActividadesNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(29, 4));
                    var partida = PartidasNegocios.ObtenerActivaPorClave(cadenaClave.Substring(39, 4));

                    if (programa == null)
                    {
                        verificacion.Add(Convert.ToInt16(cadenaClave.Substring(22, 3)), "Programa");

                    }else if (projecto == null)
                    {
                        verificacion.Add(Convert.ToInt16(cadenaClave.Substring(25, 4)), "Projecto");

                    }else if(actividad == null)
                    {
                        verificacion.Add(Convert.ToInt16(cadenaClave.Substring(29, 4)), "Actividad");

                    }else if (partida == null)
                    {
                        verificacion.Add(Convert.ToInt16(cadenaClave.Substring(39, 4)), "Partida");

                    }

                }
                if (verificacion.Count() != 0)
                {
                    verificacion.GroupBy(d => d.Value);
                    return Json(new { exitoso = false, elementoDeClave = string.Join(Environment.NewLine, verificacion.ToArray()), });
                }
                #endregion



                //verificar la validacion
                if (verificarCeldasDiferentesA43.Count() == 0)
                {

                    for (int i = 2; i <= filas; i++)
                    {
                        string cadenaClave = hoja.Cells[i, 1].Value.ToString();

                        var pro = ProgramasNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(22, 3));
                        var proj = ProyectosNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(25, 4));
                        var act = ActividadesNegocios.ObtenerActivosDelAnioPorClave(2019, cadenaClave.Substring(29, 4));
                        var par = PartidasNegocios.ObtenerActivaPorClave(cadenaClave.Substring(39, 4));
                        
                        var cc = CentrosCostosNegocios.ObtenerPorClave(ObtenerClaveCentroDeCostoPorClaveActividad(cadenaClave.Substring(29, 4)));





                        //Guarda las claves en una lista de claves
                            claves.Add(new ClavesPresupuestales
                            {
                                Clave = cadenaClave,
                                PresupuestoEnero = hoja.Cells[i, 2].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 2].Value.ToString()),
                                PresupuestoFebrero = hoja.Cells[i, 3].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 3].Value.ToString()),
                                PresupuestoMarzo = hoja.Cells[i, 4].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 4].Value.ToString()),
                                PresupuestoAbril = hoja.Cells[i, 5].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 5].Value.ToString()),
                                PresupuestoMayo = hoja.Cells[i, 6].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 6].Value.ToString()),
                                PresupuestoJunio = hoja.Cells[i, 7].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 7].Value.ToString()),
                                PresupuestoJulio = hoja.Cells[i, 8].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 8].Value.ToString()),
                                PresupuestoAgosto = hoja.Cells[i, 9].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 9].Value.ToString()),
                                PresupuestoSeptiembre = hoja.Cells[i, 10].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 10].Value.ToString()),
                                PresupuestoOctubre = hoja.Cells[i, 11].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 11].Value.ToString()),
                                PresupuestoNoviembre = hoja.Cells[i, 12].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 12].Value.ToString()),
                                PresupuestoDiciembre = hoja.Cells[i, 13].Value == null ? 0 : Convert.ToDecimal(hoja.Cells[i, 13].Value.ToString()),
                                Anio = 2019,
                                Activo = true,
                                IdPrograma = pro.Id,
                                Programas = pro,
                                IdProyecto = proj.Id,
                                Proyectos = proj,
                                IdActividad = act.Id,
                                Actividades = act,
                                IdPartida = par.Id,
                                Partidas = par,
                                IdCentroCosto = cc.Id,
                                CentrosCostos = cc
                            });


                      


                    }

                    //obtiene las claves antes guardadas y las reasigna agrupandolas por clave presupuestal y sumando los que sean iguales
                    foreach (ClavesPresupuestales clave in claves)
                    {
                        bool a = clavesAgrupadas.Contains(clave);
                        bool b = clavesAgrupadas.Exists(x => x.Clave == clave.Clave);
                   
                        if (b)
                        {
                            foreach (var unicaClaveAgrupada in clavesAgrupadas)
                            {   
                                if(unicaClaveAgrupada.Clave == clave.Clave)
                                {
                                    unicaClaveAgrupada.PresupuestoEnero += clave.PresupuestoEnero;
                                    unicaClaveAgrupada.PresupuestoFebrero += clave.PresupuestoFebrero;
                                    unicaClaveAgrupada.PresupuestoMarzo += clave.PresupuestoMarzo;
                                    unicaClaveAgrupada.PresupuestoAbril += clave.PresupuestoAbril;
                                    unicaClaveAgrupada.PresupuestoMayo += clave.PresupuestoMayo;
                                    unicaClaveAgrupada.PresupuestoJunio += clave.PresupuestoJunio;
                                    unicaClaveAgrupada.PresupuestoJulio += clave.PresupuestoJulio;
                                    unicaClaveAgrupada.PresupuestoAgosto += clave.PresupuestoAgosto;
                                    unicaClaveAgrupada.PresupuestoSeptiembre += clave.PresupuestoSeptiembre;
                                    unicaClaveAgrupada.PresupuestoOctubre += clave.PresupuestoOctubre;
                                    unicaClaveAgrupada.PresupuestoNoviembre += clave.PresupuestoNoviembre;
                                    unicaClaveAgrupada.PresupuestoDiciembre += clave.PresupuestoDiciembre;

                                }
                            
                            }
                          
                        }
                        else
                        {

                            clavesAgrupadas.Add(new ClavesPresupuestales
                            {
                                Clave = clave.Clave,
                                PresupuestoEnero = clave.PresupuestoEnero,
                                PresupuestoFebrero = clave.PresupuestoFebrero,
                                PresupuestoMarzo = clave.PresupuestoMarzo,
                                PresupuestoAbril = clave.PresupuestoAbril,
                                PresupuestoMayo = clave.PresupuestoMayo,
                                PresupuestoJunio = clave.PresupuestoJunio,
                                PresupuestoJulio = clave.PresupuestoJulio,
                                PresupuestoAgosto = clave.PresupuestoAgosto,
                                PresupuestoSeptiembre = clave.PresupuestoSeptiembre,
                                PresupuestoOctubre = clave.PresupuestoOctubre,
                                PresupuestoNoviembre = clave.PresupuestoNoviembre,
                                PresupuestoDiciembre = clave.PresupuestoDiciembre,
                                Anio = 2019,
                                Activo = true,
                                IdPrograma = clave.IdPrograma,
                                Programas = clave.Programas,
                                IdProyecto = clave.IdProyecto,
                                Proyectos = clave.Proyectos,
                                IdActividad = clave.IdActividad,
                                Actividades = clave.Actividades,
                                IdPartida = clave.IdPartida,
                                Partidas = clave.Partidas,
                                IdCentroCosto = clave.IdCentroCosto,
                                CentrosCostos = clave.CentrosCostos
                            });


                        }




                    }


             



                    var clavesDTO = AutoMapper.Mapper.Map<IEnumerable<ClavesPresupuestales>, IEnumerable<ClavePresupuestalDTO>>(clavesAgrupadas);
                    Session["ClavesPresupuestales"] = clavesDTO;

                    return Json (new
                    {
                        exitoso = true,
                        mensaje = "Se importó correctamente",
                        claves = clavesDTO.Select(c => new
                        {
                            clave = c.Clave + "| |" + c.PresupuestoEnero + "|" + c.PresupuestoFebrero + "|" + c.PresupuestoMarzo +
                            "|" + c.PresupuestoAbril + "|" + c.PresupuestoMayo + "|" + c.PresupuestoJunio + "|" + c.PresupuestoJulio + "|" + c.PresupuestoAgosto + "|" + c.PresupuestoSeptiembre +
                            "|" + c.PresupuestoOctubre + "|" + c.PresupuestoNoviembre + "|" + c.PresupuestoDiciembre
                        }),
                        clavesPresupuestales = clavesDTO
                    });


                }




                return Json(new { exitoso = false });
            }
        }

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

        private int ObtenerCentroDeCostoPorActividad(string claveActividad)
        {
            switch (claveActividad)
            {
                case "1078":
                    return CentrosCostosNegocios.ObtenerPorClave("11").Id;

                case "1079":
                    return CentrosCostosNegocios.ObtenerPorClave("01").Id;

                case "1080":
                    return CentrosCostosNegocios.ObtenerPorClave("02").Id;

                case "1081":
                    return CentrosCostosNegocios.ObtenerPorClave("02").Id;

                case "1082":
                    return CentrosCostosNegocios.ObtenerPorClave("02").Id;

                case "1083":
                    return CentrosCostosNegocios.ObtenerPorClave("02").Id;

                case "1084":
                    return CentrosCostosNegocios.ObtenerPorClave("03").Id;

                case "1085":
                    return CentrosCostosNegocios.ObtenerPorClave("04").Id;

                case "1086":
                    return CentrosCostosNegocios.ObtenerPorClave("04").Id;

                case "1087":
                    return CentrosCostosNegocios.ObtenerPorClave("05").Id;

                case "1088":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1089":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1090":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1091":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1092":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1093":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1094":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1095":
                    return CentrosCostosNegocios.ObtenerPorClave("06").Id;

                case "1096":
                    return CentrosCostosNegocios.ObtenerPorClave("08").Id;

                case "1097":
                    return CentrosCostosNegocios.ObtenerPorClave("08").Id;

                case "1098":
                    return CentrosCostosNegocios.ObtenerPorClave("08").Id;

                case "1099":
                    return CentrosCostosNegocios.ObtenerPorClave("07").Id;

                case "1100":
                    return CentrosCostosNegocios.ObtenerPorClave("09").Id;

                case "1101":
                    return CentrosCostosNegocios.ObtenerPorClave("10").Id;

                default:
                    return 0;
            }
        }

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

        public ActionResult ReporteConcentrado()
        {
            List<ClavesPresupuestales> claves = Session["ClavesPresupuestales"] as List<ClavesPresupuestales>;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Concentrado");
                int i = 1;

                worksheet.Cells[i, 1].Value = "CLAVE";
                worksheet.Cells[i, 2].Value = "DESCRIPCIÓN";
                worksheet.Cells[i, 3].Value = "ENERO";
                worksheet.Cells[i, 4].Value = "FEBRERO";
                worksheet.Cells[i, 5].Value = "MARZO";
                worksheet.Cells[i, 6].Value = "ABRIL";
                worksheet.Cells[i, 7].Value = "MAYO";
                worksheet.Cells[i, 8].Value = "JUNIO";
                worksheet.Cells[i, 9].Value = "JULIO";
                worksheet.Cells[i, 10].Value = "AGOSTO";
                worksheet.Cells[i, 11].Value = "SEPTIEMBRE";
                worksheet.Cells[i, 12].Value = "OCTUBRE";
                worksheet.Cells[i, 13].Value = "NOVIEMBRE";
                worksheet.Cells[i, 14].Value = "DICIEMBRE";
                //worksheet.Cells[i, 15].Value = "SUBTOTAL";
                ++i;

                foreach (var g in claves.GroupBy(c => c.Partidas.Clave).OrderBy(g => g.Key))
                {
                    worksheet.Cells[i, 1].Value = g.FirstOrDefault().Partidas.Clave;
                    worksheet.Cells[i, 2].Value = g.FirstOrDefault().Partidas.Descripcion;
                    worksheet.Cells[i, 3].Value = g.Sum(c => c.PresupuestoEnero);
                    worksheet.Cells[i, 4].Value = g.Sum(c => c.PresupuestoFebrero);
                    worksheet.Cells[i, 5].Value = g.Sum(c => c.PresupuestoMarzo);
                    worksheet.Cells[i, 6].Value = g.Sum(c => c.PresupuestoAbril);

                    worksheet.Cells[i, 7].Value = g.Sum(c => c.PresupuestoMayo);
                    worksheet.Cells[i, 8].Value = g.Sum(c => c.PresupuestoJunio);
                    worksheet.Cells[i, 9].Value = g.Sum(c => c.PresupuestoJulio);
                    worksheet.Cells[i, 10].Value = g.Sum(c => c.PresupuestoAgosto);

                    worksheet.Cells[i, 11].Value = g.Sum(c => c.PresupuestoSeptiembre);
                    worksheet.Cells[i, 12].Value = g.Sum(c => c.PresupuestoOctubre);
                    worksheet.Cells[i, 13].Value = g.Sum(c => c.PresupuestoNoviembre);
                    worksheet.Cells[i, 14].Value = g.Sum(c => c.PresupuestoDiciembre);
                    //worksheet.Cells[i, 15].Value = g.Sum(c => c.PresupuestoDiciembre);
                    ++i;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        public ActionResult ReporteConcentradoPrograma()
        {
            List<ClavePresupuestalDTO> claves = Session["ClavesPresupuestales"] as List<ClavePresupuestalDTO>;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Concentrado por programa");

                //programa
                foreach (var grupo in claves.GroupBy(c => c.Programa).OrderBy(g => g.Key))
                {
                    var g = grupo.GroupBy(c => new { PartidaGeneral = c.PartidaGeneral, c.CentroCosto }).OrderBy(a => a.Key.PartidaGeneral);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("CAPÍTULO", typeof(string));

                    var partidasExistentes = grupo.GroupBy(a => a.PartidaGeneral).OrderBy(a => a.Key);
                    foreach (var partida in partidasExistentes)
                    {
                        dt.Rows.Add(partida.Key);
                    }

                    var centrosCostos = grupo.GroupBy(a => a.CentroCosto);
                    int col = 1;
                    foreach (var centroCosto in centrosCostos)
                    {
                        dt.Columns.Add(centroCosto.Key, typeof(decimal));
                        int fil = 0;
                        foreach (var partida in partidasExistentes)
                        {
                            var encontrado = g.FirstOrDefault(b => b.Key.PartidaGeneral == partida.Key
                            && b.Key.CentroCosto == centroCosto.Key);
                            if (encontrado != null)
                            {
                                dt.Rows[fil][col] = encontrado.Sum(a => a.PresupuestoAnual);
                            }
                            else
                            {
                                dt.Rows[fil][col] = 0;
                            }
                            ++fil;
                        }
                        ++col;
                    }

                    decimal granTotal = 0;
                    //agregar totales, última columna
                    dt.Columns.Add("TOTAL", typeof(decimal));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][dt.Columns.Count - 1] =
                            g.Where(b => b.Key.PartidaGeneral == dt.Rows[i][0].ToString()).Sum(d => d.Sum(e => e.PresupuestoAnual));

                        granTotal += g.Where(b => b.Key.PartidaGeneral == dt.Rows[i][0].ToString()).Sum(d => d.Sum(e => e.PresupuestoAnual));
                    }
                    //agregar totales, última fila
                    dt.Rows.Add("TOTAL");
                    for (int i = 1; i < dt.Columns.Count - 1; i++)
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = g.Where(b => b.Key.CentroCosto == dt.Columns[i].ColumnName).Sum(d => d.Sum(e => e.PresupuestoAnual));
                    }

                    dt.Rows[dt.Rows.Count - 1][dt.Columns.Count - 1] = granTotal;

                    int filaInicial = worksheet.Dimension == null ? 1 : worksheet.Dimension.End.Row + 2;

                    worksheet.Cells[filaInicial, 1].Value = grupo.Key.ToString();
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Merge = true;

                    ++filaInicial;
                    var seccion = worksheet.Cells[filaInicial, 1].LoadFromDataTable(dt, true);
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    worksheet.Cells[filaInicial, 1, filaInicial, dt.Columns.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    seccion.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    seccion.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    seccion.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    seccion.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    worksheet.Cells[seccion.Start.Row + 1, 2, seccion.End.Row, seccion.End.Column].Style.Numberformat.Format = "$###,###,###,##0.00";
                    seccion.AutoFitColumns();
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        [HttpPost]
        public JsonResult GuardarClaves()
        {
            //obtener las claves de la sesion
            List<ClavePresupuestalDTO> claves = Session["ClavesPresupuestales"] as List<ClavePresupuestalDTO>;

            var clavesPresupuestales =
                AutoMapper.Mapper.Map<IEnumerable<ClavePresupuestalDTO>, IEnumerable<ClavesPresupuestales>>(claves);

            Negocios.ClavesPresupuestalesNegocios.Agregar(clavesPresupuestales.ToList());

            var total = claves.Sum(c => c.PresupuestoAnual);
            return Json(new { exitoso = true, total = total });
        }

        //public ActionResult ReporteCentroCostoMensual()
        //{
        //    Chart chart = new Chart();
        //    chart.Width = 700;
        //    chart.Height = 300;
        //    chart.BackColor = Color.FromArgb(211, 223, 240);
        //    chart.BorderlineDashStyle = ChartDashStyle.Solid;
        //    chart.BackSecondaryColor = Color.White;
        //    chart.BackGradientStyle = GradientStyle.TopBottom;
        //    chart.BorderlineWidth = 1;
        //    chart.Palette = ChartColorPalette.BrightPastel;
        //    chart.BorderlineColor = Color.FromArgb(26, 59, 105);
        //    chart.RenderType = RenderType.BinaryStreaming;
        //    chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        //    chart.AntiAliasing = AntiAliasingStyles.All;
        //    chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;

        //    List<ClavePresupuestalDTO> claves = Session["ClavesPresupuestales"] as List<ClavePresupuestalDTO>;

        //    foreach(var agrupado in claves.GroupBy(c => c.CentroCosto))
        //    {
        //        var s = new Series() { Name = agrupado.Key.ToString(), ChartType = SeriesChartType.Column, Points = agrupado.Select(g => new DataPoint() { YValues = decimal[] { g.  }  }) });

        //        chart.Series.Add(
        //    }

        //    return PartialView();
        //}

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




    }
}