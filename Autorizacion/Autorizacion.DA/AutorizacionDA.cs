using Autorizacion.Abstracciones.DA;
using Autorizacion.Abstracciones.Modelos;

namespace Autorizacion.DA
{
    public class AutorizacionDA : IAutorizacionDA
    {
        public UsuarioAutorizado ObtenerUsuario(string nombreUsuario, string correoElectronico)
        {
            return new UsuarioAutorizado
            {
                NombreUsuario = nombreUsuario,
                CorreoElectronico = correoElectronico
            };
        }
    }
}