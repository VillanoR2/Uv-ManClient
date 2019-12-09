using Assets.Scripts.Util;
using GameService.Dominio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ManejoDeCanvas : MonoBehaviour
{
    private Canvas cInicioPartida;
    private bool EstaElJuegoPausado;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private Cronometro CronometroInicioDePartida;
    private Text txtCuentaRegresiva;
    private int SegundosParaInicioDePartida;
    private string TextoEnCanvas;

    void InicializarCanvasInicioPartida()
    {
        EstaElJuegoPausado = true;
        cInicioPartida = GetComponent<Canvas>();
        cInicioPartida.enabled = true;
        SegundosParaInicioDePartida = 10;
        Time.timeScale = 0;
        //RealizarConteo();
    }

    private void SuscribirseAEventosDelJuego()
    {
        ClienteDelJuego.IniciaLaPartida += RealizarAccionDeInicioDeJuego;
        ClienteDelJuego.PrepararNuevoNivel += RealizarAccionDeInicioDeNivel;
        ClienteDelJuego.NuevoNivel += RealizarAccionInicioNivel;
    }

    private void RealizarAccionDeInicioDeNivel()
    {
        SegundosParaInicioDePartida = 5;
        EstaElJuegoPausado = true;
    }

    private void RealizarAccionInicioNivel(InicioPartida DatosDeInicioDePartida)
    {
        if (DatosDeInicioDePartida.IniciarNivel)
        {
            DesactivarPausa();
        }
        else if(DatosDeInicioDePartida.IniciarCuentaRegresivaInicioNivel)
        {
            RealizarConteo();
        }
    }

    private void RealizarAccionDeInicioDeJuego(InicioPartida iniciaPartida)
    {
        if (iniciaPartida.IniciarPartida)
        {
            if (EstaElJuegoPausado)
            {
                DesactivarPausa();
            }
        }else if (iniciaPartida.IniciarCuentaRegresivaInicioPartida)
        {
            RealizarConteo();
        }
    }

    private void DesactivarPausa()
    {
        EstaElJuegoPausado = false;
    }

    private void RealizarConteo()
    {
        CronometroInicioDePartida = new Cronometro(1000, 5000);
        CronometroInicioDePartida.TranscurrioUnIntervalo += ActualizarSegundosContador;
        CronometroInicioDePartida.FinalizoElTimepo += DesactivarPausa;
        CronometroInicioDePartida.Start();
    }

    void Start()
    {
        SuscribirseAEventosDelJuego();
        txtCuentaRegresiva = GetComponentInChildren<Text>();
        InicializarCanvasInicioPartida();
    }

    private void VerificarSiEstaActivaLaPantallaDeEspera()
    {
        if (EstaElJuegoPausado)
        {
            cInicioPartida.enabled = true;
        }
        else
        {
            cInicioPartida.enabled = false;
            Time.timeScale = 1;
        }
    }

    private void ActualizarTextoCuentaRegresiva()
    {
        TextoEnCanvas = String.Format("El juego comenzara en {0}...", SegundosParaInicioDePartida);
    }

    // Update is called once per frame
    void Update()
    {
        VerificarSiEstaActivaLaPantallaDeEspera();
        txtCuentaRegresiva.text = TextoEnCanvas;
    }

    private void ActualizarSegundosContador()
    {
        SegundosParaInicioDePartida -= 1;
    }
}
