namespace Abstracciones.Modelos.Autenticacion
{
    public class LoginRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
    }
}