import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
    {
        path: '/',
        name: 'Dashboard',
        component: () => import('@/views/DashboardView.vue')
    },

    {
        path: '/empresas',
        name: 'Empresas',
        component: () => import('@/views/EmpresasView.vue')
    },
    {
        path: '/empresas/:id',
        name: 'EmpresasDetalhes',
        component: () => import('@/views/EmpresaDetalhesView.vue')
    },
    {
        path: '/fornecedores',
        name: 'Fornecedores',
        component: () => import('@/views/FornecedoresView.vue')
    },
    {
        path: '/fornecedores/:id',
        name: 'FornecedoresDetalhes',
        component: () => import('@/views/FornecedorDetalhesView.vue')
    },
    {
        path: '/fornecedores/novo',
        name: 'NovoFornecedor',
        component: () => import('@/views/FornecedorFormView.vue')
    },
    {
        path: '/fornecedores/editar/:id',
        name: 'EditarFornecedor',
        component: () => import('@/views/FornecedorFormView.vue')
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router;
