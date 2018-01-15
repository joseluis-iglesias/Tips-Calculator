using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using Tips_Calculator.Objects;

namespace Tips_Calculator
{
    public class Service : IService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ratesURI= "http://quiet-stone-2094.herokuapp.com/rates.json";
        private const string transactionsURI = "http://quiet-stone-2094.herokuapp.com/transactions.json";

        public string GetListaPedidos()
        {
            List<Pedidos> listaPedidos = new List<Pedidos>();
            try
            {
                listaPedidos = JsonConvert.DeserializeObject<List<Pedidos>>(ObtenerDatos(transactionsURI));
                Logic.Logic.GuardarPedidos(listaPedidos);
            }
            catch (WebException wex)
            {
                log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo." + wex.Message);
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
                listaRates = JsonConvert.DeserializeObject<List<Rates>>(ObtenerDatos(ratesURI));
                Logic.Logic.GuardarRates(listaRates);
            }
            catch (WebException wex)
            {
                log.Info("Se ha detectado un error en la conexion, procedemos a recoger los datos de nuestro respaldo."+ wex.Message);
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
                pedidos = JsonConvert.DeserializeObject<List<Pedidos>>(ObtenerDatos(transactionsURI));
                rates = JsonConvert.DeserializeObject<List<Rates>>(ObtenerDatos(ratesURI));
            }
            catch (WebException wex)
            {
                log.Warn("Error al intentar acceder a la url especificada. Continuamos la operacion recogiendo los datos de nuestro respaldo.");
                log.Error(wex.Message);
                rates = Logic.Logic.ObtenerRates();
                pedidos = Logic.Logic.ObtenerPedidos();
            }
            catch (Exception ex)
            {
                log.Error("Error en ObtenerDatos: " + ex.Message);
                throw ex;
            }
            try
            {
                propinas = Logic.Logic.CalcularPropinas(cuenta, moneda, rates, pedidos);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Warn("Error a la hora de calcular las propinas, si este error persiste pongase en contacto con el administrador.");
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
                log.Warn("Error al intentar acceder a la url especificada. Continuamos la operacion recogiendo los datos de nuestro respaldo.");
                log.Error(wex.Message);
                throw wex;
            }
            catch (Exception ex)
            {
                log.Error("Error en ObtenerDatos: " + ex.Message);
                throw ex;
            }
            return json;
        }
    }
}