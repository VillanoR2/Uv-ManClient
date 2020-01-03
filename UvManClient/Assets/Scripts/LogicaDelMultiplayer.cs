using System;
using GameService.Dominio;
using LogicaDelNegocio.Modelo;
using LogicaDelNegocio.Modelo.Enum;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Se encarga de manejar a los elementos que se encuentran en el mapa y a los jugadores online
/// </summary>
public class LogicaDelMultiplayer : MonoBehaviour
{
    private CuentaModel CuentaDelJugadorPrincipal;
    private CharacterManager ManejadorDePersonajes = CharacterManager.ManejadorDePersonajes;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    public Jugador JugadorActual;
    public GameObject PrefabUvCoins;
    private GameObject UvCoins;
    private GameObject Corredor;
    private GameObject Perseguidor1;
    private GameObject Perseguidor2;
    private GameObject Perseguidor3;
    private GameObject Perseguidor4;

    private int CANTIDAD_VIDAS_CORREDOR = 5;
    private int CANTIDAD_VIDAS_PERSEGUIDOR = 3;

    private Dictionary<CuentaModel, GameObject> Jugadores = new Dictionary<CuentaModel, GameObject>();
    private Dictionary<CuentaModel, JugadorEnLinea> ScriptsDeMovimiento = new Dictionary<CuentaModel, JugadorEnLinea>();
    private readonly Vector2 POSICION_CORREDOR = new Vector2(-7f, 3.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR1 = new Vector2(-0.5f, -8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR2 = new Vector2(12.5f, -8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR3 = new Vector2(23.5f, -16.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR4 = new Vector2(-11.5f, 11.5f);

    /// <summary>
    /// Metodo de UNITY que se ejecuta en el primer cuadro de la escena
    /// </summary>
    void Start()
    {
        SuscribirseAEventosDeJuego();
        IniciarNuevoJuego();
    }

    /// <summary>
    /// Prepara a los objetos del juego para iniciar
    /// </summary>
    private void IniciarNuevoJuego()
    {
        ColocarUvCoinsEnMapa();
        InicializarPersonajesEnElMapa();
    }

    /// <summary>
    /// Se suscribe a los eventos del cliente de juego
    /// </summary>
    private void SuscribirseAEventosDeJuego()
    {
        ClienteDelJuego.SeMurioUnJugador += MatarJugador;
        ClienteDelJuego.SeMovioUnJugador += MoverJugador;
        ClienteDelJuego.SeActivoElTiempoParaComer += ActivarTiempoDeComer;
        ClienteDelJuego.TerminoLaPartida += TerminarPartida;
    }

    /// <summary>
    /// Recupera la cuenta en la lista de Jugadores en sesion que tenga el mismo nombre de usuario
    /// </summary>
    /// <param name="NombreUsuario">String</param>
    /// <returns>La cuenta del nombre de usuario</returns>
    private CuentaModel ObtenerCuentaDeNombreDeUsuario(string NombreUsuario)
    {
        CuentaModel CuentaAMover = null;
        foreach (CuentaModel cuentaEnElDiccionario in Jugadores.Keys)
        {
            if (cuentaEnElDiccionario.NombreUsuario == NombreUsuario)
            {
                CuentaAMover = cuentaEnElDiccionario;
                break;
            }
        }
        return CuentaAMover;
    }

    /// <summary>
    /// Se encarga de mover un personaje online en el mapa
    /// </summary>
    /// <param name="movimientoJugador">MovimientoJugador</param>
    private void MoverJugador(MovimientoJugador movimientoJugador)
    {
        CuentaModel cuentaAMover = ObtenerCuentaDeNombreDeUsuario(movimientoJugador.Usuario);
        if (cuentaAMover != null)
        {
            ScriptsDeMovimiento[cuentaAMover].RealizarMovimiento(movimientoJugador.PosicionX,
                movimientoJugador.PosicionY, movimientoJugador.MovimientoX, movimientoJugador.MovimentoY);
        }
    }

    /// <summary>
    /// Recupera la CuentaModel que se encuentra en el diccionario de jugadores a partir de una cuenta
    /// </summary>
    /// <param name="cuentaABuscar">CuentaModel</param>
    /// <returns>La cuenta que se encuentra en el diccionario de la cuneta</returns>
    private CuentaModel ObtenerCuentaDelDiccionario(CuentaModel cuentaABuscar)
    {
        CuentaModel cuenta = null;
        foreach (CuentaModel cuentaEnElDiccionario in Jugadores.Keys)
        {
            if (cuentaABuscar.NombreUsuario == cuentaEnElDiccionario.NombreUsuario)
            {
                cuenta = cuentaEnElDiccionario;
                break;
            }
        }
        return cuenta;
    }

    /// <summary>
    /// Envia la posicion de mi personaje en el juego asi como los movimientos que hice
    /// </summary>
    /// <param name="x">float</param>
    /// <param name="y">float</param>
    /// <param name="movimientoX">float</param>
    /// <param name="movimientoY">float</param>
    private void EnviarMiMovimiento(float x, float y, float movimientoX, float movimientoY)
    {
        ClienteDelJuego.EnviarMovimiento(x, y, movimientoX, movimientoY);
    }

    /// <summary>
    /// Instancia un Personaje a partir de las cuentas en sesion asi como los inicializa estableciendo la informacion
    /// de su perssonaje
    /// </summary>
    private void InicializarPersonajesEnElMapa()
    {
        foreach (CuentaModel cuentaEnJuego in ClienteDelJuego.CuentasEnLaSala)
        {
            GameObject PrefabAInstanciar = ManejadorDePersonajes.ObtenerPrefabDePersonaje(cuentaEnJuego.Jugador);
            GameObject InstanciaDelObjecto = null;
            JugadorEnLinea ScriptDeMovimiento = null;
            Vector2 PosicionInicial = new Vector2(0, 0);
            EnumTipoDeJugador TipoDeJugador = EnumTipoDeJugador.Corredor;
            int CantidadDeVidas = CANTIDAD_VIDAS_PERSEGUIDOR;
            bool EsElJugadorActual = EsElJugadorPrincipal(cuentaEnJuego);
            string tag = "Perseguidor";
            switch (cuentaEnJuego.Jugador.RolDelJugador)
            {
                case EnumTipoDeJugador.Corredor:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_CORREDOR, EsElJugadorActual);
                    PosicionInicial = POSICION_CORREDOR;
                    TipoDeJugador = EnumTipoDeJugador.Corredor;
                    CantidadDeVidas = CANTIDAD_VIDAS_CORREDOR;
                    tag = "Corredor";
                    break;
                case EnumTipoDeJugador.Perseguidor1:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR1, EsElJugadorActual);
                    PosicionInicial = POSICION_PERSEGUIDOR1;
                    TipoDeJugador = EnumTipoDeJugador.Perseguidor1;
                    break;
                case EnumTipoDeJugador.Perseguidor2:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR2, EsElJugadorActual);
                    PosicionInicial = POSICION_PERSEGUIDOR2;
                    TipoDeJugador = EnumTipoDeJugador.Perseguidor2;
                    break;
                case EnumTipoDeJugador.Perseguidor3:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR3, EsElJugadorActual);
                    PosicionInicial = POSICION_PERSEGUIDOR3;
                    TipoDeJugador = EnumTipoDeJugador.Perseguidor3;
                    break;
                case EnumTipoDeJugador.Perseguidor4:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR4, EsElJugadorActual);
                    PosicionInicial = POSICION_PERSEGUIDOR4;
                    TipoDeJugador = EnumTipoDeJugador.Perseguidor4;
                    break;
            }
            if (EsElJugadorActual)
            {
                CuentaDelJugadorPrincipal = cuentaEnJuego;
                InstanciaDelObjecto.GetComponent<JugadorEnLinea>().EstaActivoElScript = false;
                JugadorActual = InstanciaDelObjecto.GetComponent<Jugador>();
                JugadorActual.enabled = true;
                InicializarJugador(JugadorActual, true, EsElJugadorActual, PosicionInicial,
                    TipoDeJugador, CantidadDeVidas);
                JugadorActual.EstaActivoTiempoDeMatar = true;
            }
            else
            {
                InstanciaDelObjecto.GetComponent<Jugador>().EstaActivoElScript = false;
                InstanciaDelObjecto.GetComponent<Jugador>().enabled = false;
                ScriptDeMovimiento = InstanciaDelObjecto.GetComponent<JugadorEnLinea>();
                ScriptDeMovimiento.enabled = true;
                InicializarJugadorEnLinea(ScriptDeMovimiento, true, PosicionInicial,
                    TipoDeJugador, CantidadDeVidas);
            }
            InstanciaDelObjecto.tag = tag;
            ScriptsDeMovimiento.Add(cuentaEnJuego, ScriptDeMovimiento);
            Jugadores.Add(cuentaEnJuego, InstanciaDelObjecto);
        }
    }

    /// <summary>
    /// Establece la información necesaria al Script del Personaje del jugador actual para que pueda estar en la partida
    /// </summary>
    /// <param name="MovimientoDelPersonaje">Character_Movement</param>
    /// <param name="EstaActivo">Boolean</param>
    /// <param name="EsElJugadorActual">Boolean</param>
    /// <param name="PosicionInicial">Vector2</param>
    /// <param name="RolDeJugador">EnumTipoDeJugador</param>
    /// <param name="VidasDisponibles">int</param>
    private void InicializarJugador(Jugador MovimientoDelPersonaje, Boolean EstaActivo,
        Boolean EsElJugadorActual, Vector2 PosicionInicial, EnumTipoDeJugador RolDeJugador, int VidasDisponibles)
    {
        MovimientoDelPersonaje.EstaActivoElScript = EstaActivo;
        MovimientoDelPersonaje.RolDelJugador = RolDeJugador;
        MovimientoDelPersonaje.PosicionInicial = PosicionInicial;
        MovimientoDelPersonaje.VidasDisponibles = VidasDisponibles;
        MovimientoDelPersonaje.EsElJuegadorActual = EsElJugadorActual;
        JugadorActual.MeMovi += EnviarMiMovimiento;
        JugadorActual.MateAJugador += NotificarMateAUnJugador;
        JugadorActual.RecolectoTodasLasMonedas += TerminarPartida;
        JugadorActual.PuedoComerPerseguidores += NotificarCorredorPuedeComer;
    }

    /// <summary>
    /// Establece la información necesaria del jugador en linea para que pueda estar en la partida
    /// </summary>
    /// <param name="MovimientoDelPersonaje">ChrarcterMovementOnline</param>
    /// <param name="EstaActivo">Boolean</param>
    /// <param name="PosicionInicial">Vector2</param>
    /// <param name="RolDeJugador">EnumTipoDeJugador</param>
    /// <param name="VidasDisponibles">int</param>
    private void InicializarJugadorEnLinea(JugadorEnLinea MovimientoDelPersonaje, Boolean EstaActivo,
        Vector2 PosicionInicial, EnumTipoDeJugador RolDeJugador, int VidasDisponibles)
    {
        MovimientoDelPersonaje.EstaActivoElScript = EstaActivo;
        MovimientoDelPersonaje.RolDelJugador = RolDeJugador;
        MovimientoDelPersonaje.PosicionInicial = PosicionInicial;
        MovimientoDelPersonaje.VidasDisponibles = VidasDisponibles;
    }

    /// <summary>
    /// Intancia un prefab de las UvCoins en el mapa
    /// </summary>
    private void ColocarUvCoinsEnMapa()
    {
        UvCoins = Instantiate(PrefabUvCoins);
    }
    
    /// <summary>
    /// Verifica si la cuenta es el jugador actual
    /// </summary>
    /// <param name="cuenta">CuentaModel</param>
    /// <returns>Verdadero si es el jugador actual, falso si no</returns>
    private bool EsElJugadorPrincipal(CuentaModel cuenta)
    {
        return ClienteDelJuego.CuentaEnSesion.NombreUsuario == cuenta.NombreUsuario;
    }
   
    /// <summary>
    /// Instancia un personake en el mapa
    /// </summary>
    /// <param name="PrefabAInstanciar">GameObject</param>
    /// <param name="Posicion">Vector2</param>
    /// <param name="EsELJugadorActual">bool</param>
    /// <returns>La instancia del prefab</returns>
    private GameObject ColocarPersonajeEnElMapa(GameObject PrefabAInstanciar, Vector2 Posicion, bool EsELJugadorActual)
    {
        if (EsELJugadorActual)
        {
            PrefabAInstanciar.GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
            PrefabAInstanciar.GetComponent<Jugador>().enabled = true;
            PrefabAInstanciar.GetComponent<JugadorEnLinea>().enabled = false;
        }
        else
        {
            PrefabAInstanciar.GetComponentInChildren<Camera>(true).gameObject.SetActive(false);
            PrefabAInstanciar.GetComponent<Jugador>().enabled = false;
            PrefabAInstanciar.GetComponent<JugadorEnLinea>().enabled = true;
        }
        PrefabAInstanciar.SetActive(true);
        return Instantiate(PrefabAInstanciar, Posicion, transform.rotation);
    }

    /// <summary>
    /// Mata a un jugador en la partida descontandole una vida
    /// </summary>
    /// <param name="DatosDeLaMuerteDeJugador">Datos de la muerte del jugador al que se le descontara la vida</param>
    private void MatarJugador(MuerteJugador DatosDeLaMuerteDeJugador)
    {
        CuentaModel CuentaADescontarVidas = ObtenerCuentaDeNombreDeUsuario(DatosDeLaMuerteDeJugador.Usuario);
        if (CuentaADescontarVidas != null)
        {
            DescontarVida(CuentaADescontarVidas, DatosDeLaMuerteDeJugador.CantidadDeVidas);
            VerificarSiLosPerseguidoresAunTienenVidas();
            VerificarSiAunTengoVidas();
        }
    }

    /// <summary>
    /// Verifica que los personajes con el rol de perseguidor aun tengan vidas
    /// </summary>
    /// <returns>Verdadero si aun tienen, falso si no</returns>
    private bool LosPerseguidoresAunTienenVidas()
    {
        bool CuentanConVidas = false;
        foreach (JugadorEnLinea movimientoDelJugador in ScriptsDeMovimiento.Values)
        {
            if (movimientoDelJugador != null)
            {
                if ((movimientoDelJugador.RolDelJugador > 0) && (movimientoDelJugador.VidasDisponibles > 0))
                {
                    CuentanConVidas = true;
                    break;
                }
            }
        }
        if ((JugadorActual.RolDelJugador > 0) && (JugadorActual.VidasDisponibles > 0))
        {
            CuentanConVidas = true;
        }
        return CuentanConVidas;
    }

    /// <summary>
    /// Descuenta una vida al personaje de la cuenta
    /// </summary>
    /// <param name="CuentaADescontarVidas">Cuenta del personaje a la que se le descontara una vida</param>
    private void DescontarVida(CuentaModel CuentaADescontarVidas, int CantidadDeVidas)
    {
        if (CuentaADescontarVidas.Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            ColocarPersonajesEnPosicionInicial();
        }
        if (EsElJugadorPrincipal(CuentaADescontarVidas))
        {
            JugadorActual.DescontarVida(CantidadDeVidas);
        }
        else
        {
            ScriptsDeMovimiento[CuentaADescontarVidas].DescontarVida(CantidadDeVidas);
        }
    }

    /// <summary>
    /// Verifica si todos los perseguidores tienen alguna vida, de no ser asi termina el juego
    /// </summary>
    private void VerificarSiLosPerseguidoresAunTienenVidas()
    {
        if (!LosPerseguidoresAunTienenVidas())
        {
            TerminarPartida();
        }
    }

    /// <summary>
    /// Cambia de escena a mostrar mejores puntuaciones y 
    /// si el jugador actual es el corredor manda el mensaje de se termino la partida
    /// </summary>
    private void TerminarPartida()
    {
        ClienteDelJuego.Puntuacion = JugadorActual.PuntacionTotal;
        DesuscribirseAEventosDeJuego();
        ClienteDelJuego.TerminarPartida(JugadorActual);
        }

    /// <summary>
    /// Verifica si el jugador actual tiene vidas, de no ser asi desactiva al jugador actual y si el jugador actual es el
    /// corredo termina la partida
    /// </summary>
    private void VerificarSiAunTengoVidas()
    {
        if (JugadorActual.VidasDisponibles == 0)
        {
            DesactivarJugadorActual();
            if (JugadorActual.RolDelJugador == 0)
            {
                TerminarPartida();
            }
        }
    }

    /// <summary>
    /// Quita la camara individual y desactiva al Jugador actual
    /// </summary>
    private void DesactivarJugadorActual()
    {
        JugadorActual.ColocarseEnLaPosicionInicial();
        JugadorActual.DesactivarCamara();
        Jugadores[CuentaDelJugadorPrincipal].SetActive(false);
    }
    
    /// <summary>
    /// Notifica al cliente del juego que el jugador mato a un jugador en linea
    /// </summary>
    /// <param name="RolDelJugadorMatado">El Rol del jugador matado</param>
    private void NotificarMateAUnJugador(EnumTipoDeJugador RolDelJugadorMatado, int NumeroVidas)
    {
        CuentaModel CuentaMuerto = BuscarCuentaPorRol(RolDelJugadorMatado);
        if (CuentaMuerto != null)
        {
            ClienteDelJuego.NotificarMuerteJugador(CuentaMuerto.NombreUsuario, NumeroVidas);
        }
        if(CuentaMuerto.Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            ColocarPersonajesEnPosicionInicial();
        }
    }

    /// <summary>
    /// Busca la cuenta que tenga el rol en la lista de cuentas en la sa
    /// </summary>
    /// <param name="RolDelJugador">El rol del jugador matado</param>
    /// <returns>La cuenta que tiene ese rol en la sala</returns>
    private CuentaModel BuscarCuentaPorRol(EnumTipoDeJugador RolDelJugador)
    {
        CuentaModel CuentaDelRol = null;
        foreach (CuentaModel CuentaEnLaSala in ClienteDelJuego.CuentasEnLaSala)
        {
            if (CuentaEnLaSala.Jugador.RolDelJugador == RolDelJugador)
            {
                CuentaDelRol = CuentaEnLaSala;
            }
        }
        return CuentaDelRol;
    }
    
    /// <summary>
    /// Coloca a todos los jugadores en su posicion inicial en el mapa
    /// </summary>
    private void ColocarPersonajesEnPosicionInicial()
    {
        foreach(JugadorEnLinea ScriptDelPersonaje in ScriptsDeMovimiento.Values)
        {
            if (ScriptDelPersonaje != null)
            {
                ScriptDelPersonaje.ColocarseEnLaPosicionInicial();
            }
        }
        JugadorActual.ColocarseEnLaPosicionInicial();
    }

    /// <summary>
    /// Notifica al Cliente del juego que el corredor puede comer jugadores para que envie el mesanje a los demas jugadores
    /// </summary>
    private void NotificarCorredorPuedeComer()
    {
        ClienteDelJuego.NotificarCorredorPuedeComerJugadores();
    }

    /// <summary>
    /// Se activa el tiempo de comer en el jugador actual y en el corredor
    /// </summary>
    private void ActivarTiempoDeComer()
    {
        JugadorActual.ActivaTiempoDeMatar();
        JugadorEnLinea ScriptDelCorredor = RecuperarScriptDePersonaje(EnumTipoDeJugador.Corredor);
        if (ScriptDelCorredor != null)
        {
            ScriptDelCorredor.ActivaTiempoDeMatar();    
        }
    }
    
    /// <summary>
    /// Recupera el script de movimiento del personaje que sea del tipo de personaje
    /// </summary>
    /// <param name="tipoDelPersonaje">EnumTipoDeJugador</param>
    /// <returns>El ChracterMovenent que tiene ese rol</returns>
    private JugadorEnLinea RecuperarScriptDePersonaje(EnumTipoDeJugador tipoDelPersonaje)
    {
        JugadorEnLinea ScriptDelPersonaje = null;
        foreach (JugadorEnLinea ScriptDeMovimiento in ScriptsDeMovimiento.Values)
        {
            if (ScriptDeMovimiento != null)
            {
                if (ScriptDeMovimiento.RolDelJugador == tipoDelPersonaje)
                {
                    ScriptDelPersonaje = ScriptDeMovimiento;
                }
            }
        }
        return ScriptDelPersonaje;
    }
    
    /// <summary>
    /// Se desuscribe a los servicios de eventos en juegos
    /// </summary>
    private void DesuscribirseAEventosDeJuego()
    {
        ClienteDelJuego.SeMurioUnJugador -= MatarJugador;
        ClienteDelJuego.SeMovioUnJugador -= MoverJugador;
        ClienteDelJuego.SeActivoElTiempoParaComer -= ActivarTiempoDeComer;
        ClienteDelJuego.TerminoLaPartida -= TerminarPartida;
    }
}


