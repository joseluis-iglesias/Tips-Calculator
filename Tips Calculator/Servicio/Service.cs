using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Tips_Calculator.Objects;

namespace Tips_Calculator
{
    public class Service : IService
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _RatesURI= ConfigurationManager.AppSettings["Rates"];
        private static string _TransactionsURI = ConfigurationManager.AppSettings["Transactions"];

        public string GetListaPedidos()
        {
            List<Pedidos> listaPedidos = new List<Pedidos>();
            try
            {
                listaPedidos = JsonConvert.DeserializeObject<List<Pedidos>>(ObtenerDatos(_TransactionsURI));
                Logic.Logic.GuardarPedidos(listaPedidos);
            }
            catch (WebException wex)
            {
                _Log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo." + wex.Message);
                listaPedidos = Logic.Logic.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
            return JsonConvert.SerializeObject(listaPedidos);
        }

        public string GetListaRates()
        {
            List<Rates> listaRates = new List<Rates>();
            try
            {
                listaRates = JsonConvert.DeserializeObject<List<Rates>>(ObtenerDatos(_RatesURI));
                Logic.Logic.GuardarRates(listaRates);
            }
            catch (WebException wex)
            {
                _Log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo."+ wex.Message);
                listaRates = Logic.Logic.ObtenerRates();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return JsonConvert.SerializeObject(listaRates);
        }

        public string CalcularPropina(string cuenta, string moneda)
        {
            List<Pedidos> pedidos;
            List<Rates> rates;
            List<Propinas> propinas;
            try
            {
                pedidos = JsonConvert.DeserializeObject<List<Pedidos>>(ObtenerDatos(_TransactionsURI));
                rates = JsonConvert.DeserializeObject<List<Rates>>(ObtenerDatos(_RatesURI));
            }
            catch (WebException wex)
            {
                _Log.Warn("Error al intentar acceder a la url especificada. Continuamos la operacion recogiendo los datos de nuestro respaldo.");
                _Log.Error(wex.Message);
                rates = Logic.Logic.ObtenerRates();
                pedidos = Logic.Logic.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                _Log.Error("Error en ObtenerDatos: " + ex.Message);
                throw ex;
            }
            try
            {
                propinas = Logic.Logic.CalcularPropinas(cuenta, moneda, rates, pedidos);
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de calcular las propinas, si este error persiste pongase en contacto con el administrador.");
                throw ex;
            }
            return JsonConvert.SerializeObject(propinas);
        }

        public string ObtenerDatos(string uri ) {
            string json = "";
            try
            {
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(uri);
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
            return json;
        }
    }
}