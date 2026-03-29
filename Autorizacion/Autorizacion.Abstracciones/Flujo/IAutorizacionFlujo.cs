using Autorizacion.Abstracciones.Modelos;
using System.Security.Claims;

namespace Autorizacion.Abstracciones.Flujo
{
    public interface IAutorizacionFlujo
    {
        List<Claim> ObtenerClaims(UsuarioAutorizado usuario);
    }
}