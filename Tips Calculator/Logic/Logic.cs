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
                pedidoDesglose.PedidosPropina = ObtenerPropinas(pedidosSelect);
                var pedidosDifMon = pedidosSelect.FindAll(x => x.Currency != moneda);
                var pedidosIgMon = pedidosSelect.FindAll(x => x.Currency == moneda);
                decimal amountTotalIg = (from pedido in pedidosIgMon select pedido.Amount).Sum();
                foreach (var pedido in pedidosDifMon)
                {
                    pedido.Amount = RedondearDecimales(pedido.Amount * CalcularRates(moneda, rates, pedido.Currency, new List<string>()));
                    pedido.Currency = moneda;
                }
                decimal amountTotalDis = (from pedido in pedidosDifMon select pedido.Amount).Sum();
                
                pedidoDesglose.Amount = RedondearDecimales(amountTotalDis + amountTotalIg);
                pedidoDesglose.Tip = RedondearDecimales(CalcularPropina(pedidoDesglose.Amount));
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
                    Amount= pedido.Amount,
                    Currency= pedido.Currency,
                    Sku=pedido.Sku,
                    Tip = CalcularPropina(pedido.Amount)
                };
                propinas.Add(propina);
                
            }
            return propinas;
        }

        private static decimal CalcularRates(string moneda, List<Rate> rates, string pedido, List<string> monedasObtenidas )
        {
            try
            {
                Rate r = rates.Find(x => x.From == moneda && x.To == pedido);


                if (r != null)
                {
                    return r.Cambio;
                }
                else
                {
                    List<Rate> l = rates.FindAll(x => x.From == moneda && !monedasObtenidas.Any(y => y == moneda));
                    if (l.Count > 0)
                    {
                        foreach (Rate elem in l)
                        {
                            monedasObtenidas.Add(moneda);
                            decimal d = elem.Cambio * CalcularRates(elem.To, rates, pedido, monedasObtenidas);
                            if (d != 0) return d;
                        }
                    }
                }
                return 0;
            }  
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de realizar el Calculo del cambio de divisas");
                throw ex;
            }
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