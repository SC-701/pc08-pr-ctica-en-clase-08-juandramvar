using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web.Pages.Productos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public List<ProductoResponse> Productos { get; set; } = new List<ProductoResponse>();
        public string MensajeError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet()
        {
            var token = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Seguridad/Login");
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            var url = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["ObtenerProductos"].TrimStart('/')}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Productos = JsonSerializer.Deserialize<List<ProductoResponse>>(resultado, opciones) ?? new List<ProductoResponse>();
            }
            else
            {
                var detalle = await response.Content.ReadAsStringAsync();
                MensajeError = $"Status: {(int)response.StatusCode} - {response.ReasonPhrase}. Detalle: {detalle}";
            }

            return Page();
        }
    }
}