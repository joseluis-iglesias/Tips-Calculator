using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = "Rate")]
        public decimal Cambio { get; set; }
        #endregion

        public Rate() { }

    }
}
