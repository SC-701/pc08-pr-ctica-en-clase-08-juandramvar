using System.Net.Http;
using System.Security.Claims;
using Autorizacion.Abstracciones.Flujo;
using Autorizacion.Abstracciones.Modelos;
using Microsoft.AspNetCore.Http;

namespace Autorizacion.Middleware
{
    public class ClaimsAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimsAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAutorizacionFlujo autorizacionFlujo)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var nombreUsuario = context.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
                var correoElectronico = context.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

                var usuario = new UsuarioAutorizado
                {
                    NombreUsuario = nombreUsuario,
                    CorreoElectronico = correoElectronico
                };

                var claimsAdicionales = autorizacionFlujo.ObtenerClaims(usuario);

                var identidadActual = context.User.Identity as ClaimsIdentity;

                if (identidadActual != null)
                {
                    foreach (var claim in claimsAdicionales)
                    {
                        if (!identidadActual.HasClaim(c => c.Type == claim.Type && c.Value == claim.Value))
                        {
                            identidadActual.AddClaim(claim);
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}