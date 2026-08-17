<script setup lang="ts">
import { ref, computed, watch, onMounted } from "vue";
import { useRoute } from "vue-router";
import { message } from "ant-design-vue";
import { PlusOutlined, EditOutlined, DeleteOutlined } from "@ant-design/icons-vue";
import dayjs from "dayjs";
import { CADASTRO_MODULES } from "../modules/cadastroModules";
import { createCrudApi } from "../api/crudFactory";

const route = useRoute();
const moduleKey = computed(() => route.params.moduleKey as string);
const meta = computed(() => CADASTRO_MODULES.find((m) => m.key === moduleKey.value));

const data = ref<any[]>([]);
const loading = ref(false);
const drawerOpen = ref(false);
const editing = ref<any | null>(null);
const fkOptions = ref<Record<string, any[]>>({});
const formState = ref<Record<string, any>>({});

function api() {
  return meta.value ? createCrudApi(meta.value.resource) : null;
}

async function loadData() {
  const a = api();
  if (!a) return;
  loading.value = true;
  try {
    data.value = await a.list();
  } catch {
    message.error("Não foi possível carregar os dados.");
  } finally {
    loading.value = false;
  }
}

async function loadFkOptions() {
  if (!meta.value) return;
  const entries = await Promise.all(
    meta.value.fks.map(async (fk) => {
      try {
        const opts = await createCrudApi(fk.resource).list();
        return [fk.name, opts] as const;
      } catch {
        return [fk.name, []] as const;
      }
    })
  );
  fkOptions.value = Object.fromEntries(entries);
}

function reload() {
  data.value = [];
  loadData();
  loadFkOptions();
}

onMounted(reload);
watch(moduleKey, reload);

function openNew() {
  editing.value = null;
  formState.value = {};
  drawerOpen.value = true;
}

function openEdit(record: any) {
  editing.value = record;
  const values: Record<string, any> = { ...record };
  meta.value!.fields.forEach((f) => {
    if (f.type === "date" && values[f.name]) values[f.name] = dayjs(values[f.name]);
  });
  formState.value = values;
  drawerOpen.value = true;
}

async function handleDelete(record: any) {
  const a = api();
  if (!a) return;
  try {
    await a.remove(record.id);
    message.success("Registro excluído.");
    loadData();
  } catch {
    message.error("Não foi possível excluir. Verifique se o registro não está em uso.");
  }
}

async function handleSubmit() {
  const a = api();
  if (!a || !meta.value) return;
  const payload: Record<string, any> = { ...formState.value };
  meta.value.fields.forEach((f) => {
    if (f.type === "date" && payload[f.name]) payload[f.name] = payload[f.name].toISOString();
  });
  try {
    if (editing.value) {
      await a.update(editing.value.id, { ...payload, id: editing.value.id });
      message.success("Registro atualizado.");
    } else {
      await a.create(payload);
      message.success("Registro criado.");
    }
    drawerOpen.value = false;
    loadData();
  } catch {
    message.error("Não foi possível salvar. Confira os dados informados.");
  }
}

const columns = computed(() => {
  if (!meta.value) return [];
  return [
    { title: "ID", dataIndex: "id", key: "id", width: 90 },
    ...meta.value.fields.map((f) => ({ title: f.label, dataIndex: f.name, key: f.name })),
    { title: "Ações", key: "actions", width: 120 },
  ];
});
</script>

<template>
  <a-card v-if="!meta">
    <p>Módulo não encontrado.</p>
  </a-card>

  <a-card v-else>
    <template #title>{{ meta.label }}</template>
    <template #extra>
      <a-button type="primary" @click="openNew">
        <template #icon><plus-outlined /></template>
        Novo
      </a-button>
    </template>

    <a-table row-key="id" :loading="loading" :data-source="data" :columns="columns">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'actions'">
          <a-space>
            <a-button size="small" @click="openEdit(record)"><edit-outlined /></a-button>
            <a-popconfirm title="Excluir este registro?" @confirm="handleDelete(record)">
              <a-button size="small" danger><delete-outlined /></a-button>
            </a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>

    <a-drawer
      v-model:open="drawerOpen"
      :title="editing ? `Editar ${meta.label}` : `Novo ${meta.label}`"
      width="420"
    >
      <a-form layout="vertical">
        <a-form-item
          v-for="f in meta.fields"
          :key="f.name"
          :label="f.label"
          :required="f.required"
        >
          <a-input v-if="f.type === 'text'" v-model:value="formState[f.name]" />
          <a-input-number v-else-if="f.type === 'number'" v-model:value="formState[f.name]" style="width: 100%" />
          <a-switch v-else-if="f.type === 'boolean'" v-model:checked="formState[f.name]" />
          <a-date-picker
            v-else-if="f.type === 'date'"
            v-model:value="formState[f.name]"
            style="width: 100%"
            format="DD/MM/YYYY"
          />
        </a-form-item>

        <a-form-item
          v-for="fk in meta.fks"
          :key="fk.name"
          :label="fk.label"
          :required="fk.required"
        >
          <a-select
            v-model:value="formState[fk.name]"
            :placeholder="`Selecione ${fk.label.toLowerCase()}`"
            :options="(fkOptions[fk.name] ?? []).map((opt: any) => ({ value: opt.id, label: opt[fk.optionLabelField] ?? opt.id }))"
            show-search
            option-filter-prop="label"
          />
        </a-form-item>
      </a-form>

      <template #extra>
        <a-space>
          <a-button @click="drawerOpen = false">Cancelar</a-button>
          <a-button type="primary" @click="handleSubmit">Salvar</a-button>
        </a-space>
      </template>
    </a-drawer>
  </a-card>
</template>
