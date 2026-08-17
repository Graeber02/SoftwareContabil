<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { message } from "ant-design-vue";
import { PlusOutlined, ExportOutlined } from "@ant-design/icons-vue";
import dayjs, { Dayjs } from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { patrimonioApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const centroCustoApi = createCrudApi("centroCusto");
const grupoBemApi = createCrudApi("grupoBem");
const estadoConservacaoApi = createCrudApi("estadoConservacao");
const motivoBaixaApi = createCrudApi("motivoBaixa");
const produtoApi = createCrudApi("produto");
const cliforApi = createCrudApi("cliFor");

const cliforId = ref<string>();
const data = ref<any[]>([]);
const loading = ref(false);
const drawerOpen = ref(false);
const baixando = ref<any | null>(null);
const baixandoOpen = computed({
  get: () => !!baixando.value,
  set: (v: boolean) => { if (!v) baixando.value = null; },
});

const refs = ref<Record<string, any[]>>({});

const formDataaquisicao = ref<Dayjs>(dayjs());
const formValor = ref<number>();
const formObservacao = ref<string>();
const formDepreciavel = ref(false);
const formProdutoid = ref<number>();
const formCentrocustoid = ref<number>();
const formGrupobemid = ref<number>();
const formEstadoconservacaoid = ref<number>();
const formFornecedorid = ref<string>();

const formBaixaData = ref<Dayjs>(dayjs());
const formBaixaValor = ref<number>();
const formBaixaMotivoId = ref<number>();
const formBaixaObservacao = ref<string>();

onMounted(async () => {
  const [centroCusto, grupoBem, estadoConservacao, motivoBaixa, produto, clifor] = await Promise.all([
    centroCustoApi.list(), grupoBemApi.list(), estadoConservacaoApi.list(),
    motivoBaixaApi.list(), produtoApi.list(), cliforApi.list(),
  ]);
  refs.value = { centroCusto, grupoBem, estadoConservacao, motivoBaixa, produto, clifor };
});

async function reload() {
  if (!cliforId.value) { data.value = []; return; }
  loading.value = true;
  try {
    data.value = await patrimonioApi.ativos(cliforId.value);
  } finally {
    loading.value = false;
  }
}
watch(cliforId, reload);

function openNew() {
  formDataaquisicao.value = dayjs();
  formValor.value = undefined;
  formObservacao.value = undefined;
  formDepreciavel.value = false;
  formProdutoid.value = undefined;
  formCentrocustoid.value = undefined;
  formGrupobemid.value = undefined;
  formEstadoconservacaoid.value = undefined;
  formFornecedorid.value = undefined;
  drawerOpen.value = true;
}

async function handleSubmit() {
  if (!cliforId.value) return;
  try {
    await patrimonioApi.create({
      dataaquisicao: formDataaquisicao.value.toISOString(),
      valor: formValor.value,
      observacao: formObservacao.value,
      depreciavel: formDepreciavel.value,
      produtoid: formProdutoid.value,
      centrocustoid: formCentrocustoid.value,
      grupobemid: formGrupobemid.value,
      estadoconservacaoid: formEstadoconservacaoid.value,
      fornecedorid: formFornecedorid.value,
      cliforid: cliforId.value,
      baixado: 0,
    });
    message.success("Bem cadastrado.");
    drawerOpen.value = false;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível cadastrar o bem.");
  }
}

function openBaixa(record: any) {
  baixando.value = record;
  formBaixaData.value = dayjs();
  formBaixaValor.value = record.valor;
  formBaixaMotivoId.value = undefined;
  formBaixaObservacao.value = undefined;
}

async function confirmarBaixa() {
  if (!baixando.value || !formBaixaMotivoId.value) {
    message.warning("Selecione o motivo da baixa.");
    return;
  }
  try {
    await patrimonioApi.baixar(baixando.value.id, {
      data: formBaixaData.value.toISOString(),
      valor: formBaixaValor.value!,
      observacao: formBaixaObservacao.value,
      motivoBaixaId: formBaixaMotivoId.value,
    });
    message.success("Bem baixado.");
    baixando.value = null;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível baixar o bem.");
  }
}

const columns = [
  { title: "Data aquisição", dataIndex: "dataaquisicao" },
  { title: "Valor", dataIndex: "valor", key: "valor" },
  { title: "Observação", dataIndex: "observacao" },
  { title: "Depreciável", dataIndex: "depreciavel", key: "depreciavel" },
  { title: "Ações", key: "actions" },
];
</script>

<template>
  <a-card>
    <template #title>Bens Patrimoniais</template>
    <template #extra>
      <a-space>
        <EmpresaSelect v-model="cliforId" />
        <a-button type="primary" :disabled="!cliforId" @click="openNew">
          <template #icon><plus-outlined /></template>
          Novo bem
        </a-button>
      </a-space>
    </template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns">
      <template #bodyCell="{ column, record, text }">
        <template v-if="column.dataIndex === 'dataaquisicao'">{{ dayjs(text).format("DD/MM/YYYY") }}</template>
        <template v-else-if="column.key === 'valor'">
          {{ record.valor?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}
        </template>
        <template v-else-if="column.key === 'depreciavel'">
          <a-tag :color="record.depreciavel ? 'blue' : 'default'">{{ record.depreciavel ? "Sim" : "Não" }}</a-tag>
        </template>
        <template v-else-if="column.key === 'actions'">
          <a-button size="small" danger @click="openBaixa(record)">
            <template #icon><export-outlined /></template>
            Dar baixa
          </a-button>
        </template>
      </template>
    </a-table>

    <a-drawer v-model:open="drawerOpen" title="Novo bem patrimonial" width="420">
      <a-form layout="vertical">
        <a-form-item label="Data de aquisição" required>
          <a-date-picker v-model:value="formDataaquisicao" style="width: 100%" format="DD/MM/YYYY" />
        </a-form-item>
        <a-form-item label="Valor" required>
          <a-input-number v-model:value="formValor" style="width: 100%" :min="0" />
        </a-form-item>
        <a-form-item label="Observação">
          <a-textarea v-model:value="formObservacao" :rows="2" />
        </a-form-item>
        <a-form-item label="Depreciável">
          <a-switch v-model:checked="formDepreciavel" />
        </a-form-item>
        <a-form-item label="Produto">
          <a-select
            v-model:value="formProdutoid"
            allow-clear
            :options="(refs.produto ?? []).map((p) => ({ value: p.id, label: p.nome }))"
            show-search
            option-filter-prop="label"
          />
        </a-form-item>
        <a-form-item label="Centro de custo" required>
          <a-select v-model:value="formCentrocustoid" :options="(refs.centroCusto ?? []).map((c) => ({ value: c.id, label: c.nome }))" />
        </a-form-item>
        <a-form-item label="Grupo do bem" required>
          <a-select v-model:value="formGrupobemid" :options="(refs.grupoBem ?? []).map((g) => ({ value: g.id, label: g.descricao }))" />
        </a-form-item>
        <a-form-item label="Estado de conservação" required>
          <a-select v-model:value="formEstadoconservacaoid" :options="(refs.estadoConservacao ?? []).map((e) => ({ value: e.id, label: e.descricao }))" />
        </a-form-item>
        <a-form-item label="Fornecedor" required>
          <a-select
            v-model:value="formFornecedorid"
            :options="(refs.clifor ?? []).map((c) => ({ value: c.id, label: c.nome }))"
            show-search
            option-filter-prop="label"
          />
        </a-form-item>
      </a-form>
      <template #extra>
        <a-button type="primary" @click="handleSubmit">Salvar</a-button>
      </template>
    </a-drawer>

    <a-modal v-model:open="baixandoOpen" title="Dar baixa no bem" ok-text="Confirmar baixa" @ok="confirmarBaixa">
      <a-form layout="vertical">
        <a-form-item label="Data da baixa" required>
          <a-date-picker v-model:value="formBaixaData" style="width: 100%" format="DD/MM/YYYY" />
        </a-form-item>
        <a-form-item label="Valor" required>
          <a-input-number v-model:value="formBaixaValor" style="width: 100%" :min="0" />
        </a-form-item>
        <a-form-item label="Motivo da baixa" required>
          <a-select v-model:value="formBaixaMotivoId" :options="(refs.motivoBaixa ?? []).map((m) => ({ value: m.id, label: m.descricao }))" />
        </a-form-item>
        <a-form-item label="Observação">
          <a-textarea v-model:value="formBaixaObservacao" :rows="2" />
        </a-form-item>
      </a-form>
    </a-modal>
  </a-card>
</template>
