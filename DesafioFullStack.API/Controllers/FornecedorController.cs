using AutoMapper;
using DesafioFullStack.Application.DTOs;
using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Interfaces;
using DesafioFullStack.Domain.Services;
using DesafioFullStack.Domain.Validators;
using Microsoft.AspNetCore.Mvc;

namespace DesafioFullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorDomainService _domainService;
        private readonly IMapper _mapper;
        private readonly FornecedorValidator _validator;

        public FornecedorController(
            IFornecedorRepository fornecedorRepository,
            IFornecedorDomainService domainService,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _domainService = domainService;
            _mapper = mapper;
            _validator = new FornecedorValidator();
        }

        //Lista de Fornecedores com filtros opcionais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDto>>> GetAll([FromQuery] string? nome, [FromQuery] string? cpfCnpj)
        {
            IEnumerable<Fornecedor> fornecedores;

            if (!string.IsNullOrWhiteSpace(nome) || !string.IsNullOrWhiteSpace(cpfCnpj))
            {
                fornecedores = await _fornecedorRepository.GetByFiltersAsync(nome, cpfCnpj);
            }
            else
            {
                fornecedores = await _fornecedorRepository.GetAllAsync();
            }

            var fornecedoresDto = _mapper.Map<IEnumerable<FornecedorDto>>(fornecedores);
            return Ok(fornecedoresDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorDto>> GetById(Guid id)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);

            if (fornecedor == null)
                return NotFound(new { message = "Fornecedor não encontrado" });

            var fornecedorDto = _mapper.Map<FornecedorDto>(fornecedor);
            return Ok(fornecedorDto);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorDto>> Create([FromBody] CreateFornecedorDto createDto)
        {
            var fornecedor = _mapper.Map<Fornecedor>(createDto);

            // Validar entidade
            var validationResult = await _validator.ValidateAsync(fornecedor);
            if (!validationResult.IsValid)
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

            // Verificar CPF/CNPJ duplicado
            if (await _fornecedorRepository.CpfCnpjExistsAsync(fornecedor.CpfCnpj))
                return BadRequest(new { message = "CPF/CNPJ já cadastrado" });

            var fornecedorCriado = await _fornecedorRepository.AddAsync(fornecedor);
            var fornecedorDto = _mapper.Map<FornecedorDto>(fornecedorCriado);

            return CreatedAtAction(nameof(GetById), new { id = fornecedorDto.Id }, fornecedorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FornecedorDto>> Update(Guid id, [FromBody] UpdateFornecedorDto updateDto)
        {
            var fornecedorExistente = await _fornecedorRepository.GetByIdAsync(id);

            if (fornecedorExistente == null)
                return NotFound(new { message = "Fornecedor não encontrado" });

            // Mapear alterações
            _mapper.Map(updateDto, fornecedorExistente);

            var validationResult = await _validator.ValidateAsync(fornecedorExistente);
            if (!validationResult.IsValid)
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

            if (await _fornecedorRepository.CpfCnpjExistsAsync(fornecedorExistente.CpfCnpj, id))
                return BadRequest(new { message = "CPF/CNPJ já cadastrado" });

            await _fornecedorRepository.UpdateAsync(fornecedorExistente);
            var fornecedorDto = _mapper.Map<FornecedorDto>(fornecedorExistente);

            return Ok(fornecedorDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);

            if (fornecedor == null)
                return NotFound(new { message = "Fornecedor não encontrado" });

            await _fornecedorRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/empresas")]
        public async Task<ActionResult<IEnumerable<EmpresaDto>>> GetEmpresas(Guid id)
        {
            var empresas = await _fornecedorRepository.GetEmpresasByFornecedorIdAsync(id);
            var empresasDto = _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
            return Ok(empresasDto);
        }

        [HttpPost("{id}/validar-vinculo")]
        public async Task<ActionResult> ValidarVinculoComEmpresa(Guid id, [FromBody] ValidarVinculoDto dto)
        {
            var fornecedor = await _fornecedorRepository.GetByIdAsync(id);

            if (fornecedor == null)
                return NotFound(new { message = "Fornecedor não encontrado" });

            var podeVincular = _domainService.ValidarRegraParana(fornecedor, dto.CepEmpresa);

            if (!podeVincular)
                return BadRequest(new { message = "Empresas do Paraná não podem ter fornecedores pessoa física menores de 18 anos" });

            return Ok(new { message = "Fornecedor pode ser vinculado" });
        }
    }

    // DTO auxiliar para validação de vínculo
    public class ValidarVinculoDto
    {
        public string CepEmpresa { get; set; } = string.Empty;
    }
}
