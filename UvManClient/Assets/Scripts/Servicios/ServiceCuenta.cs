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
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="ICuentaService")]
public interface ICuentaService {
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICuentaService/Registrarse", ReplyAction="http://tempuri.org/ICuentaService/RegistrarseResponse")]
    MessageService.Dominio.Enum.EnumEstadoRegistro Registrarse(LogicaDelNegocio.Modelo.CuentaModel CuentaNueva);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICuentaService/VerificarCuenta", ReplyAction="http://tempuri.org/ICuentaService/VerificarCuentaResponse")]
    MessageService.Dominio.Enum.EnumEstadoVerificarCuenta VerificarCuenta(string CodigoDeVerificacion, LogicaDelNegocio.Modelo.CuentaModel CuentaAVerificar);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICuentaService/ReEnviarCorreoVerificacion", ReplyAction="http://tempuri.org/ICuentaService/ReEnviarCorreoVerificacionResponse")]
    void ReEnviarCorreoVerificacion(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ICuentaServiceChannel : ICuentaService, System.ServiceModel.IClientChannel {
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class CuentaServiceClient : System.ServiceModel.ClientBase<ICuentaService>, ICuentaService {
    
    public CuentaServiceClient() {
    }
    
    public CuentaServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName) {
    }
    
    public CuentaServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress) {
    }
    
    public CuentaServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress) {
    }
    
    public CuentaServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress) {
    }
    
    public MessageService.Dominio.Enum.EnumEstadoRegistro Registrarse(LogicaDelNegocio.Modelo.CuentaModel CuentaNueva) {
        return base.Channel.Registrarse(CuentaNueva);
    }
    
    public MessageService.Dominio.Enum.EnumEstadoVerificarCuenta VerificarCuenta(string CodigoDeVerificacion, LogicaDelNegocio.Modelo.CuentaModel CuentaAVerificar) {
        return base.Channel.VerificarCuenta(CodigoDeVerificacion, CuentaAVerificar);
    }
    
    public void ReEnviarCorreoVerificacion(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        base.Channel.ReEnviarCorreoVerificacion(Cuenta);
    }
}
namespace MessageService.Dominio.Enum {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnumEstadoRegistro", Namespace="http://schemas.datacontract.org/2004/07/MessageService.Dominio.Enum")]
    public enum EnumEstadoRegistro : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RegistroCorrecto = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UsuarioExistente = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ErrorEnBaseDatos = -1,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnumEstadoVerificarCuenta", Namespace="http://schemas.datacontract.org/2004/07/MessageService.Dominio.Enum")]
    public enum EnumEstadoVerificarCuenta : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        VerificadaCorrectamente = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NoCoincideElCodigo = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ErrorEnBaseDatos = -1,
    }
}

