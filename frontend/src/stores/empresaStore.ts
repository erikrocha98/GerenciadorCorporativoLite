import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Empresa, EmpresaFormData } from '@/types';
import { empresaService } from '@/services/empresaService';

export const useEmpresaStore = defineStore('empresa', () => {
    //Estados com reative reference para vigiar quando componentes vão mudar.
    const empresas = ref<Empresa[]>([]);
    const empresaAtual = ref<Empresa | null>(null);
    const loading = ref(false);
    const error = ref<string | null>(null);

    //Funções que modificam o estado
    /**
   * Buscar todas as empresas
   */
    async function fetchEmpresas() {
        loading.value = true;
        error.value = null;
        try {
            empresas.value = await empresaService.getAll();
        } catch (err: any) {
            error.value = err.message || 'Erro ao buscar empresas';
            throw err;
        } finally {
            loading.value = false;
        }

    }

    /**
   * Buscar empresa por ID
   */
    async function fetchEmpresaById(id: string) {
        loading.value = true;
        error.value = null;
        try {
            empresaAtual.value = await empresaService.getById(id);
            return empresaAtual.value;
        } catch (err: any) {
            error.value = err.message || 'Erro ao buscar empresa';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
   * Criar nova empresa
   */
    async function criarEmpresa(empresa: EmpresaFormData) {
        loading.value = true;
        error.value = null;
        try {
            const novaEmpresa = await empresaService.create(empresa);
            empresas.value.push(novaEmpresa);
            return novaEmpresa;
        } catch (err: any) {
            error.value = err.message || 'Erro ao criar empresa';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
   * Atualizar empresa
   */
    async function atualizarEmpresa(id: string, empresa: EmpresaFormData) {
        loading.value = true;
        error.value = null;
        try {
            const empresaAtualizada = await empresaService.update(id, empresa);
            const index = empresas.value.findIndex(e => e.id === id);
            if (index !== -1) {
                empresas.value[index] = empresaAtualizada;
            }
            if (empresaAtual.value?.id === id) {
                empresaAtual.value = empresaAtualizada;
            }
            return empresaAtualizada;
        } catch (err: any) {
            error.value = err.message || 'Erro ao atualizar empresa';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
   * Deletar empresa
   */
    async function deletarEmpresa(id: string) {
        loading.value = true;
        error.value = null;
        try {
            await empresaService.delete(id);
            empresas.value = empresas.value.filter(e => e.id !== id);
            if (empresaAtual.value?.id === id) {
                empresaAtual.value = null;
            }
        } catch (err: any) {
            error.value = err.message || 'Erro ao deletar empresa';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
   * Adicionar fornecedor à empresa
   */
    async function adicionarFornecedor(empresaId: string, fornecedorId: string) {
        loading.value = true;
        error.value = null;
        try {
            await empresaService.addFornecedor(empresaId, fornecedorId);
            // Recarregar empresa para atualizar lista de fornecedores
            await fetchEmpresaById(empresaId);
        } catch (err: any) {
            error.value = err.message || 'Erro ao adicionar fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
   * Remover fornecedor da empresa
   */
    async function removerFornecedor(empresaId: string, fornecedorId: string) {
        loading.value = true;
        error.value = null;
        try {
            await empresaService.removeFornecedor(empresaId, fornecedorId);
            // Recarregar empresa para atualizar lista de fornecedores
            await fetchEmpresaById(empresaId);
        } catch (err: any) {
            error.value = err.message || 'Erro ao remover fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    return {
        empresas,
        empresaAtual,
        loading,
        error,
        fetchEmpresas,
        fetchEmpresaById,
        criarEmpresa,
        atualizarEmpresa,
        deletarEmpresa,
        adicionarFornecedor,
        removerFornecedor,
    };


})