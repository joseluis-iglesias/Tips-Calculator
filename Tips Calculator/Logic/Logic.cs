using System;
using System.Collections.Generic;
using System.Linq;
using Tips_Calculator.DDBB;
using Tips_Calculator.Objects;

namespace Tips_Calculator.Logic
{
    public class Logic : ILogic
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static decimal _Porcentaje = 0.05M;

        protected IOperaciones operaciones;

        public Logic(IOperaciones operaciones)
        {
            this.operaciones = operaciones;
        }

        public void GuardarRates(List<Rate> rates)
        {
            try
            {
                operaciones.InsertarRates(rates);
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de guardar/borrar los rates: " + ex.Message);
                throw ex;
            }
        }

        public void GuardarPedidos(List<Pedido> pedidos)
        {
            try
            {
                operaciones.InsertarPedidos(pedidos);
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de guardar/borrar los pedidos: " + ex.Message);
                throw ex;
            }
        }

        public  List<Rate> ObtenerRates()
        {
            List<Rate> rates;
            try
            {
                rates = operaciones.ObtenerRates();
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de obtener los rates: " + ex.Message);
                throw ex;
            }
            return rates;
        }

        public  List<Pedido> ObtenerPedidos()
        {
            List<Pedido> pedidos;
            try
            {
                pedidos = operaciones.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                _Log.Error("Error detectado a lo hora de obtener los pedidos: " + ex.Message);
                throw ex;
            }
            return pedidos;
        }

        public PedidoDesglose CalcularPropinas(string cuenta, string moneda, List<Rate> rates, List<Pedido> pedidos)
        {
            PedidoDesglose pedidoDesglose = new PedidoDesglose(){ Currency = moneda, Sku = cuenta };
            try
            {
                var pedidosSelect = pedidos.FindAll(x => x.Sku == cuenta);
                var pedidosDifMon = pedidosSelect.FindAll(x => x.Currency != moneda);
                var pedidosIgMon = pedidosSelect.FindAll(x => x.Currency == moneda);
                decimal amountTotalIg = (from pedido in pedidosIgMon select pedido.Amount).Sum();
                foreach (var pedido in pedidosDifMon)
                {
                    pedido.Amount = RedondearDecimales(pedido.Amount * CalcularRates(moneda, rates, pedido.Currency));
                }
                decimal amountTotalDis = (from pedido in pedidosDifMon select pedido.Amount).Sum();
                pedidoDesglose.PedidosPropina = ObtenerPropinas(pedidosSelect);
                pedidoDesglose.Amount = RedondearDecimales(CalcularPropina(amountTotalDis + amountTotalIg));
                pedidoDesglose.Tip = CalcularPropina(pedidoDesglose.Amount);
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                throw ex;
            }
            return pedidoDesglose;
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

        private static decimal CalcularRates(string moneda, List<Rate> rates, string pedido)
        {
            try
            {
                if (rates.Exists(x => x.From == pedido && x.To == moneda))
                {
                    return RedondearDecimales(rates.Find(x => x.From == pedido && x.To == moneda).Cambio);
                }
                else
                {
                    return buscarRate(moneda,rates,pedido);
                }
            }  
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de realizar el Calculo del cambio de divisas");
                throw ex;
            }
        }
        private static decimal buscarRate(string moneda, List<Rate> rates, string pedido)
        {
            Rate cambio = new Rate();
            if (!pedido.Equals(moneda))
            {
                var rate = rates.FindAll(x => x.From == pedido);
                if (rate.Count > 1)
                {
                    foreach (var rt in rate)
                    {
                        if (rates.Exists(x => x.From == rt.To && x.To == moneda))
                        {
                            return RedondearDecimales(rt.Cambio * (rates.Find(x => x.From == rt.To && x.To == moneda).Cambio));
                        }
                    }
                }
                else
                {
                    cambio = rate.FirstOrDefault();
                    return RedondearDecimales(cambio.Cambio * buscarRate(pedido, rates, cambio.To));
                }
            }
                return 1;
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
            return Math.Round(cantidad, 2, MidpointRounding.ToEven);
        }
    }
}