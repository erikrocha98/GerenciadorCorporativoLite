import type { Empresa, EmpresaFormData, Fornecedor } from '@/types';
import api from './api';

export const empresaService = {
    /**
   * Buscar todas as empresas
   */
    async getAll(): Promise<Empresa[]> {
        try {
            const response = await api.get<Empresa[]>('/Empresa');
            return response.data;
        } catch (error) {
            console.error('Erro ao buscar empresas', error);
            throw error;
        }
    },

    /**
   * Buscar empresa por ID
   */
    async getById(id: string): Promise<Empresa> {
        console.log('Fetching empresa with ID:', id);
        try {
            const response = await api.get<Empresa>(`/Empresa/${id}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao buscar empresa:', error);
            throw error;
        }
    },

    /**
   * Criar nova empresa
   */
    async create(empresa: EmpresaFormData): Promise<Empresa> {
        try {
            const response = await api.post<Empresa>('/Empresa', empresa);
            return response.data;
        } catch (error) {
            console.error('Erro ao criar empresa:', error);
            throw error;
        }
    },

    /**
   * Atualizar empresa
   */
    async update(id: string, empresa: EmpresaFormData): Promise<Empresa> {
        try {
            const response = await api.put<Empresa>(`/Empresa/${id}`, empresa);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar empresa:', error);
            throw error;
        }
    },

    /**
   * Deletar empresa
   */
    async delete(id: string): Promise<void> {
        try {
            await api.delete(`/Empresa/${id}`);
        } catch (error) {
            console.error('Erro ao deletar empresa:', error);
            throw error;
        }
    },

    /**
   * Buscar fornecedores de uma empresa
   */
    async getFornecedores(id: string): Promise<Fornecedor[]> {
        try {
            const response = await api.get<Fornecedor[]>(`/Empresa/${id}/fornecedores`);
            return response.data;
        } catch (error) {
            console.error('Erro ao buscar fornecedores da empresa:', error);
            throw error;
        }
    },

    /**
   * Associar fornecedor à empresa
   */
    async addFornecedor(empresaId: string, fornecedorId: string): Promise<void> {
        try {
            await api.post(`/Empresa/${empresaId}/fornecedores/${fornecedorId}`);
        } catch (error) {
            console.error('Erro ao adicionar fornecedor:', error);
            throw error;
        }
    },

    /**
   * Remover associação de fornecedor e empresa
   */
    async removeFornecedor(empresaId: string, fornecedorId: string): Promise<void> {
        try {
            await api.delete(`/Empresa/${empresaId}/fornecedores/${fornecedorId}`);
        } catch (error) {
            console.error('Erro ao remover fornecedor:', error);
            throw error;
        }
    },

};