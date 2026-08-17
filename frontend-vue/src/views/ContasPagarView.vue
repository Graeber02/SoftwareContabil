<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { message } from "ant-design-vue";
import { DollarOutlined } from "@ant-design/icons-vue";
import dayjs, { Dayjs } from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { contaPagarApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const especieApi = createCrudApi("especie");
const contaCorrenteApi = createCrudApi("contaCorrente");

const cliforId = ref<string>();
const data = ref<any[]>([]);
const loading = ref(false);
const pagando = ref<any | null>(null);
// a-modal espera um boolean em `open` — pagando guarda o registro, então
// derivamos um boolean computado para o v-model do modal (evita que o Vue
// sobrescreva `pagando` com `false` ao fechar).
const pagandoOpen = computed({
  get: () => !!pagando.value,
  set: (v: boolean) => { if (!v) pagando.value = null; },
});
const especies = ref<any[]>([]);
const contasCorrentes = ref<any[]>([]);

const formValorPago = ref<number>(0);
const formDataPagamento = ref<Dayjs>(dayjs());
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
    data.value = await contaPagarApi.emAberto(cliforId.value);
  } finally {
    loading.value = false;
  }
}
watch(cliforId, reload);

function openPagar(record: any) {
  pagando.value = record;
  formValorPago.value = record.saldo;
  formDataPagamento.value = dayjs();
  formEspecieId.value = undefined;
  formContaCorrenteId.value = undefined;
}

async function confirmarPagamento() {
  if (!pagando.value || !formEspecieId.value || !formContaCorrenteId.value) {
    message.warning("Preencha espécie e conta corrente.");
    return;
  }
  try {
    await contaPagarApi.pagar(pagando.value.id, {
      valorPago: formValorPago.value,
      dataPagamento: formDataPagamento.value.toISOString(),
      especieId: formEspecieId.value,
      contaCorrenteId: formContaCorrenteId.value,
    });
    message.success("Pagamento registrado.");
    pagando.value = null;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível registrar o pagamento.");
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
    <template #title>Contas a Pagar</template>
    <template #extra><EmpresaSelect v-model="cliforId" /></template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns">
      <template #bodyCell="{ column, record, text }">
        <template v-if="column.dataIndex === 'datavencimento'">{{ dayjs(text).format("DD/MM/YYYY") }}</template>
        <template v-else-if="column.dataIndex === 'valor'">{{ currency(text) }}</template>
        <template v-else-if="column.key === 'saldo'">
          <a-tag :color="record.saldo > 0 ? 'orange' : 'green'">{{ currency(record.saldo) }}</a-tag>
        </template>
        <template v-else-if="column.key === 'actions'">
          <a-button size="small" type="primary" @click="openPagar(record)">
            <template #icon><dollar-outlined /></template>
            Pagar
          </a-button>
        </template>
      </template>
    </a-table>

    <a-modal v-model:open="pagandoOpen" title="Registrar pagamento" ok-text="Confirmar pagamento" @ok="confirmarPagamento">
      <a-form layout="vertical">
        <a-form-item label="Valor pago" required>
          <a-input-number v-model:value="formValorPago" style="width: 100%" :min="0.01" :step="0.01" />
        </a-form-item>
        <a-form-item label="Data do pagamento" required>
          <a-date-picker v-model:value="formDataPagamento" style="width: 100%" format="DD/MM/YYYY" />
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
