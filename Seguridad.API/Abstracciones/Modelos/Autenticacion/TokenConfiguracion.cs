namespace Abstracciones.Modelos.Autenticacion
{
    public class TokenConfiguracion
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int Expires { get; set; }
    }
}