using GameService.Dominio;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using LogicaDelNegocio.Modelo.Enum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaDelMultiplayer : MonoBehaviour
{
    private CharacterManager ManejadorDePersonajes = CharacterManager.ManejadorDePersonajes;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    public Character_Movement JugadorActual;
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
    private Dictionary<CuentaModel, CharacterMovementOnline> ScriptsDeMovimiento = new Dictionary<CuentaModel, CharacterMovementOnline>();
    private readonly Vector2 POSICION_CORREDOR = new Vector2(-7f, 3.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR1 = new Vector2(-0.5f, -8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR2 = new Vector2(12.5f, -8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR3 = new Vector2(23.5f, -16.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR4 = new Vector2(-11.5f, 11.5f);


    void Start()
    {
        ColocarUvCoinsEnMapa();
        InicializarPersonajesEnElMapa();
        SuscribirseAEventosDeJuego();
    }

    private void SuscribirseAEventosDeJuego()
    {
        ClienteDelJuego.SeMurioUnJugador += MatarJugador;
        ClienteDelJuego.SeMovioUnJugador += MoverJugador;
        ClienteDelJuego.PrepararNuevoNivel += ReiniciarNivel;
    }

    private CuentaModel ObtenerCuentaDeNombreDeUsuario(string cuenta)
    {
        CuentaModel CuentaAMover = null;
        foreach (CuentaModel cuentaEnElDiccionario in Jugadores.Keys)
        {
            if (cuentaEnElDiccionario.NombreUsuario == cuenta)
            {
                CuentaAMover = cuentaEnElDiccionario;
                break;
            }
        }
        return CuentaAMover;
    }

    private void MoverJugador(MovimientoJugador movimientoJugador)
    {
        CuentaModel cuentaAMover = ObtenerCuentaDeNombreDeUsuario(movimientoJugador.Usuario);
        if (cuentaAMover != null)
        {
            ScriptsDeMovimiento[cuentaAMover].RealizarMovimiento(movimientoJugador.PosicionX,
                movimientoJugador.PosicionY, movimientoJugador.MovimientoX, movimientoJugador.MovimentoY);
        }
    }

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

    private void EnviarCoordenadas(float x, float y, float movimientoX, float movimientoY)
    {
        ClienteDelJuego.EnviarMovimiento(x, y, movimientoX, movimientoY);
    }

    private void InicializarPersonajesEnElMapa()
    {
        foreach (CuentaModel cuentaEnJuego in ClienteDelJuego.CuentasEnLaSala)
        {
            GameObject PrefabAInstanciar = ManejadorDePersonajes.ObtenerPrefabDePersonaje(cuentaEnJuego.Jugador);
            GameObject InstanciaDelObjecto = null;
            CharacterMovementOnline ScriptDeMovimiento = null;
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
                InstanciaDelObjecto.GetComponent<CharacterMovementOnline>().EstaActivoElScript = false;
                InstanciaDelObjecto.GetComponent<CharacterMovementOnline>().enabled = false;
                JugadorActual = InstanciaDelObjecto.GetComponent<Character_Movement>();
                JugadorActual.enabled = true;
                JugadorActual.EstaActivoElScript = true;
                JugadorActual.RolDelJugador = TipoDeJugador;
                JugadorActual.PosicionInicial = PosicionInicial;
                JugadorActual.VidasDisponibles = CantidadDeVidas;
                JugadorActual.EsElJuegadorActual = EsElJugadorActual;
                JugadorActual.EstaActivoTiempoDeMatar = true;
                JugadorActual.MeMovi += EnviarCoordenadas;
                JugadorActual.MateAJugador += NotificarMateAUnJugador;
                JugadorActual.RecolectoTodasLasMonedas += IniciarNuevoNivel;
            }
            else
            {
                InstanciaDelObjecto.GetComponent<Character_Movement>().EstaActivoElScript = false;
                InstanciaDelObjecto.GetComponent<Character_Movement>().enabled = false;
                ScriptDeMovimiento = InstanciaDelObjecto.GetComponent<CharacterMovementOnline>();
                ScriptDeMovimiento.enabled = true;
                ScriptDeMovimiento.EstaActivoElScript = true;
                ScriptDeMovimiento.PosicionInicial = PosicionInicial;
                ScriptDeMovimiento.RolDelJugador = TipoDeJugador;
                ScriptDeMovimiento.VidasDisponibles = CantidadDeVidas;
            }
            InstanciaDelObjecto.tag = tag;
            ScriptsDeMovimiento.Add(cuentaEnJuego, ScriptDeMovimiento);
            Jugadores.Add(cuentaEnJuego, InstanciaDelObjecto);
        }
    }

    private void ColocarUvCoinsEnMapa()
    {
        UvCoins = Instantiate(PrefabUvCoins);
    }

    private void IniciarNuevoNivel()
    {
        ClienteDelJuego.NotificarInicioPartida();
    }

    private bool EsElJugadorPrincipal(CuentaModel cuenta)
    {
        return ClienteDelJuego.CuentaEnSesion.NombreUsuario == cuenta.NombreUsuario;
    }

    private GameObject ColocarPersonajeEnElMapa(GameObject PrefabAInstanciar, Vector2 Posicion, bool EsELJugadorActual)
    {
        if (EsELJugadorActual)
        {
            PrefabAInstanciar.GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
            PrefabAInstanciar.GetComponent<Character_Movement>().enabled = true;
            PrefabAInstanciar.GetComponent<CharacterMovementOnline>().enabled = false;
        }
        else
        {
            PrefabAInstanciar.GetComponentInChildren<Camera>(true).gameObject.SetActive(false);
            PrefabAInstanciar.GetComponent<Character_Movement>().enabled = false;
            PrefabAInstanciar.GetComponent<CharacterMovementOnline>().enabled = true;
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
        foreach (CharacterMovementOnline movimientoDelJugador in ScriptsDeMovimiento.Values)
        {
            if(movimientoDelJugador != null)
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
            //TerminarPartida();
        }
    }

    /// <summary>
    /// Cambia de escena a mostrar mejores puntuaciones y 
    /// si el jugador actual es el corredor manda el mensaje de se termino la partida
    /// </summary>
    private void TerminarPartida()
    {
        if (JugadorActual.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            ClienteDelJuego.TerminarPartida(JugadorActual);
        }
        SceneManager.LoadScene("MenuScreen");
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
    /// Oculta del juego el Personaje al que pertenece la cuenta
    /// </summary>
    /// <param name="CuentaADesactivar"></param>
    //private void DesactivarJugadorEnLinea(CuentaModel CuentaADesactivar)
    //{
    //    CuentaModel CuentaEnDiciconario = ObtenerCuentaDelDiccionario(CuentaADesactivar);
    //    if (CuentaADesactivar != null)
    //    {
    //        Jugadores[CuentaEnDiciconario].SetActive(false);
    //    }
    //}

    /// <summary>
    /// Muestra en el juego al Personaje al que pertenece la cuenta
    /// </summary>
    /// <param name="CuentaActivar"></param>
    private void ActivarJugadorEnLinea(CuentaModel CuentaActivar)
    {
        CuentaModel CuentaEnDiciconario = ObtenerCuentaDelDiccionario(CuentaActivar);
        if (CuentaActivar != null)
        {
            Jugadores[CuentaEnDiciconario].SetActive(true);
        }
    }

    /// <summary>
    /// Quita la camara individual y desactiva al Jugador actual
    /// </summary>
    private void DesactivarJugadorActual()
    {
        GameObject PersonajeActual = JugadorActual.gameObject;
        PersonajeActual.GetComponentInChildren<Camera>().enabled = false;
        PersonajeActual.SetActive(false);
    }

    /// <summary>
    /// Elimina la instancia del GameObject al que pertenezca la cuenta
    /// </summary>
    /// <param name="JugadorADestruir">Cuenta del GameObject a destruir</param>
    private void DestruirJugador(CuentaModel JugadorADestruir)
    {
        CuentaModel CuentaADestruir = ObtenerCuentaDelDiccionario(JugadorADestruir);
        if (CuentaADestruir != null)
        {
            if (ScriptsDeMovimiento[CuentaADestruir].RolDelJugador == EnumTipoDeJugador.Corredor)
            {
                TerminarPartida();
            }
            else
            {
                Destroy(Jugadores[CuentaADestruir]);
                Jugadores.Remove(CuentaADestruir);
            }
        }
    }

    /// <summary>
    /// Coloca al GameObject que pertenece del jugador actual en la posicion Inicial y establece la camara individual
    /// </summary>
    private void ActivarJugadorActual()
    {
        GameObject PersonajeActual = JugadorActual.gameObject;
        JugadorActual.ColocarseEnLaPosicionInicial();
        PersonajeActual.GetComponentInChildren<Camera>().enabled = true;
        PersonajeActual.SetActive(true);
    }

    private void ReiniciarNivel()
    {
        ReiniciarUvCoins();
        ReiniciarVidasPerseguidores();
        ReiniciarJugadoresEnLinea();
        ActivarJugadorActual();
    }


    /// <summary>
    /// Reinicia la vida de los personajes que su rol sea Persuidor y
    /// coloca a todos los jugadores en la posicion inicial
    /// </summary>
    private void ReiniciarVidasPerseguidores()
    {
        foreach (CharacterMovementOnline Personaje in ScriptsDeMovimiento.Values)
        {
            Personaje.ColocarseEnLaPosicionInicial();
            if (Personaje.RolDelJugador > 0)
            {
                Personaje.VidasDisponibles = CANTIDAD_VIDAS_PERSEGUIDOR;
            }
        }
    }

    /// <summary>
    /// Coloca en el mapa a los jugadores en linea
    /// </summary>
    private void ReiniciarJugadoresEnLinea()
    {
        foreach(CuentaModel cuenta in Jugadores.Keys)
        {
            ActivarJugadorEnLinea(cuenta);
        }
    }

    private void ReiniciarUvCoins()
    {
        Destroy(UvCoins);
        UvCoins = Instantiate(PrefabUvCoins);
    }

    /// <summary>
    /// Notifica al cliente del juego que mate a un jugador
    /// </summary>
    /// <param name="RolDelJugadorMatado">El Rol del jugador matado</param>
    private void NotificarMateAUnJugador(EnumTipoDeJugador RolDelJugadorMatado, int NumeroVidas)
    {
        CuentaModel CuentaMuerto = BuscarCuentaPorRol(RolDelJugadorMatado);
        if(CuentaMuerto != null)
        {
            ClienteDelJuego.NotificarMuerteJugador(CuentaMuerto.NombreUsuario, NumeroVidas);
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
        foreach(CuentaModel CuentaEnLaSala in ClienteDelJuego.CuentasEnLaSala)
        {
            if(CuentaEnLaSala.Jugador.RolDelJugador == RolDelJugador)
            {
                CuentaDelRol = CuentaEnLaSala;
            }
        }
        return CuentaDelRol;
    }
}


