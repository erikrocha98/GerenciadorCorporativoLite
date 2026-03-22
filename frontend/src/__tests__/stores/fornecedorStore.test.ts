import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useFornecedorStore } from '@/stores/fornecedorStore'
import type { Fornecedor } from '@/types'

vi.mock('@/services/fornecedorService', () => ({
  fornecedorService: {
    getAll: vi.fn(),
    getById: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn(),
    search: vi.fn(),
    getEmpresas: vi.fn(),
  },
}))

import { fornecedorService } from '@/services/fornecedorService'

const fornecedorPJMock: Fornecedor = {
  id: '1',
  cpfCnpj: '11222333000181',
  nome: 'Empresa XYZ Ltda',
  email: 'contato@xyz.com',
  cep: '12345678',
}

const fornecedorPFMock: Fornecedor = {
  id: '2',
  cpfCnpj: '52998224725',
  nome: 'João Silva',
  email: 'joao@teste.com',
  cep: '87654321',
  rg: '1234567',
  dataNascimento: '1990-05-15',
}

beforeEach(() => {
  setActivePinia(createPinia())
  vi.clearAllMocks()
})

// ── fetchFornecedores ─────────────────────────────────────────────────────────

describe('fetchFornecedores', () => {
  it('popula o estado com os fornecedores retornados pela API', async () => {
    vi.mocked(fornecedorService.getAll).mockResolvedValue([fornecedorPJMock, fornecedorPFMock])

    const store = useFornecedorStore()
    await store.fetchFornecedores()

    expect(store.fornecedores).toHaveLength(2)
    expect(store.fornecedores[0].nome).toBe('Empresa XYZ Ltda')
  })

  it('define loading como true durante a chamada e false ao finalizar', async () => {
    let loadingDuranteChamada = false
    vi.mocked(fornecedorService.getAll).mockImplementation(async () => {
      loadingDuranteChamada = useFornecedorStore().loading
      return [fornecedorPJMock]
    })

    const store = useFornecedorStore()
    await store.fetchFornecedores()

    expect(loadingDuranteChamada).toBe(true)
    expect(store.loading).toBe(false)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(fornecedorService.getAll).mockRejectedValue(new Error('Erro de rede'))

    const store = useFornecedorStore()
    await expect(store.fetchFornecedores()).rejects.toThrow()

    expect(store.error).toBe('Erro de rede')
    expect(store.loading).toBe(false)
  })
})

// ── fetchFornecedorById ───────────────────────────────────────────────────────

describe('fetchFornecedorById', () => {
  it('define fornecedorAtual com os dados retornados', async () => {
    vi.mocked(fornecedorService.getById).mockResolvedValue(fornecedorPFMock)

    const store = useFornecedorStore()
    await store.fetchFornecedorById('2')

    expect(store.fornecedorAtual).toEqual(fornecedorPFMock)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(fornecedorService.getById).mockRejectedValue(new Error('Não encontrado'))

    const store = useFornecedorStore()
    await expect(store.fetchFornecedorById('999')).rejects.toThrow()

    expect(store.error).toBe('Não encontrado')
  })
})

// ── criarFornecedor ───────────────────────────────────────────────────────────

describe('criarFornecedor', () => {
  it('adiciona o novo fornecedor ao array de fornecedores', async () => {
    vi.mocked(fornecedorService.create).mockResolvedValue(fornecedorPJMock)

    const store = useFornecedorStore()
    await store.criarFornecedor({
      cpfCnpj: '11222333000181',
      nome: 'Empresa XYZ Ltda',
      email: 'contato@xyz.com',
      cep: '12345678',
    })

    expect(store.fornecedores).toHaveLength(1)
    expect(store.fornecedores[0]).toEqual(fornecedorPJMock)
  })

  it('define error quando a API falha', async () => {
    vi.mocked(fornecedorService.create).mockRejectedValue(new Error('CPF/CNPJ já cadastrado'))

    const store = useFornecedorStore()
    await expect(store.criarFornecedor({
      cpfCnpj: '11222333000181',
      nome: 'Teste',
      email: 'teste@teste.com',
      cep: '12345678',
    })).rejects.toThrow()

    expect(store.error).toBe('CPF/CNPJ já cadastrado')
  })
})

// ── atualizarFornecedor ───────────────────────────────────────────────────────

describe('atualizarFornecedor', () => {
  it('substitui o fornecedor no array pelo dado atualizado', async () => {
    const atualizado = { ...fornecedorPJMock, nome: 'Nome Atualizado' }
    vi.mocked(fornecedorService.update).mockResolvedValue(atualizado)

    const store = useFornecedorStore()
    store.fornecedores = [fornecedorPJMock]

    await store.atualizarFornecedor('1', {
      cpfCnpj: '11222333000181',
      nome: 'Nome Atualizado',
      email: 'contato@xyz.com',
      cep: '12345678',
    })

    expect(store.fornecedores[0].nome).toBe('Nome Atualizado')
  })

  it('atualiza fornecedorAtual se for o fornecedor sendo editado', async () => {
    const atualizado = { ...fornecedorPJMock, nome: 'Nome Atualizado' }
    vi.mocked(fornecedorService.update).mockResolvedValue(atualizado)

    const store = useFornecedorStore()
    store.fornecedores = [fornecedorPJMock]
    store.fornecedorAtual = fornecedorPJMock

    await store.atualizarFornecedor('1', {
      cpfCnpj: '11222333000181',
      nome: 'Nome Atualizado',
      email: 'contato@xyz.com',
      cep: '12345678',
    })

    expect(store.fornecedorAtual?.nome).toBe('Nome Atualizado')
  })
})

// ── deletarFornecedor ─────────────────────────────────────────────────────────

describe('deletarFornecedor', () => {
  it('remove o fornecedor do array', async () => {
    vi.mocked(fornecedorService.delete).mockResolvedValue(undefined)

    const store = useFornecedorStore()
    store.fornecedores = [fornecedorPJMock, fornecedorPFMock]

    await store.deletarFornecedor('1')

    expect(store.fornecedores).toHaveLength(1)
    expect(store.fornecedores[0].id).toBe('2')
  })

  it('limpa fornecedorAtual se for o fornecedor deletado', async () => {
    vi.mocked(fornecedorService.delete).mockResolvedValue(undefined)

    const store = useFornecedorStore()
    store.fornecedores = [fornecedorPJMock]
    store.fornecedorAtual = fornecedorPJMock

    await store.deletarFornecedor('1')

    expect(store.fornecedorAtual).toBeNull()
  })

  it('não limpa fornecedorAtual se for outro fornecedor', async () => {
    vi.mocked(fornecedorService.delete).mockResolvedValue(undefined)

    const store = useFornecedorStore()
    store.fornecedores = [fornecedorPJMock, fornecedorPFMock]
    store.fornecedorAtual = fornecedorPFMock

    await store.deletarFornecedor('1')

    expect(store.fornecedorAtual?.id).toBe('2')
  })
})

// ── buscarFornecedores ────────────────────────────────────────────────────────

describe('buscarFornecedores', () => {
  it('chama o service com os filtros fornecidos', async () => {
    vi.mocked(fornecedorService.search).mockResolvedValue([fornecedorPFMock])

    const store = useFornecedorStore()
    await store.buscarFornecedores('João', '52998224725')

    expect(fornecedorService.search).toHaveBeenCalledWith('João', '52998224725')
    expect(store.fornecedores).toHaveLength(1)
  })

  it('chama o service sem filtros quando nenhum é passado', async () => {
    vi.mocked(fornecedorService.search).mockResolvedValue([fornecedorPJMock, fornecedorPFMock])

    const store = useFornecedorStore()
    await store.buscarFornecedores()

    expect(fornecedorService.search).toHaveBeenCalledWith(undefined, undefined)
    expect(store.fornecedores).toHaveLength(2)
  })
})
