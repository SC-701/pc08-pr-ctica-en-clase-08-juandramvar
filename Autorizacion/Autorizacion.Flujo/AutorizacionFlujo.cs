using Autorizacion.Abstracciones.Flujo;
using Autorizacion.Abstracciones.Modelos;
using System.Security.Claims;

namespace Autorizacion.Flujo
{
    public class AutorizacionFlujo : IAutorizacionFlujo
    {
        public List<Claim> ObtenerClaims(UsuarioAutorizado usuario)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico)
            };
        }
    }
}