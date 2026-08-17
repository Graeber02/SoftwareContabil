<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { message } from "ant-design-vue";
import { PlusOutlined, DeleteOutlined, SaveOutlined } from "@ant-design/icons-vue";
import dayjs, { Dayjs } from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { movimentacaoApi, type MovItemPayload } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const props = defineProps<{ tipo: "C" | "V"; titulo: string }>();

const produtoApi = createCrudApi("produto");
const localApi = createCrudApi("local");
const cliforApi = createCrudApi("cliFor");

const cliforId = ref<string>();
const empresaId = ref<string>();
const notaFiscal = ref<string>();
const data = ref<Dayjs>(dayjs());
const itens = ref<MovItemPayload[]>([]);
const produtos = ref<any[]>([]);
const locais = ref<any[]>([]);
const clifores = ref<any[]>([]);
const historico = ref<any[]>([]);
const salvando = ref(false);

onMounted(async () => {
  produtos.value = await produtoApi.list();
  locais.value = await localApi.list();
  clifores.value = await cliforApi.list();
  await reloadHistorico();
});

async function reloadHistorico() {
  const list = await movimentacaoApi.list();
  historico.value = list.filter((m: any) => m.tipo === props.tipo);
}

function addItem() {
  itens.value.push({ produtoId: 0, localId: 0, quantidade: 1, valor: 0 });
}

function removeItem(idx: number) {
  itens.value.splice(idx, 1);
}

async function salvar() {
  if (!cliforId.value || !empresaId.value) {
    message.warning("Informe cliente/fornecedor e empresa.");
    return;
  }
  if (itens.value.length === 0) {
    message.warning("Adicione ao menos um item.");
    return;
  }
  salvando.value = true;
  try {
    await movimentacaoApi.criarCompleta({
      notaFiscal: notaFiscal.value,
      tipo: props.tipo,
      data: data.value.toISOString(),
      cliForId: cliforId.value,
      empresaId: empresaId.value,
      itens: itens.value,
    });
    message.success(`${props.titulo} registrada com sucesso.`);
    itens.value = [];
    notaFiscal.value = undefined;
    await reloadHistorico();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? `Não foi possível registrar a ${props.titulo.toLowerCase()}.`);
  } finally {
    salvando.value = false;
  }
}

const total = computed(() => itens.value.reduce((acc, it) => acc + it.quantidade * it.valor, 0));

const columnsHistorico = [
  { title: "Nota Fiscal", dataIndex: "notafiscal" },
  { title: "Data", dataIndex: "data" },
  { title: "Valor Total", dataIndex: "valortotal", key: "valortotal" },
];
</script>

<template>
  <a-card>
    <template #title>{{ titulo }}</template>

    <a-space wrap style="margin-bottom: 16px">
      <EmpresaSelect v-model="empresaId" width="220px" />
      <a-select
        v-model:value="cliforId"
        :placeholder="tipo === 'C' ? 'Fornecedor' : 'Cliente'"
        style="width: 220px"
        :options="clifores.map((c) => ({ value: c.id, label: c.nome }))"
        show-search
        option-filter-prop="label"
      />
      <a-input v-model:value="notaFiscal" placeholder="Nota fiscal" style="width: 160px" />
      <a-date-picker v-model:value="data" format="DD/MM/YYYY" />
    </a-space>

    <a-divider orientation="left">Itens</a-divider>
    <a-space direction="vertical" style="width: 100%">
      <a-space v-for="(item, idx) in itens" :key="idx" wrap>
        <a-select
          v-model:value="item.produtoId"
          placeholder="Produto"
          style="width: 220px"
          :options="produtos.map((p) => ({ value: p.id, label: p.nome }))"
          show-search
          option-filter-prop="label"
        />
        <a-select
          v-model:value="item.localId"
          placeholder="Local"
          style="width: 160px"
          :options="locais.map((l) => ({ value: l.id, label: l.descricao }))"
        />
        <a-input-number v-model:value="item.quantidade" placeholder="Qtde" :min="0.01" />
        <a-input-number v-model:value="item.valor" placeholder="Valor unitário" :min="0" />
        <a-button danger @click="removeItem(idx)"><delete-outlined /></a-button>
      </a-space>
      <a-button @click="addItem"><template #icon><plus-outlined /></template>Adicionar item</a-button>
    </a-space>

    <a-divider />
    <a-space style="width: 100%; justify-content: space-between">
      <h4>Total: {{ total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}</h4>
      <a-button type="primary" :loading="salvando" @click="salvar">
        <template #icon><save-outlined /></template>
        Registrar {{ titulo.toLowerCase() }}
      </a-button>
    </a-space>

    <a-divider orientation="left">Histórico</a-divider>
    <a-table row-key="id" :data-source="historico" :columns="columnsHistorico" :pagination="{ pageSize: 10 }">
      <template #bodyCell="{ column, record, text }">
        <template v-if="column.dataIndex === 'data'">{{ dayjs(text).format("DD/MM/YYYY") }}</template>
        <template v-else-if="column.key === 'valortotal'">
          {{ record.valortotal?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) }}
        </template>
      </template>
    </a-table>
  </a-card>
</template>
