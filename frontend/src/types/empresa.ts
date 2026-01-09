import type { Fornecedor } from "./fornecedor"

export interface Empresa {
  id: string
  nomeFantasia: string
  cnpj: string
  cep: string

  // Dados do CEP (preenchidos automaticamente)
  logradouro?: string
  bairro?: string
  cidade?: string
  uf?: string
  
  // Relacionamento
  fornecedores?: Fornecedor[]
  createdAt?: string
  updatedAt?: string
}

export interface EmpresaFormData {
  nomeFantasia: string
  cnpj: string
  cep: string
  logradouro?: string
  bairro?: string
  cidade?: string
  uf?: string
}