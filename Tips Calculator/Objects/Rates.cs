using MySql.Data.MySqlClient;
using System.Runtime.Serialization;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class Rates
    {
        #region propiedades
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        #endregion

        public Rates() { }

    }
}
