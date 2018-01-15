using System.ServiceModel;

namespace Tips_Calculator
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetListaPedidos();

        [OperationContract]
        string GetListaRates();

        [OperationContract]
        string CalcularPropina(string cuenta, string moneda);
    }
}