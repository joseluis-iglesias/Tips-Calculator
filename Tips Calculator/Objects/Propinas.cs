using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class Propinas
    {
        [DataMember]
        public string Sku { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Tip { get; set; }

        public Propinas() { }

        public Propinas(Pedidos pedido)
        {
            Amount = pedido.Amount;
            Currency = pedido.Currency;
            Sku = pedido.Sku;
        }
    }
}
