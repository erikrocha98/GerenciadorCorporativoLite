import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useEmpresaStore } from '@/stores/empresaStore'
import type { Empresa } from '@/types'

vi.mock('@/services/empresaService', () => ({
  empresaService: {
    getAll: vi.fn(),
    getById: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn(),
    addFornecedor: vi.fn(),
    removeFornecedor: vi.fn(),
    getFornecedores: vi.fn(),
  },
}))

import { empresaService } from '@/services/empresaService'

const empresaMock: Empresa = {
  id: '1',
  cnpj: '11222333000181',
  nomeFantasia: 'Empresa Teste',
  cep: '12345678',
}

const empresaMock2: Empresa = {
  id: '2',
  cnpj: '45997418000153',
  nomeFantasia: 'Outra Empresa',
  cep: '87654321',
}

beforeEach(() => {
  setActivePinia(createPinia())
  vi.clearAllMocks()
})

// ── fetchEmpresas ─────────────────────────────────────────────────────────────

describe('fetchEmpresas', () => {
  it('popula o estado com as empresas retornadas pela API', async () => {
    vi.mocked(empresaService.getAll).mockResolvedValue([empresaMock, empresaMock2])

    const store = useEmpresaStore()
    await store.fetchEmpresas()

    expect(store.empresas).toHaveLength(2)
    expect(store.empresas[0].nomeFantasia).toBe('Empresa Teste')
  })

  it('define loading como true durante a chamada e false ao finalizar', async () => {
    let loadingDuranteChamada = false
    vi.mocked(empresaService.getAll).mockImplementation(async () => {
      loadingDuranteChamada = useEmpresaStore().loading
      return [empresaMock]
    })

    const store = useEmpresaStore()
    await store.fetchEmpresas()

    expect(loadingDuranteChamada).toBe(true)
    expect(store.loading).toBe(false)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(empresaService.getAll).mockRejectedValue(new Error('Erro de rede'))

    const store = useEmpresaStore()
    await expect(store.fetchEmpresas()).rejects.toThrow()

    expect(store.error).toBe('Erro de rede')
    expect(store.loading).toBe(false)
  })
})

// ── fetchEmpresaById ──────────────────────────────────────────────────────────

describe('fetchEmpresaById', () => {
  it('define empresaAtual com os dados retornados', async () => {
    vi.mocked(empresaService.getById).mockResolvedValue(empresaMock)

    const store = useEmpresaStore()
    await store.fetchEmpresaById('1')

    expect(store.empresaAtual).toEqual(empresaMock)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(empresaService.getById).mockRejectedValue(new Error('Não encontrado'))

    const store = useEmpresaStore()
    await expect(store.fetchEmpresaById('999')).rejects.toThrow()

    expect(store.error).toBe('Não encontrado')
  })
})

// ── criarEmpresa ──────────────────────────────────────────────────────────────

describe('criarEmpresa', () => {
  it('adiciona a nova empresa ao array de empresas', async () => {
    vi.mocked(empresaService.create).mockResolvedValue(empresaMock)

    const store = useEmpresaStore()
    await store.criarEmpresa({ cnpj: '11222333000181', nomeFantasia: 'Empresa Teste', cep: '12345678' })

    expect(store.empresas).toHaveLength(1)
    expect(store.empresas[0]).toEqual(empresaMock)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(empresaService.create).mockRejectedValue(new Error('CNPJ já cadastrado'))

    const store = useEmpresaStore()
    await expect(store.criarEmpresa({ cnpj: '11222333000181', nomeFantasia: 'Teste', cep: '12345678' })).rejects.toThrow()

    expect(store.error).toBe('CNPJ já cadastrado')
  })
})

// ── atualizarEmpresa ──────────────────────────────────────────────────────────

describe('atualizarEmpresa', () => {
  it('substitui a empresa no array pelo dado atualizado', async () => {
    const empresaAtualizada = { ...empresaMock, nomeFantasia: 'Nome Atualizado' }
    vi.mocked(empresaService.update).mockResolvedValue(empresaAtualizada)

    const store = useEmpresaStore()
    store.empresas = [empresaMock]

    await store.atualizarEmpresa('1', { cnpj: '11222333000181', nomeFantasia: 'Nome Atualizado', cep: '12345678' })

    expect(store.empresas[0].nomeFantasia).toBe('Nome Atualizado')
  })

  it('atualiza empresaAtual se for a empresa sendo editada', async () => {
    const empresaAtualizada = { ...empresaMock, nomeFantasia: 'Nome Atualizado' }
    vi.mocked(empresaService.update).mockResolvedValue(empresaAtualizada)

    const store = useEmpresaStore()
    store.empresas = [empresaMock]
    store.empresaAtual = empresaMock

    await store.atualizarEmpresa('1', { cnpj: '11222333000181', nomeFantasia: 'Nome Atualizado', cep: '12345678' })

    expect(store.empresaAtual?.nomeFantasia).toBe('Nome Atualizado')
  })
})

// ── deletarEmpresa ────────────────────────────────────────────────────────────

describe('deletarEmpresa', () => {
  it('remove a empresa do array', async () => {
    vi.mocked(empresaService.delete).mockResolvedValue(undefined)

    const store = useEmpresaStore()
    store.empresas = [empresaMock, empresaMock2]

    await store.deletarEmpresa('1')

    expect(store.empresas).toHaveLength(1)
    expect(store.empresas[0].id).toBe('2')
  })

  it('limpa empresaAtual se for a empresa deletada', async () => {
    vi.mocked(empresaService.delete).mockResolvedValue(undefined)

    const store = useEmpresaStore()
    store.empresas = [empresaMock]
    store.empresaAtual = empresaMock

    await store.deletarEmpresa('1')

    expect(store.empresaAtual).toBeNull()
  })

  it('não limpa empresaAtual se for outra empresa', async () => {
    vi.mocked(empresaService.delete).mockResolvedValue(undefined)

    const store = useEmpresaStore()
    store.empresas = [empresaMock, empresaMock2]
    store.empresaAtual = empresaMock2

    await store.deletarEmpresa('1')

    expect(store.empresaAtual?.id).toBe('2')
  })
})
