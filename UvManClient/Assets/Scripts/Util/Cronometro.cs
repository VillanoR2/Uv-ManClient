using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Assets.Scripts.Util
{

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

        public void Iniciar()
        {
            Inicio = DateTime.Now;
            Start();
        }

        public void Detener()
        {
            Stop();
        }

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
