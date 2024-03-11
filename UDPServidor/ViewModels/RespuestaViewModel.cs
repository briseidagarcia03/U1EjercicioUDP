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
using System.Net.Sockets;
using System.Text.Json;
using System.Text;

namespace UDPServidor.ViewModels
{
    public class RespuestaViewModel : INotifyPropertyChanged
    {
        public string IP { get; set; } = "0.0.0.0";
        public string Binario { get; set; }
        public int NumeroAleatorio { get; set; }

        public bool TiempoRespuestas;

        System.Timers.Timer timerbinario = new System.Timers.Timer(5000);
        System.Timers.Timer timermostrarrespuestas = new System.Timers.Timer(15000);
        System.Timers.Timer timerreinicio = new System.Timers.Timer(20000);

        public ObservableCollection<Usuario> RespuestasAcertadas { get; set; } = new();

        public List<RespuestaDTO> Ganadores { get; set; } = new();
        

        public UdpClient cliente;

        RespuestasServer server = new();

        public RespuestaViewModel()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            cliente = new();
            IP = ips
                .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .FirstOrDefault() ?? "0.0.0.0";
            GenerarBinario();
            IniciarTimer();
            //timerbinario = new System.Timers.Timer(5000);
            //timerbinario.Start();
            //timerbinario.Elapsed += Timerbinario_Elapsed;
            //timerbinario.AutoReset = false;
            server.RespuestaEnviada += Server_RespuestaEnviada;
        }

        private void IniciarTimer()
        {
            TiempoRespuestas = true;
            timerbinario.Enabled = true;
            timerbinario.Elapsed += (sender, e) =>
            {
                OcultarNumeroBinario();
                timerbinario.Stop();
            };
            timermostrarrespuestas.Enabled = true;
            timermostrarrespuestas.Elapsed += (sender, e) =>
            {
                Felicitacion();
                timermostrarrespuestas.Stop();
            };
            timerreinicio.Enabled = true;
            timerreinicio.Elapsed += (sender, e) =>
            {
                timerreinicio.Stop();
                timerreinicio.Start();
                timerbinario.Start();
                timermostrarrespuestas.Start();
                Reiniciar();
            };
        }

        private void Server_RespuestaEnviada(object? sender, RespuestaDTO e)
        {
            if (TiempoRespuestas == true)
            {
                if (e.Respuesta == NumeroAleatorio)
                {
                    Ganadores.Add(e);
                }
            }
        }

        private void Felicitacion()
        {
            foreach (var item in Ganadores)
            {
                server.MandarFelicitacion(item.IPUsuario);
            }
        }

        private void Reiniciar()
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Ganadores.Clear();
                RespuestasAcertadas.Clear();
                GenerarBinario();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Binario"));
            });
        }

        private void OcultarNumeroBinario()
        {
            Binario = " ";
            timerbinario.Stop();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Binario"));
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