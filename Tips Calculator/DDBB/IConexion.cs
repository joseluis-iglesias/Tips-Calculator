using MySql.Data.MySqlClient;
using System.ServiceModel;

namespace Tips_Calculator.DDBB
{
    [ServiceContract]
    public interface IConexion
    {
        [OperationContract]
        MySqlConnection GetConexion();
        [OperationContract]
        void AbrirConexion(MySqlConnection connection);
        [OperationContract]
        void CerrarConexion(MySqlConnection connection);
    }
}
