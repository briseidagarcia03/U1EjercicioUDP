using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UDPCliente.Models.Dtos;
using System.Windows;

namespace UDPCliente.Services
{
    class RespuestaService
    {
        private UdpClient cliente = new();
        public string Servidor { get; set; } = "0.0.0.0";

        public RespuestaService()
        {
            var thread = new Thread(Iniciar);
            thread.IsBackground = true;
            thread.Start();
        }

        public void EnviarRespuesta(RespuestaDTO dto)
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName());
            dto.IPUsuario = ips
                .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .Select(x => x.ToString())
                .FirstOrDefault() ?? "0.0.0.0";
            var ipep = new IPEndPoint(IPAddress.Parse(Servidor), 9000);
            dto.NombreUsuario = Dns.GetHostName();
            var json = JsonSerializer.Serialize(dto);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            cliente.Send(buffer, buffer.Length, ipep);
        }

        public void Iniciar()
        {
            IPEndPoint ipep = new(IPAddress.Any, 9001);
            while (true)
            {
                try
                {
                    byte[] buffer = cliente.Receive(ref ipep);
                    string respuesta = JsonSerializer.Deserialize<string>(Encoding.UTF8.GetString(buffer));
                    if (respuesta != null)
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            Felicitaciones?.Invoke(this, respuesta);
                        });
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public event EventHandler<string> Felicitaciones;
    }
}
