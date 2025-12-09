using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DesafioFullStack.Application.DTOs;
using DesafioFullStack.Domain.Entities;

namespace DesafioFullStack.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Empresa, EmpresaDto>();
            CreateMap<CreateEmpresaDto, Empresa>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.EmpresaFornecedores, opt => opt.Ignore());

            CreateMap<UpdateEmpresaDto, Empresa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.EmpresaFornecedores, opt => opt.Ignore());

            // Fornecedor
            CreateMap<Fornecedor, FornecedorDto>()
                .ForMember(dest => dest.EhPessoaFisica, opt => opt.MapFrom(src => src.EhPessoaFisica));

            CreateMap<CreateFornecedorDto, Fornecedor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
                .ForMember(dest => dest.EmpresaFornecedores, opt => opt.Ignore());

            CreateMap<UpdateFornecedorDto, Fornecedor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DataCadastro, opt => opt.Ignore())
                .ForMember(dest => dest.DataAtualizacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.EmpresaFornecedores, opt => opt.Ignore());
        }
    }
}
