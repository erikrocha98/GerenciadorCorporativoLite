export interface Empresa {
  id: number
  nomeFantasia: string
  cnpj: string
  cep: string

  // Dados do CEP (preenchidos automaticamente)
  logradouro?: string
  bairro?: string
  cidade?: string
  uf?: string
  
  // Relacionamento
  fornecedores?: number[]
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