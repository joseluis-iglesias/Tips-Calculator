using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Tips_Calculator.Objects;

namespace Tips_Calculator.DDBB
{
    public class Operaciones : IOperaciones
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string _InsertarRates = "`tips_calculator`.`InsertarRates`";
        private const string _ObtenerRates = "`tips_calculator`.`ObtenerRates`";
        private const string _EliminarRates = "`tips_calculator`.`EliminarRates`";
        private const string _InsertarPedidos = "`tips_calculator`.`InsertarTransactions`";
        private const string _ObtenerPedidos = "`tips_calculator`.`ObtenerTransactions`";
        private const string _EliminarPedidos = "`tips_calculator`.`EliminarTransactions`";
        protected IConexion conexion;

        public Operaciones(IConexion conexion)
        {
            this.conexion = conexion;
        }

        public void InsertarRates(List<Rate> rates)
        {
            using (MySqlConnection conn = conexion.GetConexion())
            {
                conexion.AbrirConexion(conn);
                MySqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(_EliminarRates, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (Rate rate in rates)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(_InsertarRates, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new MySqlParameter("moneda1", rate.From));
                            cmd.Parameters.Add(new MySqlParameter("moneda2", rate.To));
                            cmd.Parameters.Add(new MySqlParameter("rate", rate.Cambio));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    _Log.Error(ex.Message);
                    _Log.Warn("Procedemos al rollback de los datos");
                    transaction.Rollback();
                }
            }
        }

        public void InsertarPedidos(List<Pedido> pedidos)
        {
            using (MySqlConnection conn =  conexion.GetConexion())
            {
                conexion.AbrirConexion(conn);
                MySqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(_EliminarPedidos, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (Pedido pedido in pedidos)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(_InsertarPedidos, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new MySqlParameter("sku", pedido.Sku));
                            cmd.Parameters.Add(new MySqlParameter("amount", pedido.Amount));
                            cmd.Parameters.Add(new MySqlParameter("currency", pedido.Currency));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    _Log.Error(ex.Message);
                    _Log.Warn("Procedemos al rollback de los datos");
                    transaction.Rollback();
                }
            }
        }

        public List<Rate> ObtenerRates()
        {
            List<Rate> rates = new List<Rate>();
            using (MySqlConnection conn =  conexion.GetConexion())
            {
                conexion.AbrirConexion(conn);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(_ObtenerRates, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Rate rate = new Rate();
                            rate.From = reader["FROM"].ToString();
                            rate.To = reader["TO"].ToString();
                            rate.Cambio = (decimal)reader["RATE"];
                            rates.Add(rate);
                        }
                    }
                    conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    _Log.Error(ex.Message);
                    _Log.Warn("Error a la hora de recoger los datos de la DDBB");
                }
            }
            return rates;
        }

        public List<Pedido> ObtenerPedidos()
        {
            List<Pedido> pedidos = new List<Pedido>();
            using (MySqlConnection conn =  conexion.GetConexion())
            {
                conexion.AbrirConexion(conn);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(_ObtenerPedidos, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Pedido pedido = new Pedido();
                            pedido.Currency = reader["CURRENCY"].ToString();
                            pedido.Amount = (decimal)reader["AMOUNT"];
                            pedido.Sku = reader["SKU"].ToString();
                            pedidos.Add(pedido);
                        }
                    }
                    conexion.CerrarConexion(conn);
                }

                catch (Exception ex)
                {
                    _Log.Error(ex.Message);
                    _Log.Warn("Error a la hora de recoger los datos de la DDBB");
                }
            }
            return pedidos;
        }
    }
}