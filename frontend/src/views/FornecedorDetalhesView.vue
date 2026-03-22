<template>
  <div>
    <!-- Loading -->
    <div v-if="fornecedorStore.loading" class="text-center py-8">
      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      <p class="text-subtitle-1 mt-4">Carregando detalhes...</p>
    </div>

    <div v-else-if="fornecedorStore.fornecedorAtual">
      <!-- Cabeçalho -->
      <div class="d-flex align-center mb-6">
        <v-btn icon="mdi-arrow-left" variant="text" :to="{ name: 'Fornecedores' }"></v-btn>
        <div class="ml-4">
          <h1 class="text-h4">{{ fornecedorStore.fornecedorAtual.nome }}</h1>
          <p class="text-subtitle-1 text-medium-emphasis">Detalhes do fornecedor</p>
        </div>
      </div>

      <v-row>
        <!-- Card: Informações do Fornecedor -->
        <v-col cols="12" md="5">
          <v-card elevation="2">
            <v-card-title>
              <v-icon class="mr-2">mdi-information</v-icon>
              Informações do Fornecedor
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text>
              <v-list density="comfortable">
                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-account-badge</v-icon>
                  </template>
                  <v-list-item-title>Tipo</v-list-item-title>
                  <v-list-item-subtitle>
                    <v-chip
                      :color="isPessoaFisica ? 'blue' : 'orange'"
                      size="x-small"
                      label
                    >
                      {{ isPessoaFisica ? 'Pessoa Física' : 'Pessoa Jurídica' }}
                    </v-chip>
                  </v-list-item-subtitle>
                </v-list-item>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-card-account-details</v-icon>
                  </template>
                  <v-list-item-title>{{ isPessoaFisica ? 'CPF' : 'CNPJ' }}</v-list-item-title>
                  <v-list-item-subtitle>{{ formatarDocumento(fornecedorStore.fornecedorAtual.cpfCnpj) }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-account</v-icon>
                  </template>
                  <v-list-item-title>Nome</v-list-item-title>
                  <v-list-item-subtitle>{{ fornecedorStore.fornecedorAtual.nome }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-email</v-icon>
                  </template>
                  <v-list-item-title>E-mail</v-list-item-title>
                  <v-list-item-subtitle>{{ fornecedorStore.fornecedorAtual.email }}</v-list-item-subtitle>
                </v-list-item>

                <!-- Campos exclusivos de Pessoa Física -->
                <template v-if="isPessoaFisica">
                  <v-list-item v-if="fornecedorStore.fornecedorAtual.rg">
                    <template v-slot:prepend>
                      <v-icon>mdi-card-text</v-icon>
                    </template>
                    <v-list-item-title>RG</v-list-item-title>
                    <v-list-item-subtitle>{{ fornecedorStore.fornecedorAtual.rg }}</v-list-item-subtitle>
                  </v-list-item>

                  <v-list-item v-if="fornecedorStore.fornecedorAtual.dataNascimento">
                    <template v-slot:prepend>
                      <v-icon>mdi-calendar</v-icon>
                    </template>
                    <v-list-item-title>Data de Nascimento</v-list-item-title>
                    <v-list-item-subtitle>
                      {{ formatarData(fornecedorStore.fornecedorAtual.dataNascimento) }}
                      ({{ calcularIdade(fornecedorStore.fornecedorAtual.dataNascimento) }} anos)
                    </v-list-item-subtitle>
                  </v-list-item>
                </template>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-map-marker</v-icon>
                  </template>
                  <v-list-item-title>CEP</v-list-item-title>
                  <v-list-item-subtitle>{{ formatarCEP(fornecedorStore.fornecedorAtual.cep) }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="fornecedorStore.fornecedorAtual.logradouro">
                  <template v-slot:prepend>
                    <v-icon>mdi-road</v-icon>
                  </template>
                  <v-list-item-title>Logradouro</v-list-item-title>
                  <v-list-item-subtitle>{{ fornecedorStore.fornecedorAtual.logradouro }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="fornecedorStore.fornecedorAtual.bairro">
                  <template v-slot:prepend>
                    <v-icon>mdi-home-group</v-icon>
                  </template>
                  <v-list-item-title>Bairro</v-list-item-title>
                  <v-list-item-subtitle>{{ fornecedorStore.fornecedorAtual.bairro }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="fornecedorStore.fornecedorAtual.cidade">
                  <template v-slot:prepend>
                    <v-icon>mdi-city</v-icon>
                  </template>
                  <v-list-item-title>Cidade / UF</v-list-item-title>
                  <v-list-item-subtitle>
                    {{ fornecedorStore.fornecedorAtual.cidade }} - {{ fornecedorStore.fornecedorAtual.uf }}
                  </v-list-item-subtitle>
                </v-list-item>
              </v-list>
            </v-card-text>
          </v-card>
        </v-col>

        <!-- Card: Empresas Vinculadas -->
        <v-col cols="12" md="7">
          <v-card elevation="2">
            <v-card-title>
              <v-icon class="mr-2">mdi-domain</v-icon>
              Empresas Vinculadas
            </v-card-title>
            <v-divider></v-divider>

            <!-- Lista de Empresas -->
            <v-card-text v-if="empresasVinculadas.length > 0">
              <v-list>
                <v-list-item
                  v-for="empresa in empresasVinculadas"
                  :key="empresa.id"
                  :to="{ name: 'EmpresasDetalhes', params: { id: empresa.id } }"
                >
                  <template v-slot:prepend>
                    <v-avatar color="primary">
                      <span>{{ getIniciais(empresa.nomeFantasia) }}</span>
                    </v-avatar>
                  </template>

                  <v-list-item-title>{{ empresa.nomeFantasia }}</v-list-item-title>
                  <v-list-item-subtitle>{{ formatarCNPJ(empresa.cnpj) }}</v-list-item-subtitle>
                </v-list-item>
              </v-list>
            </v-card-text>

            <!-- Mensagem quando não há empresas -->
            <v-card-text v-else class="text-center py-8">
              <v-icon size="64" color="grey">mdi-domain-off</v-icon>
              <p class="text-subtitle-1 mt-4">Nenhuma empresa vinculada</p>
              <p class="text-body-2 text-medium-emphasis">
                Vincule este fornecedor a uma empresa pela tela de detalhes da empresa.
              </p>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </div>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" :timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { useFornecedorStore } from '@/stores/fornecedorStore';
import { fornecedorService } from '@/services/fornecedorService';
import type { Empresa } from '@/types';
import {
  formatarCPF,
  formatarCNPJ,
  formatarCEP,
  calcularIdade,
} from '@/utils/validators';

const route = useRoute();
const fornecedorStore = useFornecedorStore();

const empresasVinculadas = ref<Empresa[]>([]);

const snackbar = ref(false);
const snackbarText = ref('');
const snackbarColor = ref('success');

const isPessoaFisica = computed(() => {
  const cpfCnpj = fornecedorStore.fornecedorAtual?.cpfCnpj ?? '';
  return cpfCnpj.replace(/\D/g, '').length === 11;
});

function formatarDocumento(cpfCnpj: string): string {
  const numeros = cpfCnpj.replace(/\D/g, '');
  return numeros.length === 11 ? formatarCPF(cpfCnpj) : formatarCNPJ(cpfCnpj);
}

function formatarData(data: string): string {
  const date = new Date(data);
  return date.toLocaleDateString('pt-BR', { timeZone: 'UTC' });
}

function getIniciais(nome: string): string {
  const palavras = nome.split(' ');
  if (palavras.length >= 2) {
    return (palavras[0][0] + palavras[1][0]).toUpperCase();
  }
  return nome.substring(0, 2).toUpperCase();
}

onMounted(async () => {
  const id = route.params.id as string;
  try {
    await fornecedorStore.fetchFornecedorById(id);
    empresasVinculadas.value = await fornecedorService.getEmpresas(id);
  } catch {
    snackbarText.value = 'Erro ao carregar dados do fornecedor';
    snackbarColor.value = 'error';
    snackbar.value = true;
  }
});
</script>
