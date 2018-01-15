﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tips_Calculator.Objects;

namespace Tips_Calculator.DDBB
{
    public class Operaciones
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string insertarRates = "`tips_calculator`.`InsertarRates`";
        private const string obtenerRates = "`tips_calculator`.`ObtenerRates`";
        private const string eliminarRates = "`tips_calculator`.`EliminarRates`";
        private const string insertarPedidos = "`tips_calculator`.`InsertarTransactions`";
        private const string obtenerPedidos = "`tips_calculator`.`ObtenerTransactions`";
        private const string eliminarPedidos = "`tips_calculator`.`EliminarTransactions`";

        public static void InsertarRates(List<Rates> rates)
        {
            using (MySqlConnection conn = new Conexion().GetConexion())
            {
                Conexion.AbrirConexion(conn);
                MySqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(eliminarRates, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (Rates rate in rates)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(insertarRates, conn))
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new MySqlParameter("moneda1", rate.From));
                            cmd.Parameters.Add(new MySqlParameter("moneda2", rate.To));
                            cmd.Parameters.Add(new MySqlParameter("rate", rate.Rate));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    Conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Warn("Procedemos al rollback de los datos");
                    transaction.Rollback();
                }
            }
        }

        public static void InsertarPedidos(List<Pedidos> pedidos)
        {
            using (MySqlConnection conn = new Conexion().GetConexion())
            {
                Conexion.AbrirConexion(conn);
                MySqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(eliminarPedidos, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    foreach (Pedidos pedido in pedidos)
                    {
                        using (MySqlCommand cmd = new MySqlCommand(insertarPedidos, conn))
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
                    Conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Warn("Procedemos al rollback de los datos");
                    transaction.Rollback();
                }
            }
        }

        public static List<Rates> ObtenerRates()
        {
            List<Rates> rates = new List<Rates>();
            using (MySqlConnection conn = new Conexion().GetConexion())
            {
                Conexion.AbrirConexion(conn);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(obtenerRates, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Rates rate = new Rates();
                            rate.From = reader["FROM"].ToString();
                            rate.To = reader["TO"].ToString();
                            rate.Rate = (decimal)reader["RATE"];
                            rates.Add(rate);
                        }
                    }
                    Conexion.CerrarConexion(conn);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Warn("Error a la hora de recoger los datos de la DDBB");
                }
            }
            return rates;
        }

        public static List<Pedidos> ObtenerPedidos()
        {
            List<Pedidos> pedidos = new List<Pedidos>();
            using (MySqlConnection conn = new Conexion().GetConexion())
            {
                Conexion.AbrirConexion(conn);
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(obtenerPedidos, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Pedidos pedido = new Pedidos();
                            pedido.Currency = reader["CURRENCY"].ToString();
                            pedido.Amount = (decimal)reader["AMOUNT"];
                            pedido.Sku = reader["SKU"].ToString();
                            pedidos.Add(pedido);
                        }
                    }
                    Conexion.CerrarConexion(conn);
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    log.Warn("Error a la hora de recoger los datos de la DDBB");
                }
            }
            return pedidos;
        }
    }
}