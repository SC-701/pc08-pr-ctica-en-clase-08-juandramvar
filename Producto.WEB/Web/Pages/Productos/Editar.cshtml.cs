using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    [Authorize]
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoRequest Producto { get; set; } = new ProductoRequest();

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

                var productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones);

                if (productoResponse != null)
                {
                    Producto = new ProductoRequest
                    {
                        Nombre = productoResponse.Nombre,
                        Descripcion = productoResponse.Descripcion,
                        Precio = productoResponse.Precio,
                        Stock = productoResponse.Stock,
                        CodigoBarras = productoResponse.CodigoBarras,
                        IdSubCategoria = productoResponse.IdSubCategoria
                    };
                }
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
            var token = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Seguridad/Login");
            }

            if (id == Guid.Empty)
            {
                return RedirectToPage("/Productos/Index");
            }

            if (Producto.IdSubCategoria == Guid.Empty)
            {
                MensajeError = "El Id de Subcategoría no es válido.";
                return Page();
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            var url = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["EditarProducto"].TrimStart('/')}/{id}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(Producto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(url, contenido);

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