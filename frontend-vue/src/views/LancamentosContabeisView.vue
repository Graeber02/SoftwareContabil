<script setup lang="ts">
import { ref, watch, onMounted } from "vue";
import { message } from "ant-design-vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import dayjs, { Dayjs } from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { contaApi, lancamentoContabilApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const centroCustoApi = createCrudApi("centroCusto");

const cliforId = ref<string>();
const data = ref<any[]>([]);
const contas = ref<any[]>([]);
const centrosCusto = ref<any[]>([]);
const drawerOpen = ref(false);
const loading = ref(false);

const formIdconta = ref<number>();
const formCentrocustoid = ref<number>();
const formTipo = ref<"D" | "C">("D");
const formValor = ref<number>();
const formDatahora = ref<Dayjs>(dayjs());
const formHistorico = ref("");

onMounted(() => {
  centroCustoApi.list().then((l: any[]) => (centrosCusto.value = l));
});

async function reload() {
  if (!cliforId.value) { data.value = []; return; }
  loading.value = true;
  try {
    const [lancamentos, contasList] = await Promise.all([lancamentoContabilApi.list(), contaApi.list()]);
    data.value = lancamentos.filter((l: any) => l.cliforid === cliforId.value);
    contas.value = contasList.filter((c: any) => c.cliforid === cliforId.value && !c.sintetica);
  } finally {
    loading.value = false;
  }
}
watch(cliforId, reload);

function openNew() {
  formIdconta.value = undefined;
  formCentrocustoid.value = undefined;
  formTipo.value = "D";
  formValor.value = undefined;
  formDatahora.value = dayjs();
  formHistorico.value = "";
  drawerOpen.value = true;
}

async function handleSubmit() {
  if (!cliforId.value) return;
  try {
    await lancamentoContabilApi.create({
      idconta: formIdconta.value,
      centrocustoid: formCentrocustoid.value,
      tipo: formTipo.value,
      valor: formValor.value,
      datahora: formDatahora.value.toISOString(),
      historico: formHistorico.value,
      cliforid: cliforId.value,
    });
    message.success("Lançamento registrado.");
    drawerOpen.value = false;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível registrar o lançamento.");
  }
}

const columns = [
  { title: "Data", dataIndex: "datahora" },
  { title: "Histórico", dataIndex: "historico" },
  { title: "Tipo", dataIndex: "tipo", key: "tipo" },
  { title: "Valor", dataIndex: "valor", key: "valor" },
];
</script>

<template>
  <a-card>
    <template #title>Lançamentos Contábeis</template>
    <template #extra>
      <a-space>
        <EmpresaSelect v-model="cliforId" />
        <a-button type="primary" :disabled="!cliforId" @click="openNew">
          <template #icon><plus-outlined /></template>
          Novo lançamento
        </a-button>
      </a-space>
    </template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns">
      <template #bodyCell="{ column, record, text }">
        <template v-if="column.dataIndex === 'datahora'">{{ dayjs(text).format("DD/MM/YYYY HH:mm") }}</template>
        <template v-else-if="column.key === 'tipo'">
          <a-tag :color="record.tipo === 'D' ? 'red' : 'green'">{{ record.tipo === "D" ? "Débito" : "Crédito" }}</a-tag>
        </template>
        <template v-else-if="column.key === 'valor'">
          {{ record.valor?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}
        </template>
      </template>
    </a-table>

    <a-drawer v-model:open="drawerOpen" title="Novo lançamento contábil" width="420">
      <a-form layout="vertical">
        <a-form-item label="Conta" required>
          <a-select
            v-model:value="formIdconta"
            :options="contas.map((c) => ({ value: c.id, label: c.descricao }))"
            show-search
            option-filter-prop="label"
          />
        </a-form-item>
        <a-form-item label="Centro de custo">
          <a-select v-model:value="formCentrocustoid" allow-clear :options="centrosCusto.map((c) => ({ value: c.id, label: c.nome }))" />
        </a-form-item>
        <a-form-item label="Tipo" required>
          <a-select v-model:value="formTipo" :options="[{ value: 'D', label: 'Débito' }, { value: 'C', label: 'Crédito' }]" />
        </a-form-item>
        <a-form-item label="Valor" required>
          <a-input-number v-model:value="formValor" style="width: 100%" :min="0.01" :step="0.01" />
        </a-form-item>
        <a-form-item label="Data/hora" required>
          <a-date-picker v-model:value="formDatahora" show-time style="width: 100%" format="DD/MM/YYYY HH:mm" />
        </a-form-item>
        <a-form-item label="Histórico">
          <a-textarea v-model:value="formHistorico" :rows="3" />
        </a-form-item>
      </a-form>
      <template #extra>
        <a-button type="primary" @click="handleSubmit">Salvar</a-button>
      </template>
    </a-drawer>
  </a-card>
</template>
