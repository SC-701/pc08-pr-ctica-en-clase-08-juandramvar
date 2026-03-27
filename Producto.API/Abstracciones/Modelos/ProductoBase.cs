using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        [StringLength(100, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres", MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La propiedad descripción es requerida")]
        [StringLength(250, ErrorMessage = "La descripción debe tener entre 10 y 250 caracteres", MinimumLength = 10)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La propiedad precio es requerida")]
        [Range(0.01, 9999999, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad stock es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "La propiedad código de barras es requerida")]
        [RegularExpression(@"^[0-9]{8,13}$", ErrorMessage = "El código de barras debe tener entre 8 y 13 dígitos numéricos")]
        public string CodigoBarras { get; set; } = string.Empty;
    }

    public class ProductoRequest : ProductoBase
    {
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        public Guid Id { get; set; }
        public string SubCategoria { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        public decimal PrecioUSD { get; set; }
    }
}
