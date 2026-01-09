import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { Fornecedor, FornecedorFormData } from '@/types';
import { fornecedorService } from '@/services/fornecedorService';

export const useFornecedorStore = defineStore('fornecedor', () => {
    const fornecedores = ref<Fornecedor[]>([]);
    const fornecedorAtual = ref<Fornecedor | null>(null);
    const loading = ref(false);
    const error = ref<string | null>(null);

    /**
     * Buscar todos os fornecedores
     */
    async function fetchFornecedores() {
        loading.value = true;
        error.value = null;
        try {
            fornecedores.value = await fornecedorService.getAll();
        } catch (err: any) {
            error.value = err.message || 'Erro ao buscar fornecedores';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
     * Buscar fornecedor por ID
     */
    async function fetchFornecedorById(id: string) {
        loading.value = true;
        error.value = null;
        try {
            fornecedorAtual.value = await fornecedorService.getById(id);
            return fornecedorAtual.value;
        } catch (err: any) {
            error.value = err.message || 'Erro ao buscar fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
     * Criar novo fornecedor
     */
    async function criarFornecedor(fornecedor: FornecedorFormData) {
        loading.value = true;
        error.value = null;
        try {
            const novoFornecedor = await fornecedorService.create(fornecedor);
            fornecedores.value.push(novoFornecedor);
            return novoFornecedor;
        } catch (err: any) {
            error.value = err.message || 'Erro ao criar fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
     * Atualizar fornecedor
     */
    async function atualizarFornecedor(id: string, fornecedor: FornecedorFormData) {
        loading.value = true;
        error.value = null;
        try {
            const fornecedorAtualizado = await fornecedorService.update(id, fornecedor);
            const index = fornecedores.value.findIndex(f => f.id === id);
            if (index !== -1) {
                fornecedores.value[index] = fornecedorAtualizado;
            }
            if (fornecedorAtual.value?.id === id) {
                fornecedorAtual.value = fornecedorAtualizado;
            }
            return fornecedorAtualizado;
        } catch (err: any) {
            error.value = err.message || 'Erro ao atualizar fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
     * Deletar fornecedor
     */
    async function deletarFornecedor(id: string) {
        loading.value = true;
        error.value = null;
        try {
            await fornecedorService.delete(id);
            fornecedores.value = fornecedores.value.filter(f => f.id !== id);
            if (fornecedorAtual.value?.id === id) {
                fornecedorAtual.value = null;
            }
        } catch (err: any) {
            error.value = err.message || 'Erro ao deletar fornecedor';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    /**
     * Buscar fornecedores com filtros
     */
    async function buscarFornecedores(nome?: string, cpfCnpj?: string) {
        loading.value = true;
        error.value = null;
        try {
            fornecedores.value = await fornecedorService.search(nome, cpfCnpj);
        } catch (err: any) {
            error.value = err.message || 'Erro ao buscar fornecedores';
            throw err;
        } finally {
            loading.value = false;
        }
    }

    return {
        fornecedores,
        fornecedorAtual,
        loading,
        error,
        fetchFornecedores,
        fetchFornecedorById,
        criarFornecedor,
        atualizarFornecedor,
        deletarFornecedor,
        buscarFornecedores,
    };
});