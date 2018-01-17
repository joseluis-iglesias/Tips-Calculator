using System.Collections.Generic;
using System.ServiceModel;
using Tips_Calculator.Objects;

namespace Tips_Calculator.Logic
{
    [ServiceContract]
    public interface ILogic 
    {
        [OperationContract]
        List<Rate> ObtenerRates();

        [OperationContract]
        List<Pedido> ObtenerPedidos();

        [OperationContract]
        void GuardarRates(List<Rate> rates);

        [OperationContract]
        void GuardarPedidos(List<Pedido> pedidos);

        [OperationContract]
        PedidoDesglose CalcularPropinas(string cuenta, string moneda, List<Rate> rates, List<Pedido> pedidos);
    }
}
