using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tips_Calculator.Logic;
using Tips_Calculator.DDBB;
using Tips_Calculator;

namespace UnitTestTipsCalculator
{
    [TestClass]
    public class Pruebas
    {
        [TestMethod]
        public void ObtenerRates()
        {
            IConexion conexion = new Conexion();
            IOperaciones operaciones = new Operaciones(conexion);
            ILogic logic = new Logic(operaciones);
            IService service = new Service(logic);
            try
            {
                service.GetListaRates();
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false, e.Message);
            }
        }

        [TestMethod]
        public void ObtenerTransacciones()
        {
            IConexion conexion = new Conexion();
            IOperaciones operaciones = new Operaciones(conexion);
            ILogic logic = new Logic(operaciones);
            IService service = new Service(logic);
            try
            {
                service.GetListaPedidos();
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false, e.Message);
            }
        }

        [TestMethod]
        public void CalcularPropinas()
        {
            IConexion conexion = new Conexion();
            IOperaciones operaciones = new Operaciones(conexion);
            ILogic logic = new Logic(operaciones);
            IService service = new Service(logic);
            try
            {
                var pedidos = logic.ObtenerPedidos();
                var rates = logic.ObtenerRates();

                logic.CalcularPropinas(pedidos[0].Sku,"EUR",rates,pedidos);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.IsTrue(false, e.Message);
            }
        }
    }
}
