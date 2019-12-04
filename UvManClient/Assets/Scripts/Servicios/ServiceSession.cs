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
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="ISessionService", CallbackContract=typeof(ISessionServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
public interface ISessionService {
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionService/IniciarSesion", ReplyAction="http://tempuri.org/ISessionService/IniciarSesionResponse")]
    SessionService.Dominio.Enum.EnumEstadoInicioSesion IniciarSesion(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
    
    [System.ServiceModel.OperationContractAttribute(IsTerminating=true, Action="http://tempuri.org/ISessionService/CerrarSesion", ReplyAction="http://tempuri.org/ISessionService/CerrarSesionResponse")]
    void CerrarSesion(LogicaDelNegocio.Modelo.CuentaModel Cuenta);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ISessionServiceCallback {
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionService/EstaVivo", ReplyAction="http://tempuri.org/ISessionService/EstaVivoResponse")]
    bool EstaVivo();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ISessionServiceChannel : ISessionService, System.ServiceModel.IClientChannel {
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class SessionServiceClient : System.ServiceModel.DuplexClientBase<ISessionService>, ISessionService {
    
    public SessionServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
            base(callbackInstance) {
    }
    
    public SessionServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
            base(callbackInstance, endpointConfigurationName) {
    }
    
    public SessionServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress) {
    }
    
    public SessionServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, endpointConfigurationName, remoteAddress) {
    }
    
    public SessionServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(callbackInstance, binding, remoteAddress) {
    }
    
    public SessionService.Dominio.Enum.EnumEstadoInicioSesion IniciarSesion(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        return base.Channel.IniciarSesion(Cuenta);
    }
    
    public void CerrarSesion(LogicaDelNegocio.Modelo.CuentaModel Cuenta) {
        base.Channel.CerrarSesion(Cuenta);
    }
}
namespace LogicaDelNegocio.Modelo {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CuentaModel", Namespace="http://schemas.datacontract.org/2004/07/LogicaDelNegocio.Modelo")]
    public partial class CuentaModel : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string CodigoVerificacionField;
        
        private string ContrasenaField;
        
        private string CorreoElectronicoField;
        
        private LogicaDelNegocio.Modelo.JugadorModel JugadorField;
        
        private string NombreUsuarioField;
        
        private bool VerificadoField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodigoVerificacion {
            get {
                return this.CodigoVerificacionField;
            }
            set {
                this.CodigoVerificacionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Contrasena {
            get {
                return this.ContrasenaField;
            }
            set {
                this.ContrasenaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CorreoElectronico {
            get {
                return this.CorreoElectronicoField;
            }
            set {
                this.CorreoElectronicoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogicaDelNegocio.Modelo.JugadorModel Jugador {
            get {
                return this.JugadorField;
            }
            set {
                this.JugadorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NombreUsuario {
            get {
                return this.NombreUsuarioField;
            }
            set {
                this.NombreUsuarioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Verificado {
            get {
                return this.VerificadoField;
            }
            set {
                this.VerificadoField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JugadorModel", Namespace="http://schemas.datacontract.org/2004/07/LogicaDelNegocio.Modelo")]
    public partial class JugadorModel : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private LogicaDelNegocio.Modelo.CorredorAdquiridoModel CorredorSeleccionadoField;
        
        private LogicaDelNegocio.Modelo.CorredorAdquiridoModel[] CorredoresAdquiridosField;
        
        private int MejorPuntacionField;
        
        private AccesoDatos.PerseguidorAdquirido PerseguidorSeleccionadoField;
        
        private GameService.Dominio.Enum.EnumTipoDeJugador RolDelJugadorField;
        
        private LogicaDelNegocio.Modelo.SeguidorAdquiridoModel[] SeguidoresAdquiridosField;
        
        private int UvCoinsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogicaDelNegocio.Modelo.CorredorAdquiridoModel CorredorSeleccionado {
            get {
                return this.CorredorSeleccionadoField;
            }
            set {
                this.CorredorSeleccionadoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogicaDelNegocio.Modelo.CorredorAdquiridoModel[] CorredoresAdquiridos {
            get {
                return this.CorredoresAdquiridosField;
            }
            set {
                this.CorredoresAdquiridosField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MejorPuntacion {
            get {
                return this.MejorPuntacionField;
            }
            set {
                this.MejorPuntacionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.PerseguidorAdquirido PerseguidorSeleccionado {
            get {
                return this.PerseguidorSeleccionadoField;
            }
            set {
                this.PerseguidorSeleccionadoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public GameService.Dominio.Enum.EnumTipoDeJugador RolDelJugador {
            get {
                return this.RolDelJugadorField;
            }
            set {
                this.RolDelJugadorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogicaDelNegocio.Modelo.SeguidorAdquiridoModel[] SeguidoresAdquiridos {
            get {
                return this.SeguidoresAdquiridosField;
            }
            set {
                this.SeguidoresAdquiridosField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UvCoins {
            get {
                return this.UvCoinsField;
            }
            set {
                this.UvCoinsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CorredorAdquiridoModel", Namespace="http://schemas.datacontract.org/2004/07/LogicaDelNegocio.Modelo")]
    public partial class CorredorAdquiridoModel : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string NombreField;
        
        private string PoderField;
        
        private int PrecioField;
        
        private double VelocidadField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                this.NombreField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Poder {
            get {
                return this.PoderField;
            }
            set {
                this.PoderField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Precio {
            get {
                return this.PrecioField;
            }
            set {
                this.PrecioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double Velocidad {
            get {
                return this.VelocidadField;
            }
            set {
                this.VelocidadField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SeguidorAdquiridoModel", Namespace="http://schemas.datacontract.org/2004/07/LogicaDelNegocio.Modelo")]
    public partial class SeguidorAdquiridoModel : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string NombreField;
        
        private int PrecioField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                this.NombreField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Precio {
            get {
                return this.PrecioField;
            }
            set {
                this.PrecioField = value;
            }
        }
    }
}
namespace AccesoDatos {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PerseguidorAdquirido", Namespace="http://schemas.datacontract.org/2004/07/AccesoDatos")]
    public partial class PerseguidorAdquirido : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int IdField;
        
        private AccesoDatos.Jugador JugadorField;
        
        private int JugadorIdField;
        
        private string NombreField;
        
        private int PrecioField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.Jugador Jugador {
            get {
                return this.JugadorField;
            }
            set {
                this.JugadorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int JugadorId {
            get {
                return this.JugadorIdField;
            }
            set {
                this.JugadorIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                this.NombreField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Precio {
            get {
                return this.PrecioField;
            }
            set {
                this.PrecioField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Jugador", Namespace="http://schemas.datacontract.org/2004/07/AccesoDatos")]
    public partial class Jugador : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private AccesoDatos.CorredorAdquirido[] CorredoresAdquiridosField;
        
        private AccesoDatos.Cuenta CuentaField;
        
        private int IdField;
        
        private int MejorPuntacionField;
        
        private AccesoDatos.PerseguidorAdquirido[] PerseguidorAdquiridoField;
        
        private int UvCoinsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.CorredorAdquirido[] CorredoresAdquiridos {
            get {
                return this.CorredoresAdquiridosField;
            }
            set {
                this.CorredoresAdquiridosField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.Cuenta Cuenta {
            get {
                return this.CuentaField;
            }
            set {
                this.CuentaField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MejorPuntacion {
            get {
                return this.MejorPuntacionField;
            }
            set {
                this.MejorPuntacionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.PerseguidorAdquirido[] PerseguidorAdquirido {
            get {
                return this.PerseguidorAdquiridoField;
            }
            set {
                this.PerseguidorAdquiridoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UvCoins {
            get {
                return this.UvCoinsField;
            }
            set {
                this.UvCoinsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Cuenta", Namespace="http://schemas.datacontract.org/2004/07/AccesoDatos")]
    public partial class Cuenta : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string CodigoVerificacionField;
        
        private string CorreoElectronicoField;
        
        private string PasswordField;
        
        private string UsuarioField;
        
        private AccesoDatos.Jugador Usuario1Field;
        
        private bool ValidaField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CodigoVerificacion {
            get {
                return this.CodigoVerificacionField;
            }
            set {
                this.CodigoVerificacionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CorreoElectronico {
            get {
                return this.CorreoElectronicoField;
            }
            set {
                this.CorreoElectronicoField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                this.PasswordField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Usuario {
            get {
                return this.UsuarioField;
            }
            set {
                this.UsuarioField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.Jugador Usuario1 {
            get {
                return this.Usuario1Field;
            }
            set {
                this.Usuario1Field = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Valida {
            get {
                return this.ValidaField;
            }
            set {
                this.ValidaField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CorredorAdquirido", Namespace="http://schemas.datacontract.org/2004/07/AccesoDatos")]
    public partial class CorredorAdquirido : object, System.Runtime.Serialization.IExtensibleDataObject {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int IdField;
        
        private AccesoDatos.Jugador JugadorField;
        
        private int JugadorIdField;
        
        private string NombreField;
        
        private string PoderField;
        
        private int PrecioField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AccesoDatos.Jugador Jugador {
            get {
                return this.JugadorField;
            }
            set {
                this.JugadorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int JugadorId {
            get {
                return this.JugadorIdField;
            }
            set {
                this.JugadorIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                this.NombreField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Poder {
            get {
                return this.PoderField;
            }
            set {
                this.PoderField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Precio {
            get {
                return this.PrecioField;
            }
            set {
                this.PrecioField = value;
            }
        }
    }
}
namespace GameService.Dominio.Enum {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnumTipoDeJugador", Namespace="http://schemas.datacontract.org/2004/07/GameService.Dominio.Enum")]
    public enum EnumTipoDeJugador : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Corredor = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Perseguidor1 = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Perseguidor2 = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Perseguidor3 = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Perseguidor4 = 4,
    }
}
namespace SessionService.Dominio.Enum {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EnumEstadoInicioSesion", Namespace="http://schemas.datacontract.org/2004/07/SessionService.Dominio.Enum")]
    public enum EnumEstadoInicioSesion : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Correcto = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CredencialesInvalidas = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CuentaNoVerificada = -1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ErrorBD = -2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CuentaYaLogeada = -3,
    }
}

