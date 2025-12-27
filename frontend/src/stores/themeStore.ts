import { defineStore } from 'pinia';
import { ref, watch } from 'vue';
import { useTheme } from 'vuetify';

export const useThemeStore = defineStore('theme', () => {
  const vuetifyTheme = useTheme();
  
  // Carregar preferência salva ou usar 'light' como padrão
  const isDark = ref(localStorage.getItem('theme') === 'dark');

  // Aplicar tema inicial
  vuetifyTheme.global.name.value = isDark.value ? 'dark' : 'light';

  // Watch para salvar preferência e aplicar tema
  watch(isDark, (newValue) => {
    vuetifyTheme.global.name.value = newValue ? 'dark' : 'light';
    localStorage.setItem('theme', newValue ? 'dark' : 'light');
  });

  function toggleTheme() {
    isDark.value = !isDark.value;
  }

  return {
    isDark,
    toggleTheme,
  };
});