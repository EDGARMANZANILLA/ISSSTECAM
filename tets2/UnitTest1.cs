using System;
using System.Web.Mvc;
using ISSSTECAM.Presupuesto.Web;
using ISSSTECAM.Presupuesto.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tets2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var claseAProbar = new ClavesPresupuestalesController();
            var cuentaOrigen = new DatosDeClaves();

            cuentaOrigen.clave = "21120283626211C016000J187039010790L415A4521";
            cuentaOrigen.mes = 1;
            cuentaOrigen.monto = 0.01M;

            string motivo = "test unitario";
            JsonResult resultado = claseAProbar.Reducciones(cuentaOrigen, motivo);
            var a = resultado.Data.ToString();

        }
    }
}
