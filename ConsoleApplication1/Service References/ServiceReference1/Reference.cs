﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplication1.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetListaPedidos", ReplyAction="http://tempuri.org/IService/GetListaPedidosResponse")]
        string GetListaPedidos();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetListaPedidos", ReplyAction="http://tempuri.org/IService/GetListaPedidosResponse")]
        System.Threading.Tasks.Task<string> GetListaPedidosAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetListaRates", ReplyAction="http://tempuri.org/IService/GetListaRatesResponse")]
        string GetListaRates();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetListaRates", ReplyAction="http://tempuri.org/IService/GetListaRatesResponse")]
        System.Threading.Tasks.Task<string> GetListaRatesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CalcularPropina", ReplyAction="http://tempuri.org/IService/CalcularPropinaResponse")]
        string CalcularPropina(string cuenta, string moneda);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/CalcularPropina", ReplyAction="http://tempuri.org/IService/CalcularPropinaResponse")]
        System.Threading.Tasks.Task<string> CalcularPropinaAsync(string cuenta, string moneda);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : ConsoleApplication1.ServiceReference1.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<ConsoleApplication1.ServiceReference1.IService>, ConsoleApplication1.ServiceReference1.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetListaPedidos() {
            return base.Channel.GetListaPedidos();
        }
        
        public System.Threading.Tasks.Task<string> GetListaPedidosAsync() {
            return base.Channel.GetListaPedidosAsync();
        }
        
        public string GetListaRates() {
            return base.Channel.GetListaRates();
        }
        
        public System.Threading.Tasks.Task<string> GetListaRatesAsync() {
            return base.Channel.GetListaRatesAsync();
        }
        
        public string CalcularPropina(string cuenta, string moneda) {
            return base.Channel.CalcularPropina(cuenta, moneda);
        }
        
        public System.Threading.Tasks.Task<string> CalcularPropinaAsync(string cuenta, string moneda) {
            return base.Channel.CalcularPropinaAsync(cuenta, moneda);
        }
    }
}