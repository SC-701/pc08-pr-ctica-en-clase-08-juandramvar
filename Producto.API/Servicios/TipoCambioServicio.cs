using Abstracciones.Interfaces.Servicios;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

public class TipoCambioServicio : ITipoCambioServicio
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public TipoCambioServicio(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<decimal> ObtenerTipoCambioAsync()
    {
        var url = _config["BancoCentralCR:UrlBase"];
        var token = _config["BancoCentralCR:BearerToken"];

        var hoy = DateTime.Now.ToString("yyyy/MM/dd");
        var requestUrl = $"{url}?fechaInicio={hoy}&fechaFin={hoy}&idioma=ES";

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);

        var tipoCambio =
            doc.RootElement
            .GetProperty("datos")[0]
            .GetProperty("indicadores")[0]
            .GetProperty("series")[0]
            .GetProperty("valorDatoPorPeriodo")
            .GetDecimal();

        return tipoCambio;
    }
}

