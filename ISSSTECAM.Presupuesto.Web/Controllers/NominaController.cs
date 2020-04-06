using CrystalDecisions.CrystalReports.Engine;
using ISSSTECAM.Presupuesto.Entidades;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ISSSTECAM.Presupuesto.Web.Controllers
{
    public class NominaController : Controller
    {
        //
        // GET: /Nomina/

        
        public ActionResult Conceptos()
        {
            return PartialView();
        }

        public ActionResult Cargar()
        {
            return PartialView();
        }


        public ActionResult Reporte() {
            return PartialView();
        }
        

        [HttpPost]
        public JsonResult ImportarArchivo(/*string fechaNomina*/)
        {
            Nomina nueva = new Nomina();

            nueva.Activo = true;
            nueva.FechaAplicacion = DateTime.Now;
            //La fecha (dd/mm/yyyy)

            string yyyy = "2023/";
            string mm = "02/";
            string dd="2";
            string año = yyyy+ mm + dd;
            nueva.FechaQuincena = Convert.ToDateTime(año);
                nueva.Observacion = "Hola D_B ";


        //Convert.ToDateTime(fechaNomina);
            nueva.DetallesNomina = new List<DetallesNomina>();





            List<Concepto> Conceptos = new List<Concepto>();
            //List<CalendarioClavePresupuestal> claves = new List<CalendarioClavePresupuestal>();

            //List<ClavesPresupuestales> claves = new List<ClavesPresupuestales>();

            var archivo = Request.Files[0];
            using (ExcelPackage package = new ExcelPackage(archivo.InputStream))
            {
                //validar que solo tenga una hoja
                if (package.Workbook.Worksheets.Count != 1)
                    return Json(new { exitoso = false, mensaje = "El archivo tiene más de una hoja" });

                var hoja = package.Workbook.Worksheets[1];

                int filas = hoja.Dimension.End.Row;
                int columnas = hoja.Dimension.End.Column;

                //encontrar conceptos
                var ConceptosRegistrados = Negocios.ConceptosNegocios.ObtenerActivos();

                for (int i = 1; i <= hoja.Dimension.End.Column; i++)
                {
                    Debug.Write(hoja.Cells[8, i].Value.ToString());
                    string cabecera = hoja.Cells[8, i].Value.ToString();
                    if (!string.IsNullOrEmpty(cabecera))
                    {
                        var ConceptosEncontrados =
                            ConceptosRegistrados.Where(c => c.ConceptoNomina.Trim().ToUpper() == cabecera.Trim().ToUpper());
                        //if(ConceptosEncontrados.Count() == 1)
                        //{
                        var ConceptoEnLista =
                            Conceptos.FirstOrDefault(c => c.NombreContable == cabecera.Trim().ToUpper() && c.Columna == i);
                        if (ConceptoEnLista == null)
                        {
                            foreach (var concepto in ConceptosEncontrados)
                            {

                                Conceptos.Add(
                                new Concepto
                                {
                                    Id = concepto.Id,
                                    Columna = i,
                                    NombreContable = cabecera.Trim().ToUpper(),
                                    NombrePresupuestal = concepto.ConceptoPresupuesto,
                                    TipoEmpleado = concepto.IdTipoEmpleadoAplica
                                });
                            }
                        }
                        //}
                        //else if (ConceptosEncontrados.Count() > 1)
                        //{
                        //    var ConceptoEnLista =
                        //        Conceptos.FirstOrDefault(c => c.NombreContable == cabecera.Trim().ToUpper() 
                        //            && c.Columna == i 
                        //            && c.Id == );
                        //}
                    }
                }

                var servicioRH = new RH.ServiciosWeb.ServicioEmpleadosClient();
                List<string> empleadosInexistentes = new List<string>();
                //encontrar detalles
                for (int fila = 9; fila <= hoja.Dimension.End.Row; fila++)
                {
                    if (hoja.Cells[fila, 1].Value == null)
                        break;
                    var numeroEmpleado = hoja.Cells[fila, 1].Value.ToString();
                    var empleado = servicioRH.ObtenerPorNumeroEmpleado(numeroEmpleado);

                    //Validacion
                    //  if (numeroEmpleado == "00284")
                    //      continue;

                    if (empleado.IdEmpleado == 0)
                    {
                        empleadosInexistentes.Add(numeroEmpleado);
                        continue;
                    }


                    foreach (var concepto in Conceptos)
                    {
                        if (concepto.Detalles == null)
                            concepto.Detalles = new List<DetalleConcepto>();

                        if (concepto.TipoEmpleado.HasValue)
                        {
                            //Esto solo aplica a los empleados de Confianza
                            if (concepto.TipoEmpleado == empleado.IdTipoEmpleado)
                            {

                                //verificar que hace CentroDeCosto
                                var detalle = concepto.Detalles.FirstOrDefault(d => d.CentroCosto == empleado.NombreCosto);
                                if (detalle == null)
                                {
                                    concepto.Detalles.Add(
                                    new DetalleConcepto
                                    {
                                        CentroCosto = empleado.NombreCosto,
                                        Total = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value)
                                    });
                                }
                                else
                                {
                                    detalle.Total += Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value);
                                }
                                DetallesNomina NuevaDetalleNomina = new DetallesNomina();
                                
                                NuevaDetalleNomina.IdEmpleado = empleado.IdEmpleado;
                                NuevaDetalleNomina.IdTipoEmpleado = empleado.IdTipoEmpleado;
                                NuevaDetalleNomina.IdConcepto = concepto.Id;
                                NuevaDetalleNomina.Monto = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value);
                                NuevaDetalleNomina.CentroCosto = empleado.NombreCosto;
                                NuevaDetalleNomina.Oficina = empleado.Oficina;




                                if (NuevaDetalleNomina.Monto > 0)
                                {
                                    nueva.DetallesNomina.Add(NuevaDetalleNomina);


                                }

                                /* nueva.DetallesNomina.Add(
                                      new DetallesNomina
                                      {
                                          IdConcepto = concepto.Id,   //3
                                          IdEmpleado = empleado.IdEmpleado,       //1
                                          IdTipoEmpleado = empleado.IdTipoEmpleado,   //2
                                          Monto = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value),    //4
                                          CentroCosto = empleado.NombreCosto,     //5
                                          Oficina = "listo 1"
                                       });
                                 */
                            }
                        }
                        //Esto aplica a los demas empleados
                        else
                        {
                            var detalle = concepto.Detalles.FirstOrDefault(d => d.CentroCosto == empleado.NombreCosto);
                            if (detalle == null)
                            {
                                concepto.Detalles.Add(
                                new DetalleConcepto
                                {
                                    CentroCosto = empleado.NombreCosto,
                                    Total = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value)
                                });
                            }
                            else
                            {
                                detalle.Total += Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value);
                            }

                            DetallesNomina NuevaDetalleNomina2 = new DetallesNomina();

                            NuevaDetalleNomina2.IdEmpleado = empleado.IdEmpleado;
                            NuevaDetalleNomina2.IdTipoEmpleado = empleado.IdTipoEmpleado;
                            NuevaDetalleNomina2.IdConcepto = concepto.Id;
                            NuevaDetalleNomina2.Monto = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value);
                            NuevaDetalleNomina2.CentroCosto = empleado.NombreCosto;
                            NuevaDetalleNomina2.Oficina = empleado.Oficina;
                            //   Double a = 4815.3;
                            //   Decimal b = Convert.ToDecimal(a);


                            //   if(NuevaDetalleNomina2.Monto == b )
                            if (NuevaDetalleNomina2.Monto > 0) {
                                nueva.DetallesNomina.Add(NuevaDetalleNomina2);

                            }
                            /*   nueva.DetallesNomina.Add(

                                       new DetallesNomina 
                                       {
                                           IdConcepto = concepto.Id,
                                           IdEmpleado = empleado.IdEmpleado,
                                           IdTipoEmpleado = empleado.IdTipoEmpleado,
                                           Monto = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value),
                                           CentroCosto = empleado.NombreCosto,
                                           Oficina = "listo"

                                       });
                               */
                        }

                        //var detalle = 
                        //    concepto.Detalles.FirstOrDefault(d => d.CentroCosto == empleado.NombreCosto && d.TipoEmpleado == empleado.IdTipoEmpleado);
                        //if (detalle == null)
                        //{
                        //    concepto.Detalles.Add(
                        //        new DetalleConcepto
                        //        {
                        //            CentroCosto = empleado.NombreCosto,
                        //            TipoEmpleado = empleado.IdTipoEmpleado,
                        //            Total = Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value)
                        //        });
                        //}
                        //else
                        //{
                        //    detalle.Total += Convert.ToDecimal(hoja.Cells[fila, concepto.Columna].Value);
                        //}

                    }
                }


                Session["nueva"] = nueva;

                //Negocios.NominaNegocios.GuardarNomina(nueva);
                string mensajeMontos = "";
                Conceptos.ForEach(c => mensajeMontos += "\n" + c.NombreContable + ": " + c.Detalles.Sum(d => d.Total).ToString("c"));

                //decimal t = 0;
                //Conceptos.ForEach(c => t += c.Detalles.Sum(d => d.Total));

                List<CentroDeCosto> centrosCostos = new List<CentroDeCosto>();

                foreach (var concepto in Conceptos)
                {
                    foreach (var detalle in concepto.Detalles)
                    {
                        var centroEncontrado = centrosCostos.FirstOrDefault(cc => cc.Nombre == detalle.CentroCosto);
                        if (centroEncontrado == null)
                        {
                            centrosCostos.Add(new CentroDeCosto { Nombre = detalle.CentroCosto, Total = 0, Detalles = new List<DetalleCentroDeCosto>() });
                            centroEncontrado = centrosCostos.LastOrDefault();
                        }
                        centroEncontrado.Total += detalle.Total;

                    }
                }

                /////////////
                //Dictionary<string, decimal> Temporal = new Dictionary<string, decimal>();
             
                //    var Almacen = Conceptos
                //                .Where(c => c.Detalles.Sum(d => d.Total) > 0)
                //                .GroupBy(c => c.NombreContable)
                //                .Select(c => new { Concepto = c.Key.ToString(), Total = c.Sum(x => x.Detalles.Sum(d => d.Total)) });

                //    foreach (var Tempp in Almacen)
                //    {
                //         Temporal.Add(Convert.ToString(Tempp.Concepto) , Convert.ToDecimal(Tempp.Total));       
                //    }

                ////List<string> a = new List<string>();

                //POP(Temporal);



                //enumerate the inner list
                /* foreach (var item in empleadosInexistentes)
                 {
                     //output the actual item
                     Console.WriteLine(item);
                 }
                 */
                // String C = empleadosInexistentes.Select(x => x.ToString).ToList(); 

            //   string items = string.Join(",", empleadosInexistentes.ToString());
               // string q = string.Join(Environment.NewLine, empleadosInexistentes.ToArray());
              //  a.Add(items);

              //  POP2 p = new POP2();
                

                return Json(new
                {
                    exitoso = true,
                    //total = t,
                    errores = string.Join(Environment.NewLine, empleadosInexistentes.ToArray()),
                    mensaje = mensajeMontos, //"Se importó correctamente"
                    datos = centrosCostos, //Conceptos
                    conceptos = Conceptos
                    .Where(c => c.Detalles.Sum(d => d.Total) > 0)
                    .GroupBy(c => c.NombreContable)
                    .Select(c => new { Concepto = c.Key.ToString(), Total = c.Sum(x => x.Detalles.Sum(d => d.Total)) })
                });


            }
        }


        public JsonResult TotalesConceptos(/*DateTime fechaInicio, DateTime fechaFin*/)
        {
            DateTime fechaInicio = Convert.ToDateTime("01/01/2020");
            DateTime fechaFin = Convert.ToDateTime("31/12/2020");

            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);
            
            Dictionary<string, decimal> conceptos = new Dictionary<string, decimal>();

            
            foreach (var nomina in nominas)
            {
                var conceptosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion).GroupBy(d => d.Conceptos.ConceptoNomina);
                foreach(var conceptoEncontrado in conceptosEncontrados)
                {
                    if (conceptos.ContainsKey(conceptoEncontrado.Key.ToString()))
                    {
                        conceptos[conceptoEncontrado.Key.ToString()] += conceptoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        conceptos.Add(conceptoEncontrado.Key.ToString(), conceptoEncontrado.Sum(d => d.Monto));
                    }
                }                                
            }

            
         /*   conceptos.Add("SUELDO", 1231253.92M);
            conceptos.Add("SUELDO EVENTUAL", 202441.97M);
            conceptos.Add("DIA ECONOMICO", 2442.86M);
            conceptos.Add("SUBS. X INCAP.", 2433.99M);
            conceptos.Add("P. S. M.", 101513.34M);
            conceptos.Add("SUBS. X INCAP. (EVENTUAL)", 8841.51M);
            conceptos.Add("QUINQUENIO", 4057.45M);
            conceptos.Add("Resultadp", 1010.10M);             
        */
            return Json(conceptos.Select(c => new { Concepto = c.Key.Trim(), Total = c.Value}), JsonRequestBehavior.AllowGet);
            }

        public JsonResult TotalesCentrosCostos(/*DateTime fechaInicio, DateTime fechaFin*/)
        {
            
            
            
            DateTime fechaInicio = Convert.ToDateTime("01/01/2020");
            DateTime fechaFin = Convert.ToDateTime("31/12/2020");

            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);

            Dictionary<string, decimal> centrosCostos = new Dictionary<string, decimal>();


            foreach (var nomina in nominas)
            {
                var centrosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion).GroupBy(d => d.CentroCosto);
                foreach (var centroCostoEncontrado in centrosEncontrados)
                {
                    if (centrosCostos.ContainsKey(centroCostoEncontrado.Key.ToString()))
                    {
                        centrosCostos[centroCostoEncontrado.Key.ToString()] += centroCostoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        centrosCostos.Add(centroCostoEncontrado.Key.ToString(), centroCostoEncontrado.Sum(d => d.Monto));
                    }
                }
            }

            /*
            conceptos.Add("SUELDO", 1231253.92M);
            conceptos.Add("SUELDO EVENTUAL", 202441.97M);
            conceptos.Add("DIA ECONOMICO", 2442.86M);
            conceptos.Add("SUBS. X INCAP.", 2433.99M);
            conceptos.Add("P. S. M.", 101513.34M);
            conceptos.Add("SUBS. X INCAP. (EVENTUAL)", 8841.51M);
            conceptos.Add("QUINQUENIO", 4057.45M);
             * */

            return Json(centrosCostos.Select(c => new { Concepto = c.Key.Trim(), Total = c.Value }), JsonRequestBehavior.AllowGet);
        }





        #region obtiene fecha y luego guarda, (modificar a un solo metodo)
        public JsonResult PruebaG()
        {

            Nomina nueva = new Nomina();

            //Convert.ToDateTime(fechaNomina);
            // nueva.DetallesNomina = new List<DetallesNomina>();

            nueva = (Nomina)Session["nueva"];

            nueva.FechaAplicacion = DateTime.Now;
            //La fecha (dd/mm/yyyy)

            string yyyy = "2019/";
            string mm = "03/";
            string dd = "9";
            string año = yyyy + mm + dd;
            nueva.FechaQuincena = Convert.ToDateTime(año);
            nueva.Observacion = "Hola P_G_F ";
            nueva.Activo = true;
            nueva.idTiposNominas = 1;

            Negocios.NominaNegocios.GuardarNomina(nueva);

            string exit = "exitoso";

            // return Json(new { exitoso = true });
            //return Json(JsonConvert.SerializeObject(exit), JsonRequestBehavior.AllowGet);
            return Json(exit, JsonRequestBehavior.AllowGet);
        }
        public JsonResult FechaG(int quincena, string mes, int ano)
        {
            int NumeroDeQuincena, AnoDeQuincena;
            string Mees;

            NumeroDeQuincena = quincena;
            AnoDeQuincena = ano;
            Mees = mes;


            ////prueba

            String A = "Exitoso3";

            //return  A;
            return Json(A, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region Se obtinen  los datos para incluirlos en el dateset para la creacion del reporte
        private Dictionary<string, decimal> ObtenerMontosPorCentroCosto(DateTime fechaInicio, DateTime fechaFin)
        {
            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);

            Dictionary<string, decimal> centrosCostos = new Dictionary<string, decimal>();

            foreach (var nomina in nominas)
            {
                var centrosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion).GroupBy(d => d.CentroCosto);
                foreach (var centroCostoEncontrado in centrosEncontrados)
                {
                    if (centrosCostos.ContainsKey(centroCostoEncontrado.Key.ToString()))
                    {
                        centrosCostos[centroCostoEncontrado.Key.ToString()] += centroCostoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        centrosCostos.Add(centroCostoEncontrado.Key.ToString(), centroCostoEncontrado.Sum(d => d.Monto));
                    }
                }
            }

            return centrosCostos;
        }


        /// <summary>
        /// Metodo para obtener todas las nominas  y poder filtrarlas por el IdTipoEmpleado de DetallesNomina 
        /// </summary>
        /// <returns> Retorna un centro de costo</returns>
        private Dictionary<string, decimal> ObtenerMontosPorCentroCostoDeConFianza(DateTime fechaInicio, DateTime fechaFin)
        {
            var NOMI = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);


            Dictionary<string, decimal> centrosCostos = new Dictionary<string, decimal>();

            foreach (var nomina in NOMI)
            {
                var centrosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion && d.IdTipoEmpleado == (int)Enumeraciones.TipoEmpleado.Confianza).GroupBy(d => d.CentroCosto);

                foreach (var centroCostoEncontrado in centrosEncontrados)
                {
                    if (centrosCostos.ContainsKey(centroCostoEncontrado.Key.ToString()))
                    {
                        centrosCostos[centroCostoEncontrado.Key.ToString()] += centroCostoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        centrosCostos.Add(centroCostoEncontrado.Key.ToString(), centroCostoEncontrado.Sum(d => d.Monto));
                    }
                }
            }

            return centrosCostos;
        }


        private Dictionary<string, decimal> ObtenerMontosPorCentroCostoDeBase(DateTime fechaInicio, DateTime fechaFin)
        {
            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);


            Dictionary<string, decimal> centrosCostos = new Dictionary<string, decimal>();

            foreach (var nomina in nominas)
            {
                var centrosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion && d.IdTipoEmpleado == (int)Enumeraciones.TipoEmpleado.Base).GroupBy(d => d.CentroCosto);

                foreach (var centroCostoEncontrado in centrosEncontrados)
                {
                    if (centrosCostos.ContainsKey(centroCostoEncontrado.Key.ToString()))
                    {
                        centrosCostos[centroCostoEncontrado.Key.ToString()] += centroCostoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        centrosCostos.Add(centroCostoEncontrado.Key.ToString(), centroCostoEncontrado.Sum(d => d.Monto));
                    }
                }
            }

            return centrosCostos;
        }


        private Dictionary<string, decimal> ObtenerMontosPorCentroCostoDeContrato(DateTime fechaInicio, DateTime fechaFin)
        {
            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);


            Dictionary<string, decimal> centrosCostos = new Dictionary<string, decimal>();

            foreach (var nomina in nominas)
            {
                var centrosEncontrados = nomina.DetallesNomina.Where(d => d.Conceptos.IdTipoConcepto == (int)Enumeraciones.TipoConcepto.Percepcion && d.IdTipoEmpleado == (int)Enumeraciones.TipoEmpleado.Contrato).GroupBy(d => d.CentroCosto);

                foreach (var centroCostoEncontrado in centrosEncontrados)
                {
                    if (centrosCostos.ContainsKey(centroCostoEncontrado.Key.ToString()))
                    {
                        centrosCostos[centroCostoEncontrado.Key.ToString()] += centroCostoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        centrosCostos.Add(centroCostoEncontrado.Key.ToString(), centroCostoEncontrado.Sum(d => d.Monto));
                    }
                }
            }

            return centrosCostos;
        }



        /// <summary>
        /// Metodo que obtiene los ids y montos de DBo.detallesNomina y luego los pasa a otro diccionario verificando que no existan duplicados 
        /// conforme a DBo.Conceptos correspondiente a ConceptosNomina y si se repiten los suma
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> ObtenerMontosPorConceptos(DateTime fechaInicio, DateTime fechaFin)
        {
            var nominas = Negocios.NominaNegocios.ObtenerPorPeriodo(fechaInicio, fechaFin);

            //obtiene y guarda la llave y su monto de cada concepto por su llave unica
            Dictionary<string, decimal> idYMontoEncontrado = new Dictionary<string, decimal>();
            foreach (var nomina in nominas)
            {
                var conceptosEncontrados = nomina.DetallesNomina.GroupBy(d => d.IdConcepto);

                foreach (var idConceptoEncontrado in conceptosEncontrados)
                {
                    if (idYMontoEncontrado.ContainsKey(idConceptoEncontrado.Key.ToString()))
                    {
                        idYMontoEncontrado[idConceptoEncontrado.Key.ToString()] += idConceptoEncontrado.Sum(d => d.Monto);
                    }
                    else
                    {
                        idYMontoEncontrado.Add(idConceptoEncontrado.Key.ToString(), idConceptoEncontrado.Sum(d => d.Monto));
                    }
                }
            }


            Dictionary<string, decimal> conceptoYMonto = new Dictionary<string, decimal>();
            var conceptosRegistrados = Negocios.ConceptosNegocios.ObtenerActivos();

            var GruposCodigosConceptos = Negocios.ConceptosNegocios.ObtenerActivos().GroupBy(d => d.Codigo);

            foreach (var conceptos in conceptosRegistrados)
            {
                foreach (string llave in idYMontoEncontrado.Keys)
                {
                    if (llave == conceptos.Id.ToString())
                    {
                        if (conceptoYMonto.ContainsKey(conceptos.ConceptoNomina))
                        {
                            conceptoYMonto[conceptos.ConceptoNomina] += idYMontoEncontrado[llave];
                        }
                        else
                        {
                            conceptoYMonto.Add(conceptos.ConceptoNomina, idYMontoEncontrado[llave]);
                        }

                    }

                }

            }


            return conceptoYMonto;
        }
        #endregion

        #region Se obtiene la fecha actual y la manda a la vista(proximamente guardará la fecha en un array)

        public int devolverAo()
        {
            //DateTime año = Convert.ToDateTime("07/06/2021");

            int año = DateTime.Now.Year;

            return año;


        }
        #endregion



        public ActionResult GenerarReporte(/*DateTime fechaInicio, DateTime fechaFin*/)
        {
            DateTime fechaInicio = Convert.ToDateTime("01/01/2019");
            DateTime fechaFin = Convert.ToDateTime("31/12/2019");
            int PMensual= 10;
            int Porcent =20;
            int NTrabajadores= 30;
            int PMenXTrabajador = 40;

            var centrosCostos = ObtenerMontosPorCentroCosto(fechaInicio, fechaFin);
            var centroCostosConfianza = ObtenerMontosPorCentroCostoDeConFianza(fechaInicio, fechaFin);
            var centroCostosBase = ObtenerMontosPorCentroCostoDeBase(fechaInicio, fechaFin);
            var centroCostosContrato = ObtenerMontosPorCentroCostoDeContrato(fechaInicio, fechaFin);


            var conceptos = ObtenerMontosPorConceptos(fechaInicio, fechaFin);
            Datasets.dtsReporteGastosSueldosSalarios dts = new Datasets.dtsReporteGastosSueldosSalarios();



            //dts.ggh.AddgghRow("","",
            foreach(var centroCosto in centrosCostos)
            {
                dts.CentroCostoTotal.AddCentroCostoTotalRow(centroCosto.Key, centroCosto.Value, PMensual, Porcent, NTrabajadores, PMenXTrabajador);
            }


            ////dts.ggh.AddgghRow("","",
            ////agrega filas con datos al data set antes creado
           foreach (var centroCosto in centroCostosConfianza)
           {
               dts.CentroCostoTotalConfianza.AddCentroCostoTotalConfianzaRow(centroCosto.Key, centroCosto.Value, PMensual, Porcent, NTrabajadores, PMenXTrabajador);
           }


           foreach (var centroCosto in centroCostosBase)
           {
               dts.CentroCostoTotalBase.AddCentroCostoTotalBaseRow(centroCosto.Key, centroCosto.Value, PMensual, Porcent, NTrabajadores, PMenXTrabajador);
           }


           foreach (var centroCosto in centroCostosContrato)
           {
               dts.CentroCostoTotalContrato.AddCentroCostoTotalContratoRow(centroCosto.Key, centroCosto.Value, PMensual, Porcent, NTrabajadores, PMenXTrabajador);
               
           }


           foreach (var concepto in conceptos)
           {
               dts.ConceptosTotal.AddConceptosTotalRow(concepto.Key, concepto.Value, PMensual, Porcent, NTrabajadores, PMenXTrabajador);

           }










            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reportes"), "GastosSueldosSalarios.rpt"));

            rd.SetDataSource(dts);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "GastosSueldosSalarios.pdf"); 
            
        }


    }

    public class DetalleConcepto
    {
        public string CentroCosto { get; set; }
        public decimal Total { get; set; }
    }

    public class Concepto
    {
        public int Id { get; set; }
        public int Columna { get; set; }
        public string NombreContable { get; set; }
        public string NombrePresupuestal { get; set; }
        public Nullable<int> TipoEmpleado { get; set; }
        public List<DetalleConcepto> Detalles;
    }

    public class CentroDeCosto
    {
        public string Nombre { get; set; }
        public decimal Total { get; set; }
        public List<DetalleCentroDeCosto> Detalles;
    }

    public class DetalleCentroDeCosto
    {
        public string Concepto { get; set; }
        public int TipoEmpleado { get; set; }
        public decimal Total { get; set; }
    }

}