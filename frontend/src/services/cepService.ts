import axios from "axios";
import type { CepResponse } from "@/types";

export const cepService = {
    /**
   * Busca dados do CEP na API viaCep
   */
    async buscarCep(cep: string): Promise<CepResponse | null> {
        try {
            //validação dupla para caso o service seja chamado diretamente.
            const cepLimpo = cep.replace(/\D/g, '');
            if (cepLimpo.length !== 8) {
                throw new Error('CEP inválido');
            }
            //trocar a url pela do viacep
            const response = await axios.get(`https://cep.la/${cepLimpo}`, {
                headers: {
                    'Accept': 'application/json',
                },
            });

            return {
                cep: response.data.cep,
                logradouro: response.data.logradouro || '',
                bairro: response.data.bairro || '',
                localidade: response.data.cidade || '',
                uf: response.data.uf || '',
            };
        } catch (error) {
            console.error('Erro ao buscar cep', error);
            return null;

        }
    }


};