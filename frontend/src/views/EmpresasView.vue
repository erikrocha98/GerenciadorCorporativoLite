<template>
  <div>
    <!-- Cabeçalho -->
    <div class="d-flex justify-space-between align-center mb-6">
      <div>
        <h1 class="text-h4">Empresas</h1>
        <p class="text-subtitle-1 text-medium-emphasis">Gerencie empresas cadastradas</p>
      </div>
      <v-btn color="primary" prepend-icon="mdi-plus" size="large" @click="dialogNova = true">
        Adicionar Empresa
      </v-btn>
    </div>

    <!-- Busca -->
    <v-card class="mb-6" elevation="1">
      <v-card-text>
        <v-text-field v-model="search" prepend-inner-icon="mdi-magnify" label="Buscar empresas" variant="outlined"
          density="comfortable" hide-details clearable></v-text-field>
      </v-card-text>
    </v-card>

    <!-- Loading -->
    <div v-if="empresaStore.loading" class="text-center py-8">
      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      <p class="text-subtitle-1 mt-4">Carregando empresas...</p>
    </div>

    <!-- Grid de Cards -->
    <v-row v-else>
      <v-col v-for="empresa in empresasFiltradas" :key="empresa.id" cols="12" md="6" lg="4">
        <v-card elevation="2" hover>
          <v-card-text>
            <!-- Avatar com iniciais -->
            <div class="d-flex align-start">
              <v-avatar color="primary" size="48" class="mr-3">
                <span class="text-h6">{{ getIniciais(empresa.nomeFantasia) }}</span>
              </v-avatar>

              <div class="flex-grow-1">
                <div class="d-flex justify-space-between align-start">
                  <div>
                    <h3 class="text-h6">{{ empresa.nomeFantasia }}</h3>
                    <p class="text-caption text-medium-emphasis">empresa</p>
                  </div>

                  <!-- Menu de ações -->
                  <v-menu>
                    <template v-slot:activator="{ props }">
                      <v-btn icon="mdi-dots-vertical" variant="text" size="small" v-bind="props"></v-btn>
                    </template>
                    <v-list density="compact">
                      <v-list-item prepend-icon="mdi-pencil" title="Editar Detalhes"
                        @click="editarEmpresa(empresa)"></v-list-item>
                      <v-list-item prepend-icon="mdi-domain" title="Gerenciar Fornecedores"
                        :to="{ name: 'EmpresasDetalhes', params: { id: empresa.id } }"></v-list-item>
                      <v-divider></v-divider>
                      <v-list-item prepend-icon="mdi-delete" title="Deletar" class="text-error"
                        @click="confirmarDelete(empresa)"></v-list-item>
                    </v-list>
                  </v-menu>
                </div>

                <!-- Informações -->
                <div class="mt-3">
                  <div class="d-flex align-center mb-2">
                    <v-icon size="small" class="mr-2">mdi-card-account-details</v-icon>
                    <span class="text-body-2">{{ formatarCNPJ(empresa.cnpj) }}</span>
                  </div>
                  <div class="d-flex align-center mb-2">
                    <v-icon size="small" class="mr-2">mdi-map-marker</v-icon>
                    <span class="text-body-2">{{ formatarCEP(empresa.cep) }}</span>
                  </div>
                </div>

                <!-- Footer: Fornecedores e Status -->
                <div class="d-flex justify-space-between align-center mt-4">
                  <div>
                    <span class="text-caption text-medium-emphasis">Fornecedores</span>
                    <p class="text-h6 mb-0">{{ empresa.fornecedores?.length || 0 }}</p>
                  </div>
                  <v-chip color="success" size="small" label>
                    <v-icon start size="small">mdi-check-circle</v-icon>
                    Ativo
                  </v-chip>
                </div>

                <!-- Última atualização -->
                <div v-if="empresa.updatedAt" class="text-caption text-medium-emphasis mt-2">
                  Última atualização: {{ formatarData(empresa.updatedAt) }}
                </div>
              </div>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Mensagem quando não há empresas -->
    <v-card v-if="!empresaStore.loading && empresasFiltradas.length === 0" class="text-center pa-8" elevation="1">
      <v-icon size="64" color="grey">mdi-domain-off</v-icon>
      <h3 class="text-h6 mt-4">{{ search ? 'Nenhuma empresa encontrada' : 'Nenhuma empresa cadastrada' }}</h3>
      <p class="text-body-2 text-medium-emphasis">
        {{ search ? 'Tente usar outros termos de busca' : 'Comece adicionando sua primeira empresa' }}
      </p>
      <v-btn v-if="!search" color="primary" class="mt-4" @click="dialogNova = true">
        Adicionar Primeira Empresa
      </v-btn>
    </v-card>

    <!-- Dialog: Nova/Editar Empresa -->
    <v-dialog v-model="dialogNova" max-width="600px" persistent>
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ empresaEditando ? 'Editar Empresa' : 'Nova Empresa' }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="formRef" @submit.prevent="salvarEmpresa">
            <v-text-field v-model="form.cnpj" label="CNPJ *" variant="outlined" :rules="[rules.required, rules.cnpj]"
              @input="formatarCampoCNPJ" maxlength="18" prepend-inner-icon="mdi-card-account-details"></v-text-field>

            <v-text-field v-model="form.nomeFantasia" label="Nome Fantasia *" variant="outlined"
              :rules="[rules.required]" prepend-inner-icon="mdi-domain"></v-text-field>

            <v-text-field v-model="form.cep" label="CEP *" variant="outlined" :rules="[rules.required, rules.cep]"
              @input="formatarCampoCEP" @blur="buscarCepEmpresa" maxlength="9"
              prepend-inner-icon="mdi-map-marker"></v-text-field>

            <v-text-field v-model="form.logradouro" label="Logradouro" variant="outlined" readonly
              prepend-inner-icon="mdi-road"></v-text-field>

            <v-text-field v-model="form.bairro" label="Bairro" variant="outlined" readonly
              prepend-inner-icon="mdi-home-group"></v-text-field>

            <v-row>
              <v-col cols="8">
                <v-text-field v-model="form.cidade" label="Cidade" variant="outlined" readonly
                  prepend-inner-icon="mdi-city"></v-text-field>
              </v-col>
              <v-col cols="4">
                <v-text-field v-model="form.uf" label="UF" variant="outlined" readonly></v-text-field>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="fecharDialog">
            Cancelar
          </v-btn>
          <v-btn color="primary" variant="flat" @click="salvarEmpresa" :loading="salvando">
            Salvar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog: Confirmar Delete -->
    <v-dialog v-model="dialogDelete" max-width="500px">
      <v-card>
        <v-card-title class="text-h5">Confirmar Exclusão</v-card-title>
        <v-card-text>
          Tem certeza que deseja excluir a empresa <strong>{{ empresaDeletar?.nomeFantasia }}</strong>?
          Esta ação não pode ser desfeita.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="dialogDelete = false">
            Cancelar
          </v-btn>
          <v-btn color="error" variant="flat" @click="deletarEmpresa" :loading="deletando">
            Excluir
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar de Feedback -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" :timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useEmpresaStore } from '@/stores/empresaStore';
import { cepService } from '@/services/cepService';
import type { Empresa, EmpresaFormData } from '@/types';
import {
  formatarCNPJ,
  formatarCEP,
  validarCnpj,
  validarCEP,
  apenasNumeros,
} from '@/utils/validators';

const empresaStore = useEmpresaStore();

// Refs
const search = ref('');
const dialogNova = ref(false);
const dialogDelete = ref(false);
const formRef = ref();
const salvando = ref(false);
const deletando = ref(false);
const empresaEditando = ref<Empresa | null>(null);
const empresaDeletar = ref<Empresa | null>(null);

// Snackbar
const snackbar = ref(false);
const snackbarText = ref('');
const snackbarColor = ref('success');

// Form
const form = ref<EmpresaFormData>({
  cnpj: '',
  nomeFantasia: '',
  cep: '',
  logradouro: '',
  bairro: '',
  cidade: '',
  uf: '',
});

// Computed: Filtrar empresas
const empresasFiltradas = computed(() => {
  if (!search.value) return empresaStore.empresas;

  const termo = search.value.toLowerCase();
  return empresaStore.empresas.filter(empresa =>
    empresa.nomeFantasia.toLowerCase().includes(termo) ||
    empresa.cnpj.includes(apenasNumeros(termo)) ||
    empresa.cidade?.toLowerCase().includes(termo) ||
    empresa.uf?.toLowerCase().includes(termo)
  );
});

// Regras de validação
const rules = {
  required: (v: string) => !!v || 'Campo obrigatório',
  cnpj: (v: string) => validarCnpj(v) || 'CNPJ inválido',
  cep: (v: string) => validarCEP(v) || 'CEP inválido',
};

// Funções auxiliares
function getIniciais(nome: string): string {
  const palavras = nome.split(' ');
  if (palavras.length >= 2) {
    return (palavras[0][0] + palavras[1][0]).toUpperCase();
  }
  return nome.substring(0, 2).toUpperCase();
}

function formatarData(data: string): string {
  return new Date(data).toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

// Funções de formatação
function formatarCampoCNPJ() {
  form.value.cnpj = formatarCNPJ(form.value.cnpj);
}

function formatarCampoCEP() {
  form.value.cep = formatarCEP(form.value.cep);
}

// Buscar CEP
async function buscarCepEmpresa() {
  if (!validarCEP(form.value.cep)) return;

  const dados = await cepService.buscarCep(form.value.cep);
  if (dados) {
    form.value.logradouro = dados.logradouro;
    form.value.bairro = dados.bairro;
    form.value.cidade = dados.localidade;
    form.value.uf = dados.uf;
  } else {
    mostrarMensagem('CEP não encontrado', 'error');
  }
}

// Salvar empresa
async function salvarEmpresa() {
  const { valid } = await formRef.value.validate();
  if (!valid) return;

  salvando.value = true;
  try {
    const dados: EmpresaFormData = {
      cnpj: apenasNumeros(form.value.cnpj),
      nomeFantasia: form.value.nomeFantasia,
      cep: apenasNumeros(form.value.cep),
      logradouro: form.value.logradouro,
      bairro: form.value.bairro,
      cidade: form.value.cidade,
      uf: form.value.uf,
    };

    if (empresaEditando.value) {
      await empresaStore.atualizarEmpresa(empresaEditando.value.id!, dados);
      mostrarMensagem('Empresa atualizada com sucesso!', 'success');
    } else {
      await empresaStore.criarEmpresa(dados);
      mostrarMensagem('Empresa criada com sucesso!', 'success');
    }

    fecharDialog();
  } catch (error: any) {
    mostrarMensagem(error.message || 'Erro ao salvar empresa', 'error');
  } finally {
    salvando.value = false;
  }
}

// Editar empresa
function editarEmpresa(empresa: Empresa) {
  empresaEditando.value = empresa;
  form.value = {
    cnpj: formatarCNPJ(empresa.cnpj),
    nomeFantasia: empresa.nomeFantasia,
    cep: formatarCEP(empresa.cep),
    logradouro: empresa.logradouro || '',
    bairro: empresa.bairro || '',
    cidade: empresa.cidade || '',
    uf: empresa.uf || '',
  };
  dialogNova.value = true;
}

// Confirmar delete
function confirmarDelete(empresa: Empresa) {
  empresaDeletar.value = empresa;
  dialogDelete.value = true;
}

// Deletar empresa
async function deletarEmpresa() {
  if (!empresaDeletar.value) return;

  deletando.value = true;
  try {
    await empresaStore.deletarEmpresa(empresaDeletar.value.id!);
    mostrarMensagem('Empresa excluída com sucesso!', 'success');
    dialogDelete.value = false;
    empresaDeletar.value = null;
  } catch (error: any) {
    mostrarMensagem(error.message || 'Erro ao excluir empresa', 'error');
  } finally {
    deletando.value = false;
  }
}

// Fechar dialog
function fecharDialog() {
  dialogNova.value = false;
  empresaEditando.value = null;
  form.value = {
    cnpj: '',
    nomeFantasia: '',
    cep: '',
    logradouro: '',
    bairro: '',
    cidade: '',
    uf: '',
  };
  formRef.value?.resetValidation();
}

// Mostrar mensagem
function mostrarMensagem(texto: string, cor: string) {
  snackbarText.value = texto;
  snackbarColor.value = cor;
  snackbar.value = true;
}

// Carregar empresas ao montar
onMounted(async () => {
  await empresaStore.fetchEmpresas();
});
</script>