using Abstracciones.Modelos;
using Abstracciones.Reglas;
using Microsoft.Extensions.Configuration;
namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private readonly IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public APIEndPoint ObtenerEndPoints()
        {
            var endPoint = new APIEndPoint();
            _configuration.GetSection("APIEndPoint").Bind(endPoint);
            return endPoint;
        }
    }
}