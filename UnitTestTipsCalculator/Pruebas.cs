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
            operaciones = new Operaciones();
            logic = new Logic(operaciones);
            service = new Service();

        }

        [TestMethod]
        public void ObtenerRates()
        {
            Inicializar();
            try
            {
                var ratesJson = service.GetListaRates();
                if (ratesJson != null && !ratesJson.Equals(""))
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void ObtenerTransacciones()
        {
            Inicializar();
            try
            {
                var transaccionesJson = service.GetListaPedidos();
                if (transaccionesJson != null && !transaccionesJson.Equals(""))
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
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

                var propinas = logic.CalcularPropinas(pedidos[0].Sku, "EUR", rates, pedidos);
                if (propinas != null)
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}