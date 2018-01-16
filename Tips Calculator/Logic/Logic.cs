using System;
using System.Collections.Generic;
using System.Linq;
using Tips_Calculator.Objects;

namespace Tips_Calculator.Logic
{
    public class Logic : ILogic
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static decimal _Porcentaje = 0.05M;

        public static List<Rates> GuardarRates(List<Rates> rates)
        {
            try
            {
                DDBB.Operaciones.InsertarRates(rates);
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de guardar/borrar los rates: " + ex.Message);
                throw ex;
            }
            return rates;
        }

        public static List<Pedido> GuardarPedidos(List<Pedido> pedidos)
        {
            try
            {
                DDBB.Operaciones.InsertarPedidos(pedidos);
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de guardar/borrar los pedidos: " + ex.Message);
                throw ex;
            }
            return pedidos;
        }

        public static List<Rates> ObtenerRates()
        {
            List<Rates> rates;
            try
            {
                rates = DDBB.Operaciones.ObtenerRates();
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de obtener los rates: " + ex.Message);
                throw ex;
            }
            return rates;
        }

        public static List<Pedido> ObtenerPedidos()
        {
            List<Pedido> pedidos;
            try
            {
                pedidos = DDBB.Operaciones.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de obtener los pedidos: " + ex.Message);
                throw ex;
            }
            return pedidos;
        }
        /* TO DO*/
        public static List<Pedido> CalcularPropinas(string cuenta, string moneda, List<Rates> rates, List<Pedido> pedidos)
        {
            List<Pedido> propinas = new List<Pedido>();
            PedidoDesglose pedidoDesglose = new PedidoDesglose();
            try
            {
                var pedidosSelect = pedidos.FindAll(x => x.Sku == cuenta);
                propinas = ObtenerPropinas(pedidosSelect);
                var pedidosDifMon = pedidosSelect.FindAll(x => x.Currency != moneda);
                var pedidosIgMon = pedidosSelect.FindAll(x => x.Currency == moneda);
                decimal amountTotalIg = (from pedido in pedidosIgMon select pedido.Amount).Sum();
                foreach (var pedido in pedidosDifMon)
                {
                    pedido.Amount = RedondearDecimales( pedido.Amount * CalcularRates(moneda, rates, pedido.Currency));
                }
                decimal amountTotalDis = (from pedido in pedidosDifMon select pedido.Amount).Sum();
                pedidoDesglose.Amount = RedondearDecimales(CalcularPropina(amountTotalDis + amountTotalIg));
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                throw ex;
            }
            return propinas;
        }

        private static List<Pedido> ObtenerPropinas(List<Pedido> pedidos)
        {
            List<Pedido> propinas = new List<Pedido>();
            Pedido propina = new Pedido();
            foreach (var pedido in pedidos)
            {
                propina = new Pedido()
                {
                    Tip = CalcularPropina(pedido.Amount)
                };
                propinas.Add(propina);
                
            }
            return propinas;
        }

        private static decimal CalcularRates(string moneda, List<Rates> rates, string pedido)
        {
            decimal tip = 1;
            var rate = rates.FindAll(x => x.From == pedido && x.To == moneda);
            if (rate.Count() > 0)
            {
                foreach (var t in rate)
                    tip = tip* t.Rate;
                 RedondearDecimales(tip);
            }
            else
            {
                rate = rates.FindAll(x => x.From == pedido);
                foreach (var rt in rate)
                {
                    var ratos = rates.FindAll(x => x.From == rt.To && x.To == moneda);
                    if (ratos.Count() > 0 )
                        tip = RedondearDecimales(tip * CalcularRates(rt.To, rates, pedido));
                }
            }
            return tip;
        }

        private static decimal CalcularPropina(decimal amount)
        {
            try
            {
                return (amount * _Porcentaje);
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de realizar el Calculo de la propina");
                throw ex;
            }
        }

        private static decimal RedondearDecimales(decimal cantidad)
        {
            return Math.Round(Math.Truncate(cantidad), 2, MidpointRounding.ToEven);
        }
    }
}