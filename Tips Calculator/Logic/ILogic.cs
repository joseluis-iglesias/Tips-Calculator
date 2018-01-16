using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Tips_Calculator.Objects;

namespace Tips_Calculator.Logic
{
    [ServiceContract]
    public interface ILogic 
    {
        void obtenerdatosRates(List<Rates> rates);
    }
}
