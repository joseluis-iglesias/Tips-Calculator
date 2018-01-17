using MySql.Data.MySqlClient;
using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class Rate
    {
        #region propiedades
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public decimal Cambio { get; set; }
        #endregion

        public Rate() { }

    }
}
