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
        UdpClient cliente;
        public RespuestasServer()
        {
            var hilo = new Thread(new ThreadStart(IniciarServidor))
            {
                IsBackground = true
            };
            hilo.Start();
        }

        public event EventHandler<RespuestaDTO>? RespuestaEnviada;

        public void MandarFelicitacion(string ip)
        {

            var ipep = new IPEndPoint(IPAddress.Parse(ip), 9001);
            var json = JsonSerializer.Serialize("¡Has acertado el número, felicidades!");
            var bytes = Encoding.UTF8.GetBytes(json);
            cliente.Send(bytes, bytes.Length, ipep);
        }

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
