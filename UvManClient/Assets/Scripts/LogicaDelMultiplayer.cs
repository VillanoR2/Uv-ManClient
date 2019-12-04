using GameService.Dominio;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaDelMultiplayer : MonoBehaviour
{
    private CharacterManager ManejadorDePersonajes = CharacterManager.ManejadorDePersonajes;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    public GameObject Corredor;
    public GameObject Perseguidor1;
    public GameObject Perseguidor2;
    public GameObject Perseguidor3;
    public GameObject Perseguidor4;

    private Dictionary<CuentaModel, GameObject> Jugadores = new Dictionary<CuentaModel, GameObject>();
    private Dictionary<CuentaModel, CharacterMovementOnline> ScriptsDeMovimiento = new Dictionary<CuentaModel, CharacterMovementOnline>();
    private readonly Vector2 POSICION_CORREDOR = new Vector2(-7f,3.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR1 = new Vector2(-0.5f,-8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR2 = new Vector2(12.5f,-8.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR3 = new Vector2(23.5f,-16.5f);
    private readonly Vector2 POSICION_PERSEGUIDOR4 = new Vector2(-11.5f,11.5f);

    void Start()
    {   
        InicializarPersonajesEnElMapa();
        SuscribirseAEventosDeJuego();
    }

    

    private void SuscribirseAEventosDeJuego()
    {
        //ClienteDelJuego.IniciaLaPartida;
        ClienteDelJuego.SeMovioUnJugador += MoverJugador;
    }

    private CuentaModel CuentaDelMovimiento(string cuenta)
    {
        CuentaModel CuentaAMover = null;
        foreach(CuentaModel cuentaEnElDiccionario in Jugadores.Keys)
        {
            if(cuentaEnElDiccionario.NombreUsuario == cuenta)
            {
                CuentaAMover = cuentaEnElDiccionario;
                break;
            }
        }
        return CuentaAMover;
    }

    private void MoverJugador(MovimientoJugador movimientoJugador)
    {
        CuentaModel cuentaAMover = CuentaDelMovimiento(movimientoJugador.Usuario);
        if(cuentaAMover != null)
        {
            ScriptsDeMovimiento[cuentaAMover].RealizarMovimiento(movimientoJugador.PosicionX,
                movimientoJugador.PosicionY, movimientoJugador.MovimientoX, movimientoJugador.MovimentoY);
        }
    }

    private CuentaModel ObtenerCuentaDelDiccionario(CuentaModel cuentaABuscar)
    {
        CuentaModel cuenta = null;
        foreach(CuentaModel cuentaEnElDiccionario in Jugadores.Keys)
        {
            if(cuentaABuscar.NombreUsuario == cuentaEnElDiccionario.NombreUsuario)
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
        foreach(CuentaModel cuentaEnJuego in ClienteDelJuego.CuentasEnSesion)
        {
            GameObject PrefabAInstanciar = ManejadorDePersonajes.ObtenerPrefabDePersonaje(cuentaEnJuego.Jugador);
            bool esElJugadorPrincipal = EsElJugadorPrincipal(cuentaEnJuego);
            GameObject InstanciaDelObjecto = null;
            CharacterMovementOnline ScriptDeMovimiento = null;
            switch (cuentaEnJuego.Jugador.RolDelJugador)
            {
                case EnumTipoDeJugador.Corredor:    
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_CORREDOR, esElJugadorPrincipal);
                    break;
                case EnumTipoDeJugador.Perseguidor1:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR1, esElJugadorPrincipal);
                    break;
                case EnumTipoDeJugador.Perseguidor2:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR2, esElJugadorPrincipal);
                    break;
                case EnumTipoDeJugador.Perseguidor3:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR3, esElJugadorPrincipal);
                    break;
                case EnumTipoDeJugador.Perseguidor4:
                    InstanciaDelObjecto = ColocarPersonajeEnElMapa(PrefabAInstanciar, POSICION_PERSEGUIDOR4, esElJugadorPrincipal);
                    break;
            }
            if (esElJugadorPrincipal)
            {
                InstanciaDelObjecto.GetComponent<Character_Movement>().MeMovi += EnviarCoordenadas;
            }
            else
            {
                ScriptDeMovimiento = InstanciaDelObjecto.GetComponent<CharacterMovementOnline>();
            }
            ScriptsDeMovimiento.Add(cuentaEnJuego, ScriptDeMovimiento);
            Jugadores.Add(cuentaEnJuego, InstanciaDelObjecto);
        }
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
}
