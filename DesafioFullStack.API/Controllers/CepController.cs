using DesafioFullStack.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet("{cep}")]
        public async Task<ActionResult<CepResponse>> BuscarCep(string cep)
        {
            var resultado = await _cepService.BuscarCepAsync(cep);

            if (resultado == null)
                return NotFound(new { message = "CEP não encontrado ou inválido" });

            return Ok(resultado);
        }

        [HttpGet("{cep}/validar")]
        public async Task<ActionResult> ValidarCep(string cep)
        {
            var valido = await _cepService.ValidarCepAsync(cep);

            if (!valido)
                return BadRequest(new { message = "CEP inválido", valido = false });

            return Ok(new { message = "CEP válido", valido = true });
        }
    }
}
