namespace Abstracciones.Modelos
{
    public class APIEndPoint
    {
        public string UrlBase { get; set; } = string.Empty;
        public Dictionary<string, string> Metodos { get; set; } = new Dictionary<string, string>();
    }
}
