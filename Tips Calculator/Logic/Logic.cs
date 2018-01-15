using System;
using System.Collections.Generic;
using System.Linq;
using Tips_Calculator.Objects;

namespace Tips_Calculator.Logic
{
    public class Logic
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<Rates> GuardarRates(List<Rates> rates)
        {
            try
            {
                DDBB.Operaciones.InsertarRates(rates);
            }
            catch (Exception ex)
            {
                log.Error("Error detectado a lo hora de guardar/borrar los rates: " + ex.Message);
                throw ex;
            }
            return rates;
        }

        public static List<Pedidos> GuardarPedidos(List<Pedidos> pedidos)
        {
            try
            {
                DDBB.Operaciones.InsertarPedidos(pedidos);
            }
            catch (Exception ex)
            {
                log.Error("Error detectado a lo hora de guardar/borrar los pedidos: " + ex.Message);
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
                log.Error("Error detectado a lo hora de obtener los rates: " + ex.Message);
                throw ex;
            }
            return rates;
        }

        public static List<Pedidos> ObtenerPedidos()
        {
            List<Pedidos> pedidos;
            try
            {
                pedidos = DDBB.Operaciones.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                log.Error("Error detectado a lo hora de obtener los pedidos: " + ex.Message);
                throw ex;
            }
            return pedidos;
        }

        public static List<Propinas> CalcularPropinas(string cuenta, string moneda, List<Rates> rates, List<Pedidos> pedidos)
        {
            List<Propinas> propinas = new List<Propinas>();
            Propinas propina = new Propinas();
            try
            {
                var pedidosSelect = pedidos.FindAll(x => x.Sku == cuenta);
                foreach (var pedido in pedidosSelect)
                {
                    propina = new Propinas(pedido)
                    {
                        Tip = CalcularPropina(pedido.Amount)
                    };
                    propinas.Add(propina);
                    if (pedido.Currency != moneda)
                    {
                        var changes = from rate in rates where (rate.From == pedido.Currency ) && (rate.To == moneda) select rate;
                        if (changes.Count() > 0)
                        {
                            foreach (var cambio in changes)
                            {
                                propina = new Propinas(pedido)
                                {
                                    Currency = moneda,
                                    Amount = cambio.Rate * pedido.Amount,
                                    Tip = CalcularPropina(cambio.Rate * pedido.Amount)
                                };
                                propinas.Add(propina);
                            }
                        }
                        else
                        {
                            changes = from rate in rates where (rate.From == pedido.Currency) select rate;
                            foreach (var cambio in changes)
                            {
                                var cambios = rates.FindAll(x => x.To == moneda && x.From == cambio.To);
                                if (cambios.Count() > 0)
                                {
                                    foreach (var rate in cambios)
                                    {
                                        propina = new Propinas(pedido)
                                        {
                                            Currency = moneda,
                                            Amount = rate.Rate * pedido.Amount,
                                            Tip = CalcularPropina(rate.Rate * pedido.Amount)
                                        };
                                        propinas.Add(propina);
                                    }
                                }
                                else
                                {
                                    var tip = rates.FindAll(x => x.To == moneda && x.From == cambio.From);
                                    if (tip.Count > 0)
                                    {
                                        foreach (Rates rate in tip)
                                        {
                                            if (tip.Count() > 0)
                                            {
                                                propina = new Propinas(pedido)
                                                {
                                                    Currency = moneda,
                                                    Amount = rate.Rate * pedido.Amount,
                                                    Tip = CalcularPropina(rate.Rate * pedido.Amount)
                                                };
                                                propinas.Add(propina);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        /*REVISAR MAÑANA*/
                                       tip = rates.FindAll(x => x.To == cambio.To && x.From == cambio.From);
                                       var def = tip.FindAll(x => x.To == moneda && x.From == cambio.To);
                                        if (def.Count() <= 0)
                                        {
                                            def = tip.FindAll(x => x.To == cambio.To && x.From == cambio.From);
                                        }
                                        foreach (Rates rate in def)
                                        {
                                            if (tip.Count() > 0)
                                            {
                                                propina = new Propinas(pedido)
                                                {
                                                    Currency = moneda,
                                                    Amount = rate.Rate * pedido.Amount,
                                                    Tip = CalcularPropina(rate.Rate * pedido.Amount)
                                                };
                                                propinas.Add(propina);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
            return propinas;
        }


        private static decimal CalcularPropina(decimal amount)
        {
            try
            {
                return Math.Round(Math.Truncate(amount * 5) / 100, 2, MidpointRounding.ToEven);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Warn("Error a la hora de realizar el Calculo de la propina");
                throw ex;
            }
        }
    }
}