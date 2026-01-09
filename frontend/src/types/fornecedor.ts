import type { Empresa } from './empresa';

export interface Fornecedor {
    id?: string;
    cpfCnpj: string;
    nome: string;
    email: string;
    cep: string;
    logradouro?: string;
    bairro?: string;
    cidade?: string;
    uf?: string;

    // Campos específicos para Pessoa Física
    rg?: string;
    dataNascimento?: string; 
    
    // Relacionamento
    empresas?: Empresa[];
    createdAt?: string;
    updatedAt?: string;
}

export interface FornecedorFormData {
    cpfCnpj: string;
    nome: string;
    email: string;
    cep: string;
    rg?: string;
    dataNascimento?: string;
    logradouro?: string;
    bairro?: string;
    cidade?: string;
    uf?: string;
}


export const TipoDocumento = {
  CPF: 'CPF',
  CNPJ: 'CNPJ'
} as const;