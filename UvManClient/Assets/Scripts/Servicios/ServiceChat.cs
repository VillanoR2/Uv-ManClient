//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IChatService", CallbackContract=typeof(IChatServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
public interface IChatService {
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/Conectar", ReplyAction="http://tempuri.org/IChatService/ConectarResponse")]
    bool Conectar(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatService/ObtenerCuentasConectadas", ReplyAction="http://tempuri.org/IChatService/ObtenerCuentasConectadasResponse")]
    LogicaDelNegocio.Modelo.CuentaModel[] ObtenerCuentasConectadas();
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/EnviarMensaje")]
    void EnviarMensaje(GameChatService.Dominio.Message Mensaje);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/EstaEscribiendo")]
    void EstaEscribiendo(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, IsTerminating=true, Action="http://tempuri.org/IChatService/Desconectar")]
    void Desconectar(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IChatServiceCallback {
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/RefrescarCuentasConectadas")]
    void RefrescarCuentasConectadas(LogicaDelNegocio.Modelo.CuentaModel[] CuentasConectadas);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/RecibirMensaje")]
    void RecibirMensaje(GameChatService.Dominio.Message Mensaje);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/EstaEscribiendoCallback")]
    void EstaEscribiendoCallback(string Cuenta);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Unirse")]
    void Unirse(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
    
    [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatService/Abandonar")]
    void Abandonar(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface IChatServiceChannel : IChatService, System.ServiceModel.IClientChannel {
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class ChatServiceClient : System.ServiceModel.DuplexClientBase<IChatService>, IChatService {
    
    public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance) {
    }
    
    public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName) {
    }
    
    public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress) {
    }
    
    public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress) {
    }
    
    public ChatServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, binding, remoteAddress) {
    }
    
    public bool Conectar(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        return base.Channel.Conectar(Cuenta);
    }
    
    public LogicaDelNegocio.Modelo.CuentaModel[] ObtenerCuentasConectadas() {
        return base.Channel.ObtenerCuentasConectadas();
    }
    
    public void EnviarMensaje(GameChatService.Dominio.Message Mensaje) {
        base.Channel.EnviarMensaje(Mensaje);
    }
    
    public void EstaEscribiendo(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        base.Channel.EstaEscribiendo(Cuenta);
    }
    
    public void Desconectar(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        base.Channel.Desconectar(Cuenta);
    }
}
namespace GameChatService.Dominio {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/GameChatService.Dominio")]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.DateTime HoraEnvioField;
        
        private string MensajeField;
        
        private LogicaDelNegocio.Modelo.CuentaModel RemitenteField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime HoraEnvio {
            get {
                return this.HoraEnvioField;
            }
            set {
                this.HoraEnvioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mensaje {
            get {
                return this.MensajeField;
            }
            set {
                this.MensajeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogicaDelNegocio.Modelo.CuentaModel Remitente {
            get {
                return this.RemitenteField;
            }
            set {
                this.RemitenteField = value;
            }
        }
    }
}

