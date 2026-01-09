<template>
  <div>
    <!-- Loading -->
    <div v-if="empresaStore.loading" class="text-center py-8">
      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      <p class="text-subtitle-1 mt-4">Carregando detalhes...</p>
    </div>

    <div v-else-if="empresaStore.empresaAtual">
      <!-- Cabeçalho -->
      <div class="d-flex align-center mb-6">
        <v-btn icon="mdi-arrow-left" variant="text" :to="{ name: 'Empresas' }"></v-btn>
        <div class="ml-4">
          <h1 class="text-h4">{{ empresaStore.empresaAtual.nomeFantasia }}</h1>
          <p class="text-subtitle-1 text-medium-emphasis">Detalhes da empresa</p>
        </div>
      </div>

      <v-row>
        <!-- Card: Informações da Empresa -->
        <v-col cols="12" md="5">
          <v-card elevation="2">
            <v-card-title>
              <v-icon class="mr-2">mdi-information</v-icon>
              Informações da Empresa
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text>
              <v-list density="comfortable">
                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-card-account-details</v-icon>
                  </template>
                  <v-list-item-title>CNPJ</v-list-item-title>
                  <v-list-item-subtitle>{{ formatarCNPJ(empresaStore.empresaAtual.cnpj) }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-domain</v-icon>
                  </template>
                  <v-list-item-title>Nome Fantasia</v-list-item-title>
                  <v-list-item-subtitle>{{ empresaStore.empresaAtual.nomeFantasia }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item>
                  <template v-slot:prepend>
                    <v-icon>mdi-map-marker</v-icon>
                  </template>
                  <v-list-item-title>CEP</v-list-item-title>
                  <v-list-item-subtitle>{{ formatarCEP(empresaStore.empresaAtual.cep) }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="empresaStore.empresaAtual.logradouro">
                  <template v-slot:prepend>
                    <v-icon>mdi-road</v-icon>
                  </template>
                  <v-list-item-title>Logradouro</v-list-item-title>
                  <v-list-item-subtitle>{{ empresaStore.empresaAtual.logradouro }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="empresaStore.empresaAtual.bairro">
                  <template v-slot:prepend>
                    <v-icon>mdi-home-group</v-icon>
                  </template>
                  <v-list-item-title>Bairro</v-list-item-title>
                  <v-list-item-subtitle>{{ empresaStore.empresaAtual.bairro }}</v-list-item-subtitle>
                </v-list-item>

                <v-list-item v-if="empresaStore.empresaAtual.cidade">
                  <template v-slot:prepend>
                    <v-icon>mdi-city</v-icon>
                  </template>
                  <v-list-item-title>Cidade / UF</v-list-item-title>
                  <v-list-item-subtitle>
                    {{ empresaStore.empresaAtual.cidade }} - {{ empresaStore.empresaAtual.uf }}
                  </v-list-item-subtitle>
                </v-list-item>
              </v-list>
            </v-card-text>
          </v-card>
        </v-col>

        <!-- Card: Fornecedores -->
        <v-col cols="12" md="7">
          <v-card elevation="2">
            <v-card-title class="d-flex justify-space-between align-center">
              <div>
                <v-icon class="mr-2">mdi-account-multiple</v-icon>
                Fornecedores Vinculados
              </div>
              <v-btn color="primary" size="small" prepend-icon="mdi-plus" @click="dialogAdd = true">
                Adicionar
              </v-btn>
            </v-card-title>
            <v-divider></v-divider>

            <!-- Lista de Fornecedores -->
            <v-card-text v-if="fornecedoresVinculados.length > 0">
              <v-list>
                <v-list-item v-for="fornecedor in fornecedoresVinculados" :key="fornecedor.id"
                  :to="{ name: 'FornecedoresDetalhes', params: { id: fornecedor.id } }">
                  <template v-slot:prepend>
                    <v-avatar color="success">
                      <span>{{ getIniciais(fornecedor.nome) }}</span>
                    </v-avatar>
                  </template>

                  <v-list-item-title>{{ fornecedor.nome }}</v-list-item-title>
                  <v-list-item-subtitle>
                    {{ fornecedor.cpfCnpj.length === 11 ? formatarCPF(fornecedor.cpfCnpj) :
                      formatarCNPJ(fornecedor.cpfCnpj) }}
                  </v-list-item-subtitle>

                  <template v-slot:append>
                    <v-btn icon="mdi-delete" size="small" variant="text" color="error"
                      @click.prevent="confirmarRemover(fornecedor)"></v-btn>
                  </template>
                </v-list-item>
              </v-list>
            </v-card-text>

            <!-- Mensagem quando não há fornecedores -->
            <v-card-text v-else class="text-center py-8">
              <v-icon size="64" color="grey">mdi-account-off</v-icon>
              <p class="text-subtitle-1 mt-4">Nenhum fornecedor vinculado</p>
              <v-btn color="primary" class="mt-2" @click="dialogAdd = true">
                Adicionar Primeiro Fornecedor
              </v-btn>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </div>

    <!-- Dialog: Adicionar Fornecedor -->
    <v-dialog v-model="dialogAdd" max-width="600px">
      <v-card>
        <v-card-title>Adicionar Fornecedor</v-card-title>
        <v-card-text>
          <v-text-field v-model="searchFornecedor" prepend-inner-icon="mdi-magnify" label="Buscar fornecedor"
            variant="outlined" density="comfortable" hide-details clearable class="mb-4"></v-text-field>

          <v-list v-if="fornecedoresDisponiveis.length > 0" max-height="400" class="overflow-y-auto">
            <v-list-item v-for="fornecedor in fornecedoresDisponiveisFiltrados" :key="fornecedor.id"
              @click="adicionarFornecedor(fornecedor.id!)">
              <template v-slot:prepend>
                <v-avatar color="success">
                  <span>{{ getIniciais(fornecedor.nome) }}</span>
                </v-avatar>
              </template>

              <v-list-item-title>{{ fornecedor.nome }}</v-list-item-title>
              <v-list-item-subtitle>
                {{ fornecedor.cpfCnpj.length === 11 ? formatarCPF(fornecedor.cpfCnpj) : formatarCNPJ(fornecedor.cpfCnpj)
                }}
              </v-list-item-subtitle>

              <template v-slot:append>
                <v-icon>mdi-plus-circle</v-icon>
              </template>
            </v-list-item>
          </v-list>

          <div v-else class="text-center py-8">
            <v-icon size="64" color="grey">mdi-account-off</v-icon>
            <p class="text-subtitle-1 mt-4">Todos os fornecedores já estão vinculados</p>
          </div>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="dialogAdd = false">
            Fechar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog: Confirmar Remoção -->
    <v-dialog v-model="dialogRemove" max-width="500px">
      <v-card>
        <v-card-title class="text-h5">Remover Fornecedor</v-card-title>
        <v-card-text>
          Tem certeza que deseja remover o fornecedor <strong>{{ fornecedorRemover?.nome }}</strong> desta empresa?
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="dialogRemove = false">
            Cancelar
          </v-btn>
          <v-btn color="error" variant="flat" @click="removerFornecedor" :loading="removendo">
            Remover
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" :timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute } from 'vue-router';
import { useEmpresaStore } from '@/stores/empresaStore';
import { useFornecedorStore } from '@/stores/fornecedorStore';
import type { Fornecedor } from '@/types';
import {
  formatarCNPJ,
  formatarCPF,
  formatarCEP,
  apenasNumeros,
} from '@/utils/validators';

const route = useRoute();
const empresaStore = useEmpresaStore();
const fornecedorStore = useFornecedorStore();

// Refs
const dialogAdd = ref(false);
const dialogRemove = ref(false);
const searchFornecedor = ref('');
const fornecedorRemover = ref<Fornecedor | null>(null);
const removendo = ref(false);

// Snackbar
const snackbar = ref(false);
const snackbarText = ref('');
const snackbarColor = ref('success');

// Computed: Fornecedores vinculados à empresa
const fornecedoresVinculados = computed(() => {
  if (!empresaStore.empresaAtual?.fornecedores) return [];
  return empresaStore.empresaAtual.fornecedores;
});

// Computed: Fornecedores disponíveis (não vinculados)
const fornecedoresDisponiveis = computed(() => {
  const vinculadosIds = fornecedoresVinculados.value.map(f => f.id);
  return fornecedorStore.fornecedores.filter(f => !vinculadosIds.includes(f.id));
});

// Computed: Fornecedores disponíveis filtrados
const fornecedoresDisponiveisFiltrados = computed(() => {
  if (!searchFornecedor.value) return fornecedoresDisponiveis.value;

  const termo = searchFornecedor.value.toLowerCase();
  return fornecedoresDisponiveis.value.filter(f =>
    f.nome.toLowerCase().includes(termo) ||
    f.cpfCnpj.includes(apenasNumeros(termo)) ||
    f.email.toLowerCase().includes(termo)
  );
});

// Funções auxiliares
function getIniciais(nome: string): string {
  const palavras = nome.split(' ');
  if (palavras.length >= 2) {
    return (palavras[0][0] + palavras[1][0]).toUpperCase();
  }
  return nome.substring(0, 2).toUpperCase();
}

// Adicionar fornecedor
async function adicionarFornecedor(fornecedorId: number) {
  try {
    await empresaStore.adicionarFornecedor(empresaStore.empresaAtual!.id!, fornecedorId);
    mostrarMensagem('Fornecedor adicionado com sucesso!', 'success');
    dialogAdd.value = false;
    searchFornecedor.value = '';
  } catch (error: any) {
    mostrarMensagem(error.message || 'Erro ao adicionar fornecedor', 'error');
  }
}

// Confirmar remoção
function confirmarRemover(fornecedor: Fornecedor) {
  fornecedorRemover.value = fornecedor;
  dialogRemove.value = true;
}

// Remover fornecedor
async function removerFornecedor() {
  if (!fornecedorRemover.value) return;

  removendo.value = true;
  try {
    await empresaStore.removerFornecedor(
      empresaStore.empresaAtual!.id!,
      fornecedorRemover.value.id!
    );
    mostrarMensagem('Fornecedor removido com sucesso!', 'success');
    dialogRemove.value = false;
    fornecedorRemover.value = null;
  } catch (error: any) {
    mostrarMensagem(error.message || 'Erro ao remover fornecedor', 'error');
  } finally {
    removendo.value = false;
  }
}

// Mostrar mensagem
function mostrarMensagem(texto: string, cor: string) {
  snackbarText.value = texto;
  snackbarColor.value = cor;
  snackbar.value = true;
}

// Carregar dados ao montar
onMounted(async () => {
  const id = route.params.id as string;
  await empresaStore.fetchEmpresaById(id);
  await fornecedorStore.fetchFornecedores();
});
</script>