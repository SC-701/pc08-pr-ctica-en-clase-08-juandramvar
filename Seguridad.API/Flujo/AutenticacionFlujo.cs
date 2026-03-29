using Abstracciones.DA;
using Abstracciones.Flujo;
using Abstracciones.Modelos.Autenticacion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Flujo
{
    public class AutenticacionFlujo : IAutenticacionFlujo
    {
        private readonly IAutenticacionDA _autenticacionDA;
        private readonly IConfiguration _configuration;

        public AutenticacionFlujo(IAutenticacionDA autenticacionDA, IConfiguration configuration)
        {
            _autenticacionDA = autenticacionDA;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest login)
        {
            var usuario = await _autenticacionDA.ObtenerUsuario(login);

            if (usuario == null)
            {
                return new LoginResponse
                {
                    ValidacionExitosa = false,
                    AccessToken = string.Empty
                };
            }

            var token = GenerarToken(usuario);

            return new LoginResponse
            {
                ValidacionExitosa = true,
                AccessToken = token
            };
        }

        private string GenerarToken(LoginBase usuario)
        {
            var tokenConfiguracion = new TokenConfiguracion();
            _configuration.GetSection("Token").Bind(tokenConfiguracion);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguracion.Key));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: tokenConfiguracion.Issuer,
                audience: tokenConfiguracion.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenConfiguracion.Expires),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}