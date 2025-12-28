<template>
  <div>
    <h1 class="text-h4 mb-6">Dashboard</h1>

    <!-- Cards de Estatísticas -->
    <v-row>
      <!-- Card: Total de Empresas -->
      <v-col cols="12" md="4">
        <v-card elevation="2" color="primary" dark>
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <div class="text-overline mb-1">Total de Empresas</div>
                <div class="text-h3 font-weight-bold">
                  {{ empresaStore.loading ? '...' : empresaStore.empresas.length }}
                </div>
              </div>
              <v-icon size="64" class="opacity-50">mdi-domain</v-icon>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" :to="{ name: 'Empresas' }">
              Ver empresas
              <v-icon end>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <!-- Card: Total de Fornecedores -->
      <v-col cols="12" md="4">
        <v-card elevation="2" color="success" dark>
          <v-card-text>
            <div class="d-flex align-center justify-space-between">
              <div>
                <div class="text-overline mb-1">Total de Fornecedores</div>
                <div class="text-h3 font-weight-bold">
                  {{ fornecedorStore.loading ? '...' : fornecedorStore.fornecedores.length }}
                </div>
              </div>
              <v-icon size="64" class="opacity-50">mdi-account-multiple</v-icon>
            </div>
          </v-card-text>
          <v-card-actions>
            <v-btn variant="text" :to="{ name: 'Fornecedores' }">
              Ver fornecedores
              <v-icon end>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <!-- Card: Ações Rápidas -->
      <v-col cols="12" md="4">
        <v-card elevation="2">
          <v-card-title>
            <v-icon class="mr-2">mdi-lightning-bolt</v-icon>
            Ações Rápidas
          </v-card-title>
          <v-card-text>
            <v-list density="compact">
              <v-list-item
                prepend-icon="mdi-plus-circle"
                title="Nova Empresa"
                :to="{ name: 'Empresas' }"
              ></v-list-item>
              <v-list-item
                prepend-icon="mdi-plus-circle"
                title="Novo Fornecedor"
                :to="{ name: 'NovoFornecedor' }"
              ></v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Tabela de Empresas Recentes -->
    <v-row class="mt-4">
      <v-col cols="12">
        <v-card elevation="2">
          <v-card-title>
            <v-icon class="mr-2">mdi-clock-outline</v-icon>
            Empresas Recentes
          </v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="empresasRecentes"
              :loading="empresaStore.loading"
              items-per-page="5"
              no-data-text="Nenhuma empresa cadastrada"
            >
              <template v-slot:item.cnpj="{ item }">
                {{ formatarCNPJ(item.cnpj) }}
              </template>
              <template v-slot:item.cep="{ item }">
                {{ formatarCEP(item.cep) }}
              </template>
              <template v-slot:item.actions="{ item }">
                <v-btn
                  icon="mdi-eye"
                  size="small"
                  variant="text"
                  :to="{ name: 'EmpresasDetalhes', params: { id: item.id } }"
                ></v-btn>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useEmpresaStore } from '@/stores/empresaStore';
import { useFornecedorStore } from '@/stores/fornecedorStore';
import { formatarCNPJ, formatarCEP } from '@/utils/validators';

const empresaStore = useEmpresaStore();
const fornecedorStore = useFornecedorStore();

const headers = [
  { title: 'CNPJ', key: 'cnpj', sortable: true },
  { title: 'Nome Fantasia', key: 'nomeFantasia', sortable: true },
  { title: 'CEP', key: 'cep', sortable: false },
  { title: 'Cidade', key: 'cidade', sortable: true },
  { title: 'UF', key: 'uf', sortable: true },
  { title: 'Ações', key: 'actions', sortable: false, align: 'end' },
] as const;

// Pegar as 5 empresas mais recentes
const empresasRecentes = computed(() => {
  return empresaStore.empresas.slice(0, 5);
});

onMounted(async () => {
  await empresaStore.fetchEmpresas();
  await fornecedorStore.fetchFornecedores();
});
</script>

<style scoped>
.opacity-50 {
  opacity: 0.5;
}
</style>