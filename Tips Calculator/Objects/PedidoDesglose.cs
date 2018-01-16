using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class PedidoDesglose
    {
        #region Propiedades
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Tip { get; set; }
        [DataMember]
        public List<Pedido> Pedidos { get; set; }
        #endregion
        public PedidoDesglose() { }
    }
}
