using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using UDPServidor.Models.Dtos;

namespace UDPServidor.Services
{
    internal class RespuestasServer
    {
        public RespuestasServer()
        {
            var hilo = new Thread(new ThreadStart(IniciarServidor))
            {
                IsBackground = true
            };
            hilo.Start();
        }

        public event EventHandler<RespuestaDTO>? RespuestaEnviada;

        void IniciarServidor()
        {
            UdpClient servidor = new(9000);
            while (true)
            {
                IPEndPoint ipep = new(IPAddress.Any, 9000);
                byte[] buffer = servidor.Receive(ref ipep);

                RespuestaDTO? dto = JsonSerializer.Deserialize<RespuestaDTO>(Encoding.UTF8.GetString(buffer));
                if (dto != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        RespuestaEnviada?.Invoke(this, dto);
                    });
                }
            }
        }
    }
}
