export interface Fornecedor {
    id?: number
    cpfCnpj: string
    nome: string
    email: string
    cep: string
    logradouro?: string
    bairro?: string
    cidade?: string
    uf?: string

    // Campos específicos para Pessoa Física
    rg?: string
    dataNascimento?: Date
    
    // Relacionamento
    empresas?: number[]
    createdAt?: string
    updatedAt?: string
}

export interface FornecedorFormData {
    cpfCnpj: string
    nome: string
    email: string
    cep: string
    rg?: string
    dataNascimento?: string
    logradouro?: string
    bairro?: string
    cidade?: string
    uf?: string
}

export const TipoDocumento = {
  CPF: 'CPF',
  CNPJ: 'CNPJ',
} as const;