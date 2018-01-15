using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class Pedidos
    {
        #region Propiedades
        [DataMember]
        public string Sku { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        #endregion

        public Pedidos() { }
    }
}
