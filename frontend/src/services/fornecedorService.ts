import api from './api';
import type { Fornecedor, FornecedorFormData, Empresa } from '@/types';

export const fornecedorService = {
  /**
   * Buscar todos os fornecedores
   */
  async getAll(): Promise<Fornecedor[]> {
    try {
      const response = await api.get<Fornecedor[]>('/Fornecedor');
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar fornecedores:', error);
      throw error;
    }
  },

  /**
   * Buscar fornecedor por ID
   */
  async getById(id: string): Promise<Fornecedor> {
    try {
      const response = await api.get<Fornecedor>(`/Fornecedor/${id}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar fornecedor:', error);
      throw error;
    }
  },

  /**
   * Criar novo fornecedor
   */
  async create(fornecedor: FornecedorFormData): Promise<Fornecedor> {
    try {
      const response = await api.post<Fornecedor>('/Fornecedor', fornecedor);
      return response.data;
    } catch (error) {
      console.error('Erro ao criar fornecedor:', error);
      throw error;
    }
  },

  /**
   * Atualizar fornecedor
   */
  async update(id: string, fornecedor: FornecedorFormData): Promise<Fornecedor> {
    try {
      const response = await api.put<Fornecedor>(`/Fornecedor/${id}`, fornecedor);
      return response.data;
    } catch (error) {
      console.error('Erro ao atualizar fornecedor:', error);
      throw error;
    }
  },

  /**
   * Deletar fornecedor
   */
  async delete(id: string): Promise<void> {
    try {
      await api.delete(`/Fornecedor/${id}`);
    } catch (error) {
      console.error('Erro ao deletar fornecedor:', error);
      throw error;
    }
  },

  /**
   * Buscar fornecedores com filtros
   */
  async search(nome?: string, cpfCnpj?: string): Promise<Fornecedor[]> {
    try {
      const params = new URLSearchParams();
      if (nome) params.append('nome', nome);
      if (cpfCnpj) params.append('cpfCnpj', cpfCnpj);

      const response = await api.get<Fornecedor[]>(`/Fornecedor/search?${params.toString()}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar fornecedores:', error);
      throw error;
    }
  },

  /**
   * Buscar empresas de um fornecedor
   */
  async getEmpresas(id: string): Promise<Empresa[]> {
    try {
      const response = await api.get<Empresa[]>(`/Fornecedor/${id}/empresas`);
      return response.data;
    } catch (error) {
      console.error('Erro ao buscar empresas do fornecedor:', error);
      throw error;
    }
  },
};