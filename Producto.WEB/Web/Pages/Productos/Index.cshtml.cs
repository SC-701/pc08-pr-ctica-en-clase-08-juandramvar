using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public List<ProductoResponse> Productos { get; set; } = new List<ProductoResponse>();
        public string MensajeError { get; set; } = string.Empty;
        public string UrlConsumida { get; set; } = string.Empty;

        public async Task OnGet()
        {
            var endPoint = _configuracion.ObtenerEndPoints();
            UrlConsumida = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["ObtenerProductos"].TrimStart('/')}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(UrlConsumida);

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
        }
    }
}