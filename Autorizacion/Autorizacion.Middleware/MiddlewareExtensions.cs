using Autorizacion.Abstracciones.DA;
using Autorizacion.Abstracciones.Flujo;
using Autorizacion.DA;
using Autorizacion.Flujo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Autorizacion.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddAutorizacionMiddleware(this IServiceCollection services)
        {
            services.AddScoped<IAutorizacionDA, AutorizacionDA>();
            services.AddScoped<IAutorizacionFlujo, AutorizacionFlujo>();

            return services;
        }

        public static IApplicationBuilder AutorizacionClaims(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ClaimsAuthorizationMiddleware>();
        }
    }
}