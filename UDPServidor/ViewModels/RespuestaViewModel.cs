using UDPServidor.Models;
using UDPServidor.Models.Dtos;
using UDPServidor.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows;

namespace UDPServidor.ViewModels
{
    public class RespuestasViewModel : INotifyPropertyChanged
    {
        public string IP { get; set; } = "0.0.0.0";
        public string Binario { get; set; }
        public int NumeroAleatorio { get; set; }

        public bool TiempoRespuestas;

        System.Timers.Timer timerbinario;
        System.Timers.Timer timerrespuesta;

        public ObservableCollection<RespuestaDTO> Respuestas { get; set; } = new();

        public ObservableCollection<Usuario> NombresUsuarios { get; set; } = new();

        RespuestasServer server = new();

        public RespuestasViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            IP = ips
                .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .FirstOrDefault() ?? "0.0.0.0";
            GenerarBinario();
            timerbinario = new System.Timers.Timer(5000);
            timerbinario.Start();
            timerbinario.Elapsed += Timerbinario_Elapsed;
            timerbinario.AutoReset = false;
            server.RespuestaEnviada += Server_RespuestaEnviada;
        }

        private void Server_RespuestaEnviada(object? sender, RespuestaDTO e)
        {
            if (TiempoRespuestas == true)
            {
                if (e.Respuesta == NumeroAleatorio)
                {
                    NombresUsuarios.Add(new Usuario
                    {
                        NombreUsuario = e.NombreUsuario,
                    });
                }
            }
        }

        private void Timerbinario_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Binario = " ";
                OnPropertyChanged(nameof(Binario));
                TiempoRespuestas = true;
                timerrespuesta = new System.Timers.Timer(10000);
                timerrespuesta.Start();
                timerrespuesta.Elapsed += Timerrespuesta_Elapsed;
                timerrespuesta.AutoReset = false;
            });
        }

        private void Timerrespuesta_Elapsed(object? sender, ElapsedEventArgs e)
        {
            TiempoRespuestas = false;
        }

        private void GenerarBinario()
        {
            Random r = new Random();
            NumeroAleatorio = r.Next(0, 257);
            Binario = Convert.ToString(NumeroAleatorio, 2);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
