using System;
using System.Timers;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// Es un cronometro que cada vez que pasa el tiempo asignado crea un evento
    /// </summary>
    public class Cronometro : System.Timers.Timer
    {
        private DateTime Inicio;
        public delegate void TiempoTranscurrido();
        public delegate void TiempoFinalizado();
        public event TiempoFinalizado FinalizoElTimepo;
        public event TiempoTranscurrido TranscurrioUnIntervalo;
        private double TiempoTotalATranscurrir;
        private double MilisegundosTranscurridos = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="intervalo">En milisegundos</param>
        /// <param name="tiempoFinalizar">En milisegundos</param>
        public Cronometro(double intervalo, double tiempoFinalizar)
        {
            Interval = intervalo;
            Elapsed += Tic;
            TiempoTotalATranscurrir = tiempoFinalizar;
        }

        /// <summary>
        /// Inicia el conteo del cronometro
        /// </summary>
        public void Iniciar()
        {
            Inicio = DateTime.Now;
            Start();
        }

        /// <summary>
        /// Detiene el conteo del cronometro
        /// </summary>
        public void Detener()
        {
            Stop();
        }
        
        /// <summary>
        /// Evento capturado cada vez que transcurre el intervalo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ElepasedEventArgs</param>
        private void Tic(object sender, ElapsedEventArgs e)
        {
            TranscurrioUnIntervalo?.Invoke();
            MilisegundosTranscurridos += Interval;
            if(MilisegundosTranscurridos >= TiempoTotalATranscurrir)
            {
                Detener();
                FinalizoElTimepo?.Invoke();
            }
        }
    }
}
