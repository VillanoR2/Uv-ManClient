using Assets.Scripts.Util;
using GameService.Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class ManejoDeCanvas : MonoBehaviour
{
    private Canvas cInicioPartida;
    private bool EstaElJuegoEnInicio;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private Cronometro CronometroInicioDePartida;
    private Text txtCuentaRegresiva;
    private int SegundosParaInicioDePartida;

    void InicializarCanvasInicioPartida()
    {
        EstaElJuegoEnInicio = true;
        cInicioPartida = GetComponent<Canvas>();
        cInicioPartida.enabled = true;
        SegundosParaInicioDePartida = 5;
        Time.timeScale = 0;
        RealizarConteo();
    }

    private void SuscribirseAEventosDelJuego()
    {
        ClienteDelJuego.IniciaLaPartida += RealizarAccionDeInicio;
    }

    private void RealizarAccionDeInicio(InicioPartida iniciaPartida)
    {
        if (iniciaPartida.IniciarPartida)
        {
            if (EstaElJuegoEnInicio)
            {
                DesactivarPausa();
            }
        }else if (iniciaPartida.CambiarPantallaMultijugador)
        {
            RealizarConteo();
        }
    }

    private void DesactivarPausa()
    {
        EstaElJuegoEnInicio = false;
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
        txtCuentaRegresiva = GetComponentInChildren<Text>();
        InicializarCanvasInicioPartida();
    }

    private void VerificarSiEstaActivaLaPantallaDeEspera()
    {
        if (EstaElJuegoEnInicio)
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
        txtCuentaRegresiva.text = String.Format("El juego comenzara en {0}...", SegundosParaInicioDePartida);
    }

    // Update is called once per frame
    void Update()
    {
        VerificarSiEstaActivaLaPantallaDeEspera();
        ActualizarTextoCuentaRegresiva();
    }

    private void ActualizarSegundosContador()
    {
        SegundosParaInicioDePartida -= 1;
    }
}
