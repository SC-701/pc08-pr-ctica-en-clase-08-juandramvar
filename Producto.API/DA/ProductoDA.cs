using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        #region Operaciones

        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Guid.NewGuid(),
                    producto.IdSubCategoria,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    producto.CodigoBarras
                });

            return resultado;
        }

        public async Task<Guid> Editar(Guid id, ProductoRequest producto)
        {
            await verificarProductoExiste(id);

            string query = @"EditarProducto";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = id,
                    producto.IdSubCategoria,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    producto.CodigoBarras
                });

            return resultado;
        }

        public async Task<Guid> Eliminar(Guid id)
        {
            await verificarProductoExiste(id);

            string query = @"EliminarProducto";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new { Id = id });

            return resultado;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"ObtenerProductos";

            var resultado = await _sqlConnection
                .QueryAsync<ProductoResponse>(query);

            return resultado;
        }

        public async Task<ProductoResponse> Obtener(Guid id)
        {
            string query = @"ObtenerProducto";

            var resultado = await _sqlConnection
                .QueryAsync<ProductoResponse>(query, new { Id = id });

            return resultado.FirstOrDefault();
        }

        #endregion

        #region Helpers

        private async Task verificarProductoExiste(Guid id)
        {
            ProductoResponse? producto = await Obtener(id);

            if (producto == null)
                throw new Exception("No se encontró el producto");
        }

        #endregion
    }
}