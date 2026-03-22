import api from './api';
import type { CepResponse } from "@/types";

export const cepService = {
    async buscarCep(cep: string): Promise<CepResponse | null> {
        try {
            const cepLimpo = cep.replace(/\D/g, '');
            if (cepLimpo.length !== 8) {
                throw new Error('CEP inválido');
            }

            const response = await api.get<CepResponse>(`/Cep/${cepLimpo}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao buscar cep', error);
            return null;
        }
    }
};