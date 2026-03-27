using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;

public class ProductoReglas : IProductoReglas
{
    private readonly ITipoCambioServicio _tipoCambio;

    public ProductoReglas(ITipoCambioServicio tipoCambio)
    {
        _tipoCambio = tipoCambio;
    }

    public async Task<decimal> CalcularPrecioUSD(decimal precioCRC)
    {
        var tc = await _tipoCambio.ObtenerTipoCambioAsync();
        return Math.Round(precioCRC / tc, 2);
    }
}