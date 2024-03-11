using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UDPCliente.Models.Dtos;

namespace UDPCliente.Services
{
    class RespuestaService
    {
        private UdpClient cliente = new();
        public string Servidor { get; set; } = "0.0.0.0";

        public void EnviarRespuesta(RespuestaDTO dto)
        {
            var ipep = new IPEndPoint(IPAddress.Parse(Servidor), 8000);
            var json = JsonSerializer.Serialize(dto);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            cliente.Send(buffer, buffer.Length, ipep);
        }
    }
}
