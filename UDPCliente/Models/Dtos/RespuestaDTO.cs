using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPCliente.Models.Dtos
{
    public class RespuestaDTO
    {
        public int Respuesta { get; set; }
        public string IPUsuario { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
    }
}
