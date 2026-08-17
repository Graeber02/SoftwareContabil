<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { message } from "ant-design-vue";
import { DollarOutlined } from "@ant-design/icons-vue";
import dayjs, { Dayjs } from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { contaReceberApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const especieApi = createCrudApi("especie");
const contaCorrenteApi = createCrudApi("contaCorrente");

const cliforId = ref<string>();
const data = ref<any[]>([]);
const loading = ref(false);
const recebendo = ref<any | null>(null);
const recebendoOpen = computed({
  get: () => !!recebendo.value,
  set: (v: boolean) => { if (!v) recebendo.value = null; },
});
const especies = ref<any[]>([]);
const contasCorrentes = ref<any[]>([]);

const formValorRecebido = ref<number>(0);
const formDataRecebimento = ref<Dayjs>(dayjs());
const formEspecieId = ref<number>();
const formContaCorrenteId = ref<number>();

onMounted(() => {
  especieApi.list().then((l: any[]) => (especies.value = l));
  contaCorrenteApi.list().then((l: any[]) => (contasCorrentes.value = l));
});

async function reload() {
  if (!cliforId.value) { data.value = []; return; }
  loading.value = true;
  try {
    data.value = await contaReceberApi.emAberto(cliforId.value);
  } finally {
    loading.value = false;
  }
}
watch(cliforId, reload);

function openReceber(record: any) {
  recebendo.value = record;
  formValorRecebido.value = record.saldo;
  formDataRecebimento.value = dayjs();
  formEspecieId.value = undefined;
  formContaCorrenteId.value = undefined;
}

async function confirmarRecebimento() {
  if (!recebendo.value || !formEspecieId.value || !formContaCorrenteId.value) {
    message.warning("Preencha espécie e conta corrente.");
    return;
  }
  try {
    await contaReceberApi.receber(recebendo.value.id, {
      valorRecebido: formValorRecebido.value,
      dataRecebimento: formDataRecebimento.value.toISOString(),
      especieId: formEspecieId.value,
      contaCorrenteId: formContaCorrenteId.value,
    });
    message.success("Recebimento registrado.");
    recebendo.value = null;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível registrar o recebimento.");
  }
}

const columns = [
  { title: "Descrição", dataIndex: "descricao" },
  { title: "Vencimento", dataIndex: "datavencimento" },
  { title: "Valor", dataIndex: "valor" },
  { title: "Saldo", dataIndex: "saldo", key: "saldo" },
  { title: "Ações", key: "actions" },
];

function currency(v: number) {
  return v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
}
</script>

<template>
  <a-card>
    <template #title>Contas a Receber</template>
    <template #extra><EmpresaSelect v-model="cliforId" /></template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns">
      <template #bodyCell="{ column, record, text }">
        <template v-if="column.dataIndex === 'datavencimento'">{{ dayjs(text).format("DD/MM/YYYY") }}</template>
        <template v-else-if="column.dataIndex === 'valor'">{{ currency(text) }}</template>
        <template v-else-if="column.key === 'saldo'">
          <a-tag :color="record.saldo > 0 ? 'orange' : 'green'">{{ currency(record.saldo) }}</a-tag>
        </template>
        <template v-else-if="column.key === 'actions'">
          <a-button size="small" type="primary" @click="openReceber(record)">
            <template #icon><dollar-outlined /></template>
            Receber
          </a-button>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="recebendoOpen" title="Registrar recebimento" ok-text="Confirmar recebimento" @ok="confirmarRecebimento">
      <a-form layout="vertical">
        <a-form-item label="Valor recebido" required>
          <a-input-number v-model:value="formValorRecebido" style="width: 100%" :min="0.01" :step="0.01" />
        </a-form-item>
        <a-form-item label="Data do recebimento" required>
          <a-date-picker v-model:value="formDataRecebimento" style="width: 100%" format="DD/MM/YYYY" />
        </a-form-item>
        <a-form-item label="Espécie" required>
          <a-select v-model:value="formEspecieId" :options="especies.map((e) => ({ value: e.id, label: e.descricao }))" />
        </a-form-item>
        <a-form-item label="Conta corrente" required>
          <a-select v-model:value="formContaCorrenteId" :options="contasCorrentes.map((c) => ({ value: c.id, label: c.descricao }))" />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>
