using Abstracciones.DA;
using Abstracciones.Modelos.Autenticacion;

namespace DA
{
    public class AutenticacionDA : IAutenticacionDA
    {
        public async Task<LoginBase?> ObtenerUsuario(LoginRequest login)
        {
            await Task.CompletedTask;

            if (login.NombreUsuario == "admin" && login.PasswordHash == "123")
            {
                return new LoginBase
                {
                    NombreUsuario = "admin",
                    CorreoElectronico = "admin@correo.com",
                    PasswordHash = "123"
                };
            }

            return null;
        }
    }
}