using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tips_Calculator.Objects
{
    [DataContract]
    public class PedidoDesglose : Pedidos
    {
        #region Propiedades
        [DataMember]
        public List<Pedidos> Pedidos { get; set; }
        #endregion
        public PedidoDesglose() { }
    }
}
