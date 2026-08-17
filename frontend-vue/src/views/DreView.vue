<script setup lang="ts">
import { ref, watch } from "vue";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { gerencialApi } from "../api/modules";

const cliforId = ref<string>();
const data = ref<any[]>([]);
const loading = ref(false);

watch(cliforId, async () => {
  if (!cliforId.value) { data.value = []; return; }
  loading.value = true;
  try {
    data.value = await gerencialApi.dre(cliforId.value);
  } finally {
    loading.value = false;
  }
});

const columns = [
  { title: "Grupo", dataIndex: "grupo" },
  { title: "Valor", dataIndex: "valorTotal", key: "valorTotal", align: "right" as const },
];
</script>

<template>
  <a-card>
    <template #title>DRE — Demonstração do Resultado do Exercício</template>
    <template #extra><EmpresaSelect v-model="cliforId" /></template>

    <a-table row-key="ordem" :loading="loading" :data-source="data" :columns="columns" :pagination="false">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'valorTotal'">
          {{ Number(record.valorTotal).toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}
        </template>
      </template>
    </a-table>
  </a-card>
</template>
