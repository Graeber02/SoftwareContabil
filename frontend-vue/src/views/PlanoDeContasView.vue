<script setup lang="ts">
import { ref, watch } from "vue";
import { message } from "ant-design-vue";
import { PlusOutlined } from "@ant-design/icons-vue";
import EmpresaSelect from "../components/EmpresaSelect.vue";
import { contaApi } from "../api/modules";

const cliforId = ref<string>();
const arvore = ref<any[]>([]);
const contasFlat = ref<any[]>([]);
const drawerOpen = ref(false);

const formDescricao = ref("");
const formOrdem = ref<number>();
const formSintetica = ref(false);
const formContapai = ref<number>();

function toTreeData(nodes: any[]): any[] {
  return nodes.map((n) => ({
    key: n.id,
    title: `${n.descricao}${n.sintetica ? "" : "  (analítica)"}`,
    children: n.filhos?.length ? toTreeData(n.filhos) : undefined,
  }));
}

async function reload() {
  if (!cliforId.value) { arvore.value = []; return; }
  const [tree, flat] = await Promise.all([contaApi.arvore(cliforId.value), contaApi.list()]);
  arvore.value = tree;
  contasFlat.value = flat.filter((c: any) => c.cliforid === cliforId.value);
}
watch(cliforId, reload);

function openNew() {
  formDescricao.value = "";
  formOrdem.value = undefined;
  formSintetica.value = false;
  formContapai.value = undefined;
  drawerOpen.value = true;
}

async function handleSubmit() {
  if (!cliforId.value) return;
  try {
    await contaApi.create({
      descricao: formDescricao.value,
      ordem: formOrdem.value,
      sintetica: formSintetica.value,
      contapai: formContapai.value,
      cliforid: cliforId.value,
    });
    message.success("Conta criada.");
    drawerOpen.value = false;
    reload();
  } catch (e: any) {
    message.error(e?.response?.data?.title ?? "Não foi possível criar a conta.");
  }
}
</script>

<template>
  <a-card>
    <template #title>Plano de Contas</template>
    <template #extra>
      <a-space>
        <EmpresaSelect v-model="cliforId" />
        <a-button type="primary" :disabled="!cliforId" @click="openNew">
          <template #icon><plus-outlined /></template>
          Nova conta
        </a-button>
      </a-space>
    </template>

    <a-tree v-if="cliforId" :tree-data="toTreeData(arvore)" default-expand-all show-line />
    <span v-else style="color: rgba(0,0,0,0.45)">Selecione uma empresa/cliente para ver o plano de contas.</span>

    <a-drawer v-model:open="drawerOpen" title="Nova conta contábil" width="400">
      <a-form layout="vertical">
        <a-form-item label="Descrição" required>
          <a-input v-model:value="formDescricao" />
        </a-form-item>
        <a-form-item label="Ordem" required>
          <a-input-number v-model:value="formOrdem" style="width: 100%" />
        </a-form-item>
        <a-form-item label="Sintética (agrupadora)">
          <a-switch v-model:checked="formSintetica" />
        </a-form-item>
        <a-form-item label="Conta pai (opcional)">
          <a-select
            v-model:value="formContapai"
            allow-clear
            :options="contasFlat.map((c) => ({ value: c.id, label: c.descricao }))"
            show-search
            option-filter-prop="label"
          />
        </a-form-item>
      </a-form>
      <template #extra>
        <a-button type="primary" @click="handleSubmit">Salvar</a-button>
      </template>
    </a-drawer>
  </a-card>
</template>
