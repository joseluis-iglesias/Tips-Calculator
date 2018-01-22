using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Tips_Calculator.Objects;
using Tips_Calculator.Logic;
using Tips_Calculator.DDBB;

namespace Tips_Calculator
{
    public class Service : IService
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _RatesURI= ConfigurationManager.AppSettings["Rates"];
        private static string _TransactionsURI = ConfigurationManager.AppSettings["Transactions"];
        protected ILogic logic;
        public Service(ILogic logic)
        {
            this.logic = logic;
        }
        private ILogic Initialize()
        {
            IOperaciones operaciones = new Operaciones();
            ILogic logic = new Logic.Logic(operaciones);
            return logic;
        }
        public Service()
        {
            this.logic = Initialize();
        }

        public string GetListaPedidos()
        {
            List<Pedido> listaPedidos = new List<Pedido>();
            try
            {
                listaPedidos = JsonConvert.DeserializeObject<List<Pedido>>(ObtenerDatos(_TransactionsURI));
                logic.GuardarPedidos(listaPedidos);
            }
            catch (WebException wex)
            {
                _Log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo." + wex.Message);
                listaPedidos = logic.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
            return JsonConvert.SerializeObject(listaPedidos);
        }

        public string GetListaRates()
        {
            List<Rate> listaRates = new List<Rate>();
            try
            {
                listaRates = JsonConvert.DeserializeObject<List<Rate>>(ObtenerDatos(_RatesURI));
                logic.GuardarRates(listaRates);
            }
            catch (WebException wex)
            {
                _Log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo."+ wex.Message);
                listaRates = logic.ObtenerRates();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return JsonConvert.SerializeObject(listaRates);
        }
        public string CalcularPropina(string cuenta, string moneda)
        {
            List<Pedido> pedidos;
            List<Rate> rates;
            PedidoDesglose pedidoDesglose;
            try
            {
                pedidos = JsonConvert.DeserializeObject<List<Pedido>>(ObtenerDatos(_TransactionsURI));
                rates = JsonConvert.DeserializeObject<List<Rate>>(ObtenerDatos(_RatesURI));
            }
            catch (WebException wex)
            {
                _Log.Warn("Error al intentar acceder a la url especificada. Continuamos la operacion recogiendo los datos de nuestro respaldo.");
                _Log.Error(wex.Message);
                rates = logic.ObtenerRates();
                pedidos = logic.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                _Log.Error("Error en ObtenerDatos: " + ex.Message);
                throw ex;
            }
            try
            {
                pedidoDesglose = logic.CalcularPropinas(cuenta, moneda, rates, pedidos);
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de calcular las propinas, si este error persiste pongase en contacto con el administrador.");
                throw ex;
            }
            return JsonConvert.SerializeObject(pedidos);
        }

        public string ObtenerDatos(string uri ) {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    return wc.DownloadString(uri);
                }
            }
            catch (WebException wex)
            {
                _Log.Warn("Error al intentar acceder a la url especificada. Continuamos la operacion recogiendo los datos de nuestro respaldo.");
                _Log.Error(wex.Message);
                throw wex;
            }
            catch (Exception ex)
            {
                _Log.Error("Error en ObtenerDatos: " + ex.Message);
                throw ex;
            }
        }
    }
}