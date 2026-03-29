using Abstracciones.Flujo;
using Abstracciones.Modelos.Autenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionFlujo _autenticacionFlujo;

        public AutenticacionController(IAutenticacionFlujo autenticacionFlujo)
        {
            _autenticacionFlujo = autenticacionFlujo;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var resultado = await _autenticacionFlujo.LoginAsync(login);

            if (!resultado.ValidacionExitosa)
            {
                return Unauthorized(resultado);
            }

            return Ok(resultado);
        }
    }
}