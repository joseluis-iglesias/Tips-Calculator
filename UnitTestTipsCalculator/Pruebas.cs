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
        IOperaciones operaciones;
        ILogic logic;
        IService service;
        private void Inicializar()
        {
            operaciones = new  Operaciones();
            logic = new Logic(operaciones);
            service = new Service();
            
        }

        [TestMethod]
        public void ObtenerRates()
        {
            Inicializar();
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
            Inicializar();
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
            Inicializar();
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
