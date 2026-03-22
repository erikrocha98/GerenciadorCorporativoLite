<template>
  <div>
    <!-- Cabeçalho -->
    <div class="d-flex justify-space-between align-center mb-6">
      <div>
        <h1 class="text-h4">Fornecedores</h1>
        <p class="text-subtitle-1 text-medium-emphasis">Gerencie fornecedores cadastrados</p>
      </div>
      <v-btn color="primary" prepend-icon="mdi-plus" size="large" @click="dialogNovo = true">
        Adicionar Fornecedor
      </v-btn>
    </div>

    <!-- Busca -->
    <v-card class="mb-6" elevation="1">
      <v-card-text>
        <v-row>
          <v-col cols="12" md="6">
            <v-text-field v-model="filtroNome" prepend-inner-icon="mdi-magnify" label="Buscar por nome"
              variant="outlined" density="comfortable" hide-details clearable></v-text-field>
          </v-col>
          <v-col cols="12" md="6">
            <v-text-field v-model="filtroCpfCnpj" prepend-inner-icon="mdi-card-account-details"
              label="Buscar por CPF/CNPJ" variant="outlined" density="comfortable" hide-details
              clearable></v-text-field>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <!-- Loading -->
    <div v-if="fornecedorStore.loading" class="text-center py-8">
      <v-progress-circular indeterminate color="primary" size="64"></v-progress-circular>
      <p class="text-subtitle-1 mt-4">Carregando fornecedores...</p>
    </div>

    <!-- Grid de Cards -->
    <v-row v-else>
      <v-col v-for="fornecedor in fornecedoresFiltrados" :key="fornecedor.id" cols="12" md="6" lg="4">
        <v-card elevation="2" hover>
          <v-card-text>
            <div class="d-flex align-start">
              <v-avatar color="primary" size="48" class="mr-3">
                <span class="text-h6">{{ getIniciais(fornecedor.nome) }}</span>
              </v-avatar>

              <div class="flex-grow-1">
                <div class="d-flex justify-space-between align-start">
                  <div>
                    <h3 class="text-h6">{{ fornecedor.nome }}</h3>
                    <v-chip :color="isPessoaFisica(fornecedor.cpfCnpj) ? 'blue' : 'orange'"
                      size="x-small" label class="mt-1">
                      {{ isPessoaFisica(fornecedor.cpfCnpj) ? 'Pessoa Física' : 'Pessoa Jurídica' }}
                    </v-chip>
                  </div>

                  <!-- Menu de ações -->
                  <v-menu>
                    <template v-slot:activator="{ props }">
                      <v-btn icon="mdi-dots-vertical" variant="text" size="small" v-bind="props"></v-btn>
                    </template>
                    <v-list density="compact">
                      <v-list-item prepend-icon="mdi-pencil" title="Editar"
                        @click="editarFornecedor(fornecedor)"></v-list-item>
                      <v-list-item prepend-icon="mdi-account-details" title="Ver Detalhes"
                        :to="{ name: 'FornecedoresDetalhes', params: { id: fornecedor.id } }"></v-list-item>
                      <v-divider></v-divider>
                      <v-list-item prepend-icon="mdi-delete" title="Deletar" class="text-error"
                        @click="confirmarDelete(fornecedor)"></v-list-item>
                    </v-list>
                  </v-menu>
                </div>

                <!-- Informações -->
                <div class="mt-3">
                  <div class="d-flex align-center mb-2">
                    <v-icon size="small" class="mr-2">mdi-card-account-details</v-icon>
                    <span class="text-body-2">{{ formatarDocumento(fornecedor.cpfCnpj) }}</span>
                  </div>
                  <div class="d-flex align-center mb-2">
                    <v-icon size="small" class="mr-2">mdi-email</v-icon>
                    <span class="text-body-2">{{ fornecedor.email }}</span>
                  </div>
                  <div class="d-flex align-center mb-2">
                    <v-icon size="small" class="mr-2">mdi-map-marker</v-icon>
                    <span class="text-body-2">{{ formatarCEP(fornecedor.cep) }}</span>
                  </div>
                </div>

                <!-- Footer -->
                <div class="d-flex justify-space-between align-center mt-4">
                  <div>
                    <span class="text-caption text-medium-emphasis">Empresas</span>
                    <p class="text-h6 mb-0">{{ fornecedor.empresasCount ?? 0 }}</p>
                  </div>
                  <v-chip color="success" size="small" label>
                    <v-icon start size="small">mdi-check-circle</v-icon>
                    Ativo
                  </v-chip>
                </div>

                <div v-if="fornecedor.updatedAt" class="text-caption text-medium-emphasis mt-2">
                  Última atualização: {{ formatarData(fornecedor.updatedAt) }}
                </div>
              </div>
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Mensagem quando não há fornecedores -->
    <v-card v-if="!fornecedorStore.loading && fornecedoresFiltrados.length === 0" class="text-center pa-8"
      elevation="1">
      <v-icon size="64" color="grey">mdi-account-off</v-icon>
      <h3 class="text-h6 mt-4">{{ temFiltro ? 'Nenhum fornecedor encontrado' : 'Nenhum fornecedor cadastrado' }}</h3>
      <p class="text-body-2 text-medium-emphasis">
        {{ temFiltro ? 'Tente usar outros termos de busca' : 'Comece adicionando seu primeiro fornecedor' }}
      </p>
      <v-btn v-if="!temFiltro" color="primary" class="mt-4" @click="dialogNovo = true">
        Adicionar Primeiro Fornecedor
      </v-btn>
    </v-card>

    <!-- Dialog: Novo/Editar Fornecedor -->
    <v-dialog v-model="dialogNovo" max-width="600px" persistent>
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ fornecedorEditando ? 'Editar Fornecedor' : 'Novo Fornecedor' }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="formRef" @submit.prevent="salvarFornecedor">
            <v-text-field v-model="form.cpfCnpj" label="CPF/CNPJ *" variant="outlined"
              :rules="[rules.required, rules.cpfCnpj]" @input="formatarCampoCpfCnpj" maxlength="18"
              prepend-inner-icon="mdi-card-account-details"></v-text-field>

            <v-text-field v-model="form.nome" label="Nome *" variant="outlined" :rules="[rules.required]"
              prepend-inner-icon="mdi-account"></v-text-field>

            <v-text-field v-model="form.email" label="E-mail *" variant="outlined"
              :rules="[rules.required, rules.email]" prepend-inner-icon="mdi-email"></v-text-field>

            <v-text-field v-model="form.cep" label="CEP *" variant="outlined" :rules="[rules.required, rules.cep]"
              @input="formatarCampoCEP" @blur="buscarCepFornecedor" maxlength="9"
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

            <!-- Campos exclusivos para Pessoa Física -->
            <template v-if="isPessoaFisicaForm">
              <v-text-field v-model="form.rg" label="RG *" variant="outlined" :rules="[rules.required]"
                prepend-inner-icon="mdi-card-text"></v-text-field>

              <v-text-field v-model="form.dataNascimento" label="Data de Nascimento *" variant="outlined"
                type="date" :rules="[rules.required]" prepend-inner-icon="mdi-calendar"></v-text-field>
            </template>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="fecharDialog">Cancelar</v-btn>
          <v-btn color="primary" variant="flat" @click="salvarFornecedor" :loading="salvando">Salvar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog: Confirmar Delete -->
    <v-dialog v-model="dialogDelete" max-width="500px">
      <v-card>
        <v-card-title class="text-h5">Confirmar Exclusão</v-card-title>
        <v-card-text>
          Tem certeza que deseja excluir o fornecedor <strong>{{ fornecedorDeletar?.nome }}</strong>?
          Esta ação não pode ser desfeita.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="dialogDelete = false">Cancelar</v-btn>
          <v-btn color="error" variant="flat" @click="deletarFornecedor" :loading="deletando">Excluir</v-btn>
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
import { ref, computed, onMounted } from 'vue';
import { useFornecedorStore } from '@/stores/fornecedorStore';
import { cepService } from '@/services/cepService';
import type { Fornecedor, FornecedorFormData } from '@/types';
import {
  formatarCPF,
  formatarCNPJ,
  formatarCEP,
  validarCPF,
  validarCnpj,
  validarCEP,
  validarEmail,
  apenasNumeros,
} from '@/utils/validators';

const fornecedorStore = useFornecedorStore();

// Refs
const filtroNome = ref('');
const filtroCpfCnpj = ref('');
const dialogNovo = ref(false);
const dialogDelete = ref(false);
const formRef = ref();
const salvando = ref(false);
const deletando = ref(false);
const fornecedorEditando = ref<Fornecedor | null>(null);
const fornecedorDeletar = ref<Fornecedor | null>(null);

// Snackbar
const snackbar = ref(false);
const snackbarText = ref('');
const snackbarColor = ref('success');

// Form
const form = ref<FornecedorFormData>({
  cpfCnpj: '',
  nome: '',
  email: '',
  cep: '',
  rg: '',
  dataNascimento: '',
  logradouro: '',
  bairro: '',
  cidade: '',
  uf: '',
});

// Helpers de tipo
function isPessoaFisica(cpfCnpj: string): boolean {
  return apenasNumeros(cpfCnpj).length <= 11;
}

const isPessoaFisicaForm = computed(() => isPessoaFisica(form.value.cpfCnpj));

const temFiltro = computed(() => !!filtroNome.value || !!filtroCpfCnpj.value);

// Computed: Filtrar fornecedores
const fornecedoresFiltrados = computed(() => {
  return fornecedorStore.fornecedores.filter(f => {
    const nomeOk = !filtroNome.value || f.nome.toLowerCase().includes(filtroNome.value.toLowerCase());
    const docOk = !filtroCpfCnpj.value || f.cpfCnpj.includes(apenasNumeros(filtroCpfCnpj.value));
    return nomeOk && docOk;
  });
});

// Regras de validação
const rules = {
  required: (v: string) => !!v || 'Campo obrigatório',
  cpfCnpj: (v: string) => {
    const nums = apenasNumeros(v);
    if (nums.length <= 11) return validarCPF(v) || 'CPF inválido';
    return validarCnpj(v) || 'CNPJ inválido';
  },
  cep: (v: string) => validarCEP(v) || 'CEP inválido',
  email: (v: string) => validarEmail(v) || 'E-mail inválido',
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

function formatarDocumento(cpfCnpj: string): string {
  const nums = apenasNumeros(cpfCnpj);
  return nums.length <= 11 ? formatarCPF(nums) : formatarCNPJ(nums);
}

// Formatação dos campos
function formatarCampoCpfCnpj() {
  const nums = apenasNumeros(form.value.cpfCnpj);
  if (nums.length <= 11) {
    form.value.cpfCnpj = formatarCPF(nums);
  } else {
    form.value.cpfCnpj = formatarCNPJ(nums);
  }
}

function formatarCampoCEP() {
  form.value.cep = formatarCEP(form.value.cep);
}

// Buscar CEP
async function buscarCepFornecedor() {
  if (!validarCEP(form.value.cep)) return;

  const dados = await cepService.buscarCep(form.value.cep);
  if (dados) {
    form.value.logradouro = dados.logradouro;
    form.value.bairro = dados.bairro;
    form.value.cidade = dados.cidade;
    form.value.uf = dados.uf;
  } else {
    mostrarMensagem('CEP não encontrado', 'error');
  }
}

// Salvar fornecedor
async function salvarFornecedor() {
  const { valid } = await formRef.value.validate();
  if (!valid) return;

  salvando.value = true;
  try {
    const dados: FornecedorFormData = {
      cpfCnpj: apenasNumeros(form.value.cpfCnpj),
      nome: form.value.nome,
      email: form.value.email,
      cep: apenasNumeros(form.value.cep),
      logradouro: form.value.logradouro,
      bairro: form.value.bairro,
      cidade: form.value.cidade,
      uf: form.value.uf,
      ...(isPessoaFisicaForm.value && {
        rg: form.value.rg,
        dataNascimento: form.value.dataNascimento,
      }),
    };

    if (fornecedorEditando.value) {
      await fornecedorStore.atualizarFornecedor(fornecedorEditando.value.id!, dados);
      mostrarMensagem('Fornecedor atualizado com sucesso!', 'success');
    } else {
      await fornecedorStore.criarFornecedor(dados);
      mostrarMensagem('Fornecedor criado com sucesso!', 'success');
    }

    fecharDialog();
  } catch (error: any) {
    const msg =
      error?.response?.data?.message ||
      error?.response?.data?.errors?.[0] ||
      error.message ||
      'Erro ao salvar fornecedor';
    mostrarMensagem(msg, 'error');
  } finally {
    salvando.value = false;
  }
}

// Editar fornecedor
function editarFornecedor(fornecedor: Fornecedor) {
  fornecedorEditando.value = fornecedor;
  const nums = apenasNumeros(fornecedor.cpfCnpj);
  form.value = {
    cpfCnpj: nums.length <= 11 ? formatarCPF(nums) : formatarCNPJ(nums),
    nome: fornecedor.nome,
    email: fornecedor.email,
    cep: formatarCEP(fornecedor.cep),
    logradouro: fornecedor.logradouro || '',
    bairro: fornecedor.bairro || '',
    cidade: fornecedor.cidade || '',
    uf: fornecedor.uf || '',
    rg: fornecedor.rg || '',
    dataNascimento: fornecedor.dataNascimento ? fornecedor.dataNascimento.split('T')[0] : '',
  };
  dialogNovo.value = true;
}

// Confirmar delete
function confirmarDelete(fornecedor: Fornecedor) {
  fornecedorDeletar.value = fornecedor;
  dialogDelete.value = true;
}

// Deletar fornecedor
async function deletarFornecedor() {
  if (!fornecedorDeletar.value) return;

  deletando.value = true;
  try {
    await fornecedorStore.deletarFornecedor(fornecedorDeletar.value.id!);
    mostrarMensagem('Fornecedor excluído com sucesso!', 'success');
    dialogDelete.value = false;
    fornecedorDeletar.value = null;
  } catch (error: any) {
    mostrarMensagem(error.message || 'Erro ao excluir fornecedor', 'error');
  } finally {
    deletando.value = false;
  }
}

// Fechar dialog
function fecharDialog() {
  dialogNovo.value = false;
  fornecedorEditando.value = null;
  form.value = {
    cpfCnpj: '',
    nome: '',
    email: '',
    cep: '',
    rg: '',
    dataNascimento: '',
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

// Carregar fornecedores ao montar
onMounted(async () => {
  await fornecedorStore.fetchFornecedores();
});
</script>
