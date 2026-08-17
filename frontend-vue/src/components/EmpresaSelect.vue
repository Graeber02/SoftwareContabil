<script setup lang="ts">
import { ref, onMounted } from "vue";
import { createCrudApi } from "../api/crudFactory";

const props = defineProps<{ modelValue?: string; width?: string }>();
const emit = defineEmits<{ "update:modelValue": [value: string] }>();

const cliforApi = createCrudApi("cliFor");
const options = ref<{ value: string; label: string }[]>([]);

onMounted(async () => {
  const list = await cliforApi.list();
  options.value = list.map((c: any) => ({ value: c.id, label: c.nome ?? c.id }));
});

function onChange(value: string) {
  emit("update:modelValue", value);
}
</script>

<template>
  <a-select
    placeholder="Selecione a empresa/cliente"
    :style="{ width: props.width ?? '280px' }"
    :value="props.modelValue"
    :options="options"
    show-search
    option-filter-prop="label"
    @change="onChange"
  />
</template>
