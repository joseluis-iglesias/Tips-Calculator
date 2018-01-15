using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Tips_Calculator.DDBB
{
    class Conexion
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public MySqlConnection GetConexion()
        {
            return Initialize();
        }

        //Initialize values
        private MySqlConnection Initialize()
        {
            server = "localhost";
            database = "Tips_Calculator";
            uid = "jose";
            password = "JoseLuis31";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

           return new MySqlConnection(connectionString);
        }

        public static void AbrirConexion(MySqlConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        log.Error("No se ha podido conectar al servidor. Contacte con el admionistrador del servicio.");
                        break;

                    case 1045:
                        log.Error("Usuario o contaseña incorrecto, porfavor revise que sea correcto e intentelo de nuevo.");
                        break;
                }
                throw ex;
            }
        }

        public static void CerrarConexion(MySqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }
    }
}
