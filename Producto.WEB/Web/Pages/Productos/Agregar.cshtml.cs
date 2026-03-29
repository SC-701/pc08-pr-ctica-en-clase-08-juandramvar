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

        public IActionResult OnGet()
        {
            var token = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Seguridad/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Seguridad/Login");
            }

            if (Producto.IdSubCategoria == Guid.Empty)
            {
                MensajeError = "El Id de Subcategoría no es válido.";
                return Page();
            }

            var endPoint = _configuracion.ObtenerEndPoints();
            var url = $"{endPoint.UrlBase.TrimEnd('/')}/{endPoint.Metodos["AgregarProducto"].TrimStart('/')}";

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(Producto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, contenido);

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