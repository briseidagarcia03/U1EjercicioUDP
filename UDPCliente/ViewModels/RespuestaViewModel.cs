﻿using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UDPCliente.Models.Dtos;
using UDPCliente.Services;

namespace UDPCliente.ViewModels
{
    public class RespuestaViewModel : INotifyPropertyChanged
    {
        public RespuestaDTO Respuesta { get; set; } = new();
        RespuestaService respuestaService = new();
        public ICommand EnviarRespuestaCommand { get; set; }
        public string IP { get; set; } = "0.0.0.0";

        public RespuestaViewModel()
        {
            EnviarRespuestaCommand = new RelayCommand(EnviarRespuesta);
        }

        private void EnviarRespuesta()
        {
            respuestaService.Servidor = IP;
            respuestaService.EnviarRespuesta(Respuesta);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}