using Abstracciones.Modelos.Producto;
using Abstracciones.Reglas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoRequest Producto { get; set; } = new ProductoRequest();

        public string MensajeError { get; set; } = string.Empty;
        public string UrlConsumida { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (Producto.IdSubCategoria == Guid.Empty)
            {
                MensajeError = "El Id de Subcategoría no es válido o no existe.";
                return Page();
            }
            var endPoint = _configuracion.ObtenerEndPoints();
            UrlConsumida = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["AgregarProducto"].TrimStart('/')}";

            using var httpClient = new HttpClient();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = JsonSerializer.Serialize(Producto, opciones);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(UrlConsumida, contenido);

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