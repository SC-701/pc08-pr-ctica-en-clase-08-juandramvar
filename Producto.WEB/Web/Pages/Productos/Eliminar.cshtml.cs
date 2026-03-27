using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ProductoResponse Producto { get; set; } = new ProductoResponse();
        public string MensajeError { get; set; } = string.Empty;
        public string UrlConsumida { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToPage("/Productos/Index");
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            UrlConsumida = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["ObtenerProducto"].TrimStart('/')}/{id}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(UrlConsumida);

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

        public async Task<IActionResult> OnPost(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToPage("/Productos/Index");
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            UrlConsumida = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["EliminarProducto"].TrimStart('/')}/{id}";

            using var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync(UrlConsumida);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Productos/Index");
            }

            var detalle = await response.Content.ReadAsStringAsync();
            MensajeError = $"Status: {(int)response.StatusCode} - {response.ReasonPhrase}. Detalle: {detalle}";
            return Page();
        }
    }
}