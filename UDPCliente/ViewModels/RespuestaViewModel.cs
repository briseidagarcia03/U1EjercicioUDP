using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UDPCliente.Models.Dtos;
using UDPCliente.Services;

namespace UDPCliente.ViewModels
{
    public class RespuestaViewModel : INotifyPropertyChanged
    {
        public RespuestaDTO Respuesta { get; set; } = new();
        public string Mensaje { get; set; }

        RespuestaService respuestaService = new();
        public ICommand EnviarRespuestaCommand { get; set; }
        public string IP { get; set; } = "172.23.32.1";
        System.Timers.Timer timerresetearmensajedefelicitaciones = new System.Timers.Timer(5000);
        private void InicializarTimer()
        {
            timerresetearmensajedefelicitaciones.Enabled = false;
            timerresetearmensajedefelicitaciones.Elapsed += (sender, e) =>
            {
                Mensaje = " ";
                timerresetearmensajedefelicitaciones.Stop();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
            };
        }
        public RespuestaViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
            respuestaService.Felicitaciones += RespuestaService_Felicitaciones;
            InicializarTimer();
        }

        private void RespuestaService_Felicitaciones(object? sender, string e)
        {
            Mensaje = e;
            timerresetearmensajedefelicitaciones.Start();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Mensaje)));
        }

        private void EnviarRespuesta()
        {
            respuestaService.Servidor = IP;
            respuestaService.EnviarRespuesta(Respuesta);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
