<template>
  <v-app >
    <!-- App Bar (Header) -->
    <v-app-bar color="primary" prominent>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title class="d-flex align-center">
        <v-icon icon="mdi-domain" class="mr-2"></v-icon>
        <span class="font-weight-bold">Gestão de Empresas e Fornecedores</span>
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <!-- Toggle de Tema -->
      <v-btn icon @click="themeStore.toggleTheme()">
        <v-icon>
          {{ themeStore.isDark ? 'mdi-weather-sunny' : 'mdi-weather-night' }}
        </v-icon>
      </v-btn>

      <!-- User Menu -->
      <v-btn icon>
        <v-icon>mdi-account-circle</v-icon>
      </v-btn>
    </v-app-bar>

    <!-- Navigation Drawer (Sidebar) -->
    <v-navigation-drawer v-model="drawer" temporary>
      <v-list>
        <!-- Header do Menu -->
        <v-list-item prepend-icon="mdi-view-dashboard" title="Menu teste" subtitle="Navegação"></v-list-item>
      </v-list>

      <v-divider></v-divider>

      <!-- Itens do Menu -->
      <v-list density="compact" nav>
        <v-list-item prepend-icon="mdi-view-dashboard" title="Dashboard" value="dashboard"
          :to="{ name: 'Dashboard' }"></v-list-item>

        <v-list-item prepend-icon="mdi-domain" title="Empresas" value="empresas"
          :to="{ name: 'Empresas' }"></v-list-item>

        <v-list-item prepend-icon="mdi-account-multiple" title="Fornecedores" value="fornecedores"
          :to="{ name: 'Fornecedores' }"></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Main Content -->
    <v-main>
      <v-container fluid>
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useThemeStore } from '@/stores/themeStore';

const drawer = ref(false);
const themeStore = useThemeStore();
</script>

<style scoped>
/* Para adicionar estilos customizados posteriormente se necessário */
</style>