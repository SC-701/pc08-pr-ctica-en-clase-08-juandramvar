using Abstracciones.Modelos.Autenticacion;

namespace Abstracciones.DA
{
    public interface IAutenticacionDA
    {
        Task<LoginBase?> ObtenerUsuario(LoginRequest login);
    }
}