using System;
using System.Web.Mvc;
using ISSSTECAM.Presupuesto.Web;
using ISSSTECAM.Presupuesto.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ISSSTECAM.Presupuesto.Web.Tests
{
    [TestClass]
    public class ClavesPresupuestalesTests
    {

        #region Crear mas tests de validaciones para cada caso posible a darse dentro de reduccion 

        //Metodo para probar la reduccion automatizada
        [TestMethod]
        public void TestDeReduccionConValoresCorrectos()
        {
            //instancia del controlador a probar
            var claseAProbar = new ClavesPresupuestalesController();

            //instancia del modelo que se compobara
            // y llenado del modelo para su validacion posterior
            var cuentaOrigen = new  Controllers.DatosDeClaves();
            cuentaOrigen.clave = "21120283626211C016000J187039010790L415A4521";
            cuentaOrigen.mes = 1;
            cuentaOrigen.monto = 0.01M;
            string motivo = "test unitario";
            
            
            //Intento de verificar todo lo anterio dentro del metodo Reduccion del controlador
            JsonResult resultado = claseAProbar.Reducciones(cuentaOrigen, motivo);
            var datoRecibido = resultado.Data.ToString();
          

        }

        #endregion



        #region Test para transacciones 
        //crear test probando cada posible esenario
        #endregion


        #region Test para transacciones de muchas claver a una en especifico 
        //crear test probando cada posible esenario
        #endregion



    }

}
