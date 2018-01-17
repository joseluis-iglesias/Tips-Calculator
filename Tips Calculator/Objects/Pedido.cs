using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class Pedido
    {
        #region Propiedades
        [DataMember]
        public string Sku { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal Tip { get; set; }
        #endregion

        public Pedido() { }
    }
}
