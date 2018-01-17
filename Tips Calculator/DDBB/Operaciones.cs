using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tips_Calculator.Objects;

namespace Tips_Calculator.DDBB
{
    public class Operaciones : IOperaciones
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void InsertarRates(List<Rate> rates)
        {
            StringBuilder contentFile = new StringBuilder();
            string filePath = @"C:\Rates.json";
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(rates));
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error al crear el archivo");
                throw ex;
            }
        }

        public void InsertarPedidos(List<Pedido> pedidos)
        {
            StringBuilder contentFile = new StringBuilder();
            string filePath = @"C:\Pedidos.json";
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(pedidos));
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error al crear el archivo");
                throw ex;
            }
        }

        public List<Rate> ObtenerRates()
        {
            try
            {
                using (StreamReader file = new StreamReader(@"C:\Rates.json"))
                {
                    return JsonConvert.DeserializeObject<List<Rate>>(file.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de recoger los datos del archivo");
                throw ex;
            }
        }


        public List<Pedido> ObtenerPedidos()
        {
            try
            {
                using (StreamReader file = new StreamReader(@"C:\Pedidos.json"))
                {
                    return JsonConvert.DeserializeObject<List<Pedido>>(file.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                _Log.Error(ex.Message);
                _Log.Warn("Error a la hora de recoger los datos del archivo");
                throw ex;
            }
        }
    }
}