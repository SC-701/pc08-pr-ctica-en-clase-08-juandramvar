using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Abstracciones.Interfaces.Reglas;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private readonly IProductoDA _productoDA;
        private readonly IProductoReglas _productoReglas;
        public ProductoFlujo(
            IProductoDA productoDA,
            IProductoReglas productoReglas)
        {
            _productoDA = productoDA;
            _productoReglas = productoReglas;
        }
        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid id, ProductoRequest producto)
        {
            return _productoDA.Editar(id, producto);
        }

        public Task<Guid> Eliminar(Guid id)
        {
            return _productoDA.Eliminar(id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productoDA.Obtener();
        }

        public async Task<ProductoResponse> Obtener(Guid id)
        {
            var producto = await _productoDA.Obtener(id);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            producto.PrecioUSD =
                await _productoReglas.CalcularPrecioUSD(producto.Precio);

            return producto;
        }
    }
}
