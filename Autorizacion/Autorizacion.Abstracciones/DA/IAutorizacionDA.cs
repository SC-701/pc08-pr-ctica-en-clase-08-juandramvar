using Autorizacion.Abstracciones.Modelos;

namespace Autorizacion.Abstracciones.DA
{
    public interface IAutorizacionDA
    {
        UsuarioAutorizado ObtenerUsuario(string nombreUsuario, string correoElectronico);
    }
}