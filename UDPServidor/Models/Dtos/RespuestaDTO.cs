﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDPServidor.Models.Dtos
{
    public class RespuestaDTO
    {
        public int Respuesta { get; set; }
        public string IPUsuario { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
    }
}
