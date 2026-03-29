namespace Abstracciones.Modelos.Autenticacion
{
    public class LoginResponse
    {
        public bool ValidacionExitosa { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}