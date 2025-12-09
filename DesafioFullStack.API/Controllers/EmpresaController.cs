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
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly EmpresaValidator _validator;
        private readonly IFornecedorDomainService _domainService;

        public EmpresaController(IEmpresaRepository empresaRepository, IMapper mapper, IFornecedorRepository fornecedorRepository, IFornecedorDomainService domainService)
        {
            _empresaRepository = empresaRepository;
            _mapper = mapper;
            _validator = new EmpresaValidator();
            _fornecedorRepository = fornecedorRepository;
            _domainService = domainService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaDto>>> GetAll()
        {
            var empresas = await _empresaRepository.GetAllAsync();
            var empresasDto = _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
            return Ok(empresasDto);
        }

        [HttpGet("id")]
        public async Task<ActionResult<EmpresaDto>> GetById(Guid id)
        {
            var empresa = await _empresaRepository.GetByIdAsync(id);

            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrada" });

            var empresaDto = _mapper.Map<EmpresaDto>(empresa);
            return Ok(empresaDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaDto>> Create([FromBody] CreateEmpresaDto createDto)
        {
            var empresa = _mapper.Map<Empresa>(createDto);

            // Validar entidade
            var validationResult = await _validator.ValidateAsync(empresa);
            if (!validationResult.IsValid)
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

            // Verificar CNPJ duplicado
            if (await _empresaRepository.CnpjExistsAsync(empresa.Cnpj))
                return BadRequest(new { message = "CNPJ já cadastrado" });

            var empresaCriada = await _empresaRepository.AddAsync(empresa);
            var empresaDto = _mapper.Map<EmpresaDto>(empresaCriada);

            return CreatedAtAction(nameof(GetById), new { id = empresaDto.Id }, empresaDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpresaDto>> Update(Guid id, [FromBody] UpdateEmpresaDto updateDto)
        {
            var empresaExistente = await _empresaRepository.GetByIdAsync(id);

            if (empresaExistente == null)
                return NotFound(new { message = "Empresa não encontrada" });

            // Mapear alterações
            _mapper.Map(updateDto, empresaExistente);

            // Validar entidade
            var validationResult = await _validator.ValidateAsync(empresaExistente);
            if (!validationResult.IsValid)
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });

            // Verificar CNPJ duplicado (excluindo a própria empresa)
            if (await _empresaRepository.CnpjExistsAsync(empresaExistente.Cnpj, id))
                return BadRequest(new { message = "CNPJ já cadastrado" });

            await _empresaRepository.UpdateAsync(empresaExistente);
            var empresaDto = _mapper.Map<EmpresaDto>(empresaExistente);

            return Ok(empresaDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var empresa = await _empresaRepository.GetByIdAsync(id);

            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrada" });

            await _empresaRepository.DeleteAsync(id);
            return NoContent();
        }

        //Lista Fornecedores com base no id de uma Empresa
        [HttpGet("{id}/fornecedores")]
        public async Task<ActionResult<IEnumerable<FornecedorDto>>> GetFornecedores(Guid id)
        {
            var fornecedores = await _empresaRepository.GetFornecedoresByEmpresaIdAsync(id);
            var fornecedoresDto = _mapper.Map<IEnumerable<FornecedorDto>>(fornecedores);
            return Ok(fornecedoresDto);
        }

        [HttpPost("{empresaId}/fornecedores/{fornecedorId}")]
        public async Task<ActionResult> VincularFornecedor(Guid empresaId, Guid fornecedorId)
        {
            var empresa = await _empresaRepository.GetByIdAsync(empresaId);
            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrada" });

            var fornecedor = await _fornecedorRepository.GetByIdAsync(fornecedorId);
            if (fornecedor==null)
                return NotFound(new { message = "Fornecedor não encontrado" });

            if (await _empresaRepository.VinculoExisteAsync(empresaId, fornecedorId))
                return BadRequest(new { message = "Fornecedor já está vinculado a esta empresa" });

            var podeVincular = _domainService.ValidarRegraParana(fornecedor, empresa.Cep);
            if (!podeVincular)
                return BadRequest(new { message = "Empresas do Paraná não podem ter fornecedores pessoa física menores de 18 anos" });

            await _empresaRepository.VincularFornecedorAsync(empresaId, fornecedorId);
            return Ok(new { message = "Fornecedor vinculado com sucesso" });
        }

        [HttpDelete("{empresaId}/fornecedores/{fornecedorId}")]
        public async Task<ActionResult> DesvincularFornecedor(Guid empresaId, Guid fornecedorId)
        {
            await _empresaRepository.DesvincularFornecedorAsync(empresaId, fornecedorId);
            return Ok(new { message = "Fornecedor desvinculado com sucesso" });
        }

    }
}
