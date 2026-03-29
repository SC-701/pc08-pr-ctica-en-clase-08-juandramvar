using Abstracciones.Modelos.Autenticacion;

namespace Abstracciones.Flujo
{
    public interface IAutenticacionFlujo
    {
        Task<LoginResponse> LoginAsync(LoginRequest login);
    }
}