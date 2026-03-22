import { describe, it, expect } from 'vitest'
import {
  apenasNumeros,
  validarCPF,
  validarCnpj,
  validarCEP,
  validarEmail,
  formatarCPF,
  formatarCNPJ,
  formatarCEP,
  calcularIdade,
  ehMenordeIdade,
} from '@/utils/validators'

// ── apenasNumeros ────────────────────────────────────────────────────────────

describe('apenasNumeros', () => {
  it('remove pontos, traços e barras', () => {
    expect(apenasNumeros('529.982.247-25')).toBe('52998224725')
  })

  it('remove espaços e outros caracteres', () => {
    expect(apenasNumeros('11.222.333/0001-81')).toBe('11222333000181')
  })

  it('retorna a string inalterada se já for apenas dígitos', () => {
    expect(apenasNumeros('12345678')).toBe('12345678')
  })

  it('retorna string vazia para entrada vazia', () => {
    expect(apenasNumeros('')).toBe('')
  })
})

// ── validarCPF ───────────────────────────────────────────────────────────────

describe('validarCPF', () => {
  it('retorna true para CPF válido sem formatação', () => {
    expect(validarCPF('52998224725')).toBe(true)
  })

  it('retorna true para CPF válido com formatação', () => {
    expect(validarCPF('529.982.247-25')).toBe(true)
  })

  it('retorna false para CPF com dígito verificador errado', () => {
    expect(validarCPF('52998224726')).toBe(false)
  })

  it('retorna false para CPF com todos os dígitos iguais', () => {
    expect(validarCPF('11111111111')).toBe(false)
    expect(validarCPF('00000000000')).toBe(false)
    expect(validarCPF('99999999999')).toBe(false)
  })

  it('retorna false para CPF com menos de 11 dígitos', () => {
    expect(validarCPF('1234567890')).toBe(false)
  })

  it('retorna false para CPF com mais de 11 dígitos', () => {
    expect(validarCPF('529982247250')).toBe(false)
  })

  it('retorna false para string vazia', () => {
    expect(validarCPF('')).toBe(false)
  })
})

// ── validarCnpj ──────────────────────────────────────────────────────────────

describe('validarCnpj', () => {
  it('retorna true para CNPJ válido sem formatação', () => {
    expect(validarCnpj('11222333000181')).toBe(true)
  })

  it('retorna true para CNPJ válido com formatação', () => {
    expect(validarCnpj('11.222.333/0001-81')).toBe(true)
  })

  it('retorna false para CNPJ com dígito verificador errado', () => {
    expect(validarCnpj('11222333000182')).toBe(false)
  })

  it('retorna false para CNPJ com todos os dígitos iguais', () => {
    expect(validarCnpj('00000000000000')).toBe(false)
    expect(validarCnpj('11111111111111')).toBe(false)
  })

  it('retorna false para CNPJ com menos de 14 dígitos', () => {
    expect(validarCnpj('1122233300018')).toBe(false)
  })

  it('retorna false para string vazia', () => {
    expect(validarCnpj('')).toBe(false)
  })
})

// ── validarCEP ───────────────────────────────────────────────────────────────

describe('validarCEP', () => {
  it('retorna true para CEP com 8 dígitos', () => {
    expect(validarCEP('12345678')).toBe(true)
  })

  it('retorna true para CEP formatado', () => {
    expect(validarCEP('12345-678')).toBe(true)
  })

  it('retorna false para CEP com menos de 8 dígitos', () => {
    expect(validarCEP('1234567')).toBe(false)
  })

  it('retorna false para CEP com mais de 8 dígitos', () => {
    expect(validarCEP('123456789')).toBe(false)
  })

  it('retorna false para string vazia', () => {
    expect(validarCEP('')).toBe(false)
  })
})

// ── validarEmail ─────────────────────────────────────────────────────────────

describe('validarEmail', () => {
  it('retorna true para e-mail válido', () => {
    expect(validarEmail('usuario@dominio.com')).toBe(true)
    expect(validarEmail('contato@empresa.com.br')).toBe(true)
  })

  it('retorna false para e-mail sem @', () => {
    expect(validarEmail('usuariodominio.com')).toBe(false)
  })

  it('retorna false para e-mail sem domínio', () => {
    expect(validarEmail('usuario@')).toBe(false)
  })

  it('retorna false para e-mail sem extensão', () => {
    expect(validarEmail('usuario@dominio')).toBe(false)
  })

  it('retorna false para string vazia', () => {
    expect(validarEmail('')).toBe(false)
  })
})

// ── formatarCPF ──────────────────────────────────────────────────────────────

describe('formatarCPF', () => {
  it('formata CPF sem máscara corretamente', () => {
    expect(formatarCPF('52998224725')).toBe('529.982.247-25')
  })

  it('formata CPF já com máscara sem duplicar', () => {
    expect(formatarCPF('529.982.247-25')).toBe('529.982.247-25')
  })
})

// ── formatarCNPJ ─────────────────────────────────────────────────────────────

describe('formatarCNPJ', () => {
  it('formata CNPJ sem máscara corretamente', () => {
    expect(formatarCNPJ('11222333000181')).toBe('11.222.333/0001-81')
  })

  it('formata CNPJ já com máscara sem duplicar', () => {
    expect(formatarCNPJ('11.222.333/0001-81')).toBe('11.222.333/0001-81')
  })
})

// ── formatarCEP ──────────────────────────────────────────────────────────────

describe('formatarCEP', () => {
  it('formata CEP sem máscara corretamente', () => {
    expect(formatarCEP('12345678')).toBe('12345-678')
  })

  it('formata CEP já com máscara sem duplicar', () => {
    expect(formatarCEP('12345-678')).toBe('12345-678')
  })
})

// ── calcularIdade ────────────────────────────────────────────────────────────

describe('calcularIdade', () => {
  it('retorna a idade correta para aniversário já passado no ano', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 25, hoje.getMonth() - 1, hoje.getDate())
    expect(calcularIdade(nascimento.toISOString())).toBe(25)
  })

  it('retorna N-1 para quem ainda não fez aniversário no ano corrente', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 25, hoje.getMonth() + 1, hoje.getDate())
    expect(calcularIdade(nascimento.toISOString())).toBe(24)
  })

  it('retorna a idade correta no dia do aniversário', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 18, hoje.getMonth(), hoje.getDate())
    expect(calcularIdade(nascimento.toISOString())).toBe(18)
  })
})

// ── ehMenordeIdade ───────────────────────────────────────────────────────────

describe('ehMenordeIdade', () => {
  it('retorna true para pessoa com 17 anos', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 17, hoje.getMonth() - 1, hoje.getDate())
    expect(ehMenordeIdade(nascimento.toISOString())).toBe(true)
  })

  it('retorna false para pessoa com 18 anos exatos', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 18, hoje.getMonth(), hoje.getDate())
    expect(ehMenordeIdade(nascimento.toISOString())).toBe(false)
  })

  it('retorna false para pessoa com 25 anos', () => {
    const hoje = new Date()
    const nascimento = new Date(hoje.getFullYear() - 25, hoje.getMonth() - 1, hoje.getDate())
    expect(ehMenordeIdade(nascimento.toISOString())).toBe(false)
  })
})
