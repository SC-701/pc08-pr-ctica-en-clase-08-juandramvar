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
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ProductoResponse Producto { get; set; } = new ProductoResponse();
        public string MensajeError { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(Guid id)
        {
            var token = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Seguridad/Login");
            }

            if (id == Guid.Empty)
            {
                return RedirectToPage("/Productos/Index");
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            var url = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["ObtenerProducto"].TrimStart('/')}/{id}";

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

                Producto = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones) ?? new ProductoResponse();
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