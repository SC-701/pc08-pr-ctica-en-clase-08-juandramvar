using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
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
            if (Producto.IdSubCategoria == Guid.Empty)
            {
                MensajeError = "El Id de Subcategoría no es válido o no existe.";
                return Page();
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            UrlConsumida = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["EditarProducto"].TrimStart('/')}/{id}";

            using var httpClient = new HttpClient();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = JsonSerializer.Serialize(Producto, opciones);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync(UrlConsumida, contenido);

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