using System.Collections.Generic;
using System.ServiceModel;
using Tips_Calculator.Objects;

namespace Tips_Calculator.DDBB
{
    [ServiceContract]
    public interface IOperaciones
    {
        [OperationContract]
        void InsertarRates(List<Rate> rates);

        [OperationContract]
        void InsertarPedidos(List<Pedido> pedidos);

        [OperationContract]
        List<Rate> ObtenerRates();

        [OperationContract]
        List<Pedido> ObtenerPedidos();

    }
}