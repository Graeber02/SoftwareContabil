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
    data.value = await gerencialApi.balancete(cliforId.value);
  } finally {
    loading.value = false;
  }
});

const columns = [
  { title: "Conta", dataIndex: "descricao" },
  { title: "Hierarquia", dataIndex: "hierarquia" },
  { title: "Saldo", dataIndex: "saldo", key: "saldo", align: "right" as const },
];
</script>

<template>
  <a-card>
    <template #title>Balancete de Verificação</template>
    <template #extra><EmpresaSelect v-model="cliforId" /></template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns" :pagination="{ pageSize: 20 }">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'saldo'">
          {{ Number(record.saldo).toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}
        </template>
      </template>
    </a-table>
  </a-card>
</template>
