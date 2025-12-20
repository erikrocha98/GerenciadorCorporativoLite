<template>
  <v-app>
    <!-- Menu Lateral (Drawer) -->
    <v-navigation-drawer
      v-model="drawer"
      app
      :rail="rail"
      @click="rail = false"
      color="surface"
    >
      <!-- Logo/Título -->
      <v-list-item
        prepend-icon="mdi-office-building"
        :title="rail ? '' : 'Gerenciador Corporativo'"
        nav
      >
        <template v-slot:append>
          <v-btn
            icon
            variant="text"
            @click.stop="rail = !rail"
          >
            <v-icon>{{ rail ? 'mdi-chevron-right' : 'mdi-chevron-left' }}</v-icon>
          </v-btn>
        </template>
      </v-list-item>

      <v-divider></v-divider>

      <!-- Menu Items -->
      <v-list density="compact" nav color="primary">
        <v-list-item
          prepend-icon="mdi-view-dashboard"
          title="Dashboard"
          value="dashboard"
          :to="{ name: 'Dashboard' }"
          rounded="xl"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-office-building"
          title="Empresas"
          value="empresas"
          :to="{ name: 'Empresas' }"
          rounded="xl"
        ></v-list-item>

        <v-list-item
          prepend-icon="mdi-account-multiple"
          title="Fornecedores"
          value="fornecedores"
          :to="{ name: 'Fornecedores' }"
          rounded="xl"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- App Bar -->
    <v-app-bar color="primary" prominent>
      <v-app-bar-nav-icon 
        @click="drawer = !drawer"
        color="white"
      ></v-app-bar-nav-icon>

      <v-toolbar-title class="text-white">
        Sistema Gerenciador Corporativo
      </v-toolbar-title>

      <v-spacer></v-spacer>

      <!-- Botão de tema claro/escuro -->
      <v-btn icon @click="toggleTheme" color="white">
        <v-icon>{{ theme.global.current.value.dark ? 'mdi-weather-sunny' : 'mdi-weather-night' }}</v-icon>
      </v-btn>
    </v-app-bar>

    <!-- Conteúdo Principal -->
    <v-main style="min-height: 100vh;">
      <router-view />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useTheme } from 'vuetify'

const drawer = ref(true)
const rail = ref(false)
const theme = useTheme()

const toggleTheme = () => {
  theme.global.name.value = theme.global.current.value.dark ? 'light' : 'dark'
}
</script>