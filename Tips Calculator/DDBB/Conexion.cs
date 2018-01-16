using MySql.Data.MySqlClient;
using System.Configuration;

namespace Tips_Calculator.DDBB
{
    public class Conexion : IConexion
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string _Server;
        private string _Database;
        private string _Uid;
        private string _Password;
       
        public MySqlConnection GetConexion()
        {
            return Initialize();
        }

        //Initialize values
        private MySqlConnection Initialize()
        {
            _Server = ConfigurationManager.AppSettings["Server"];
            _Database = ConfigurationManager.AppSettings["Database"];
            _Uid = ConfigurationManager.AppSettings["Uid"];
            _Password = ConfigurationManager.AppSettings["Password"];
            string connectionString = "SERVER=" + _Server + ";" + "DATABASE=" +
            _Database + ";" + "UID=" + _Uid + ";" + "PASSWORD=" + _Password + ";";

           return new MySqlConnection(connectionString);
        }

        public void AbrirConexion(MySqlConnection connection)
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
                        _Log.Error("No se ha podido conectar al servidor. Contacte con el admionistrador del servicio.");
                        break;

                    case 1045:
                        _Log.Error("Usuario o contaseña incorrecto, porfavor revise que sea correcto e intentelo de nuevo.");
                        break;
                }
                throw ex;
            }
        }

        public void CerrarConexion(MySqlConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (MySqlException ex)
            {
                _Log.Error(ex.Message);
                throw ex;
            }
        }
    }
}
