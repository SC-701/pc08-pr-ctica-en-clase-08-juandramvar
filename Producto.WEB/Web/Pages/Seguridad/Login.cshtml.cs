using Abstracciones.Modelos.Autenticacion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Seguridad
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public LoginModelInput LoginInput { get; set; } = new LoginModelInput();

        public string MensajeError { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var urlBase = _configuration["SeguridadAPI:UrlBase"];
            var loginPath = _configuration["SeguridadAPI:Login"];
            var url = $"{urlBase!.TrimEnd('/')}/{loginPath!.TrimStart('/')}";

            var loginRequest = new LoginRequest
            {
                NombreUsuario = LoginInput.NombreUsuario,
                PasswordHash = LoginInput.PasswordHash,
                CorreoElectronico = string.Empty
            };

            using var httpClient = new HttpClient();

            var json = JsonSerializer.Serialize(loginRequest);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, contenido);

            if (!response.IsSuccessStatusCode)
            {
                MensajeError = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var resultado = await response.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(resultado, opciones);

            if (loginResponse == null || !loginResponse.ValidacionExitosa || string.IsNullOrEmpty(loginResponse.AccessToken))
            {
                MensajeError = "No se pudo iniciar sesión.";
                return Page();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.AccessToken);

            var claims = new List<Claim>(jwtToken.Claims)
            {
                new Claim("AccessToken", loginResponse.AccessToken)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToPage("/Productos/Index");
        }

        public class LoginModelInput
        {
            [Required]
            public string NombreUsuario { get; set; } = string.Empty;

            [Required]
            public string PasswordHash { get; set; } = string.Empty;
        }
    }
}