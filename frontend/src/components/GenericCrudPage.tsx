import { useEffect, useMemo, useState } from "react";
import {
  Table, Button, Drawer, Form, Input, InputNumber, Switch, DatePicker,
  Select, Space, Popconfirm, message, Typography, Card,
} from "antd";
import { PlusOutlined, EditOutlined, DeleteOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import { useParams } from "react-router-dom";
import { CADASTRO_MODULES } from "../modules/cadastroModules";
import { createCrudApi } from "../api/crudFactory";

const { Title } = Typography;

export default function GenericCrudPage() {
  const { moduleKey } = useParams<{ moduleKey: string }>();
  const meta = useMemo(() => CADASTRO_MODULES.find((m) => m.key === moduleKey), [moduleKey]);

  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [editing, setEditing] = useState<any | null>(null);
  const [fkOptions, setFkOptions] = useState<Record<string, any[]>>({});
  const [form] = Form.useForm();

  const api = useMemo(() => (meta ? createCrudApi(meta.resource) : null), [meta]);

  async function loadData() {
    if (!api) return;
    setLoading(true);
    try {
      const list = await api.list();
      setData(list);
    } catch {
      message.error("Não foi possível carregar os dados.");
    } finally {
      setLoading(false);
    }
  }

  async function loadFkOptions() {
    if (!meta) return;
    const entries = await Promise.all(
      meta.fks.map(async (fk) => {
        try {
          const opts = await createCrudApi(fk.resource).list();
          return [fk.name, opts] as const;
        } catch {
          return [fk.name, []] as const;
        }
      })
    );
    setFkOptions(Object.fromEntries(entries));
  }

  useEffect(() => {
    setData([]);
    loadData();
    loadFkOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [moduleKey]);

  if (!meta) {
    return <Card>Módulo não encontrado.</Card>;
  }

  function openNew() {
    setEditing(null);
    form.resetFields();
    setDrawerOpen(true);
  }

  function openEdit(record: any) {
    setEditing(record);
    const values: any = { ...record };
    meta!.fields.forEach((f) => {
      if (f.type === "date" && values[f.name]) values[f.name] = dayjs(values[f.name]);
    });
    form.setFieldsValue(values);
    setDrawerOpen(true);
  }

  async function handleDelete(record: any) {
    if (!api) return;
    try {
      await api.remove(record.id);
      message.success("Registro excluído.");
      loadData();
    } catch {
      message.error("Não foi possível excluir. Verifique se o registro não está em uso.");
    }
  }

  async function handleSubmit() {
    if (!api) return;
    const values = await form.validateFields();
    const payload: any = { ...values };
    meta.fields.forEach((f) => {
      if (f.type === "date" && payload[f.name]) payload[f.name] = payload[f.name].toISOString();
    });
    try {
      if (editing) {
        await api.update(editing.id, { ...payload, id: editing.id });
        message.success("Registro atualizado.");
      } else {
        await api.create(payload);
        message.success("Registro criado.");
      }
      setDrawerOpen(false);
      loadData();
    } catch {
      message.error("Não foi possível salvar. Confira os dados informados.");
    }
  }

  const columns = [
    { title: "ID", dataIndex: "id", key: "id", width: 90 },
    ...meta.fields.map((f) => ({ title: f.label, dataIndex: f.name, key: f.name })),
    {
      title: "Ações",
      key: "actions",
      width: 120,
      render: (_: any, record: any) => (
        <Space>
          <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(record)} />
          <Popconfirm title="Excluir este registro?" onConfirm={() => handleDelete(record)}>
            <Button size="small" danger icon={<DeleteOutlined />} />
          </Popconfirm>
        </Space>
      ),
    },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>{meta.label}</Title>}
      extra={
        <Button type="primary" icon={<PlusOutlined />} onClick={openNew}>
          Novo
        </Button>
      }
    >
      <Table rowKey="id" loading={loading} dataSource={data} columns={columns as any} />

      <Drawer
        title={editing ? `Editar ${meta.label}` : `Novo ${meta.label}`}
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        width={420}
        extra={
          <Space>
            <Button onClick={() => setDrawerOpen(false)}>Cancelar</Button>
            <Button type="primary" onClick={handleSubmit}>Salvar</Button>
          </Space>
        }
      >
        <Form layout="vertical" form={form}>
          {meta.fields.map((f) => (
            <Form.Item
              key={f.name}
              name={f.name}
              label={f.label}
              valuePropName={f.type === "boolean" ? "checked" : "value"}
              rules={f.required ? [{ required: true, message: `Informe ${f.label.toLowerCase()}` }] : []}
            >
              {f.type === "text" && <Input />}
              {f.type === "number" && <InputNumber style={{ width: "100%" }} />}
              {f.type === "boolean" && <Switch />}
              {f.type === "date" && <DatePicker style={{ width: "100%" }} format="DD/MM/YYYY" />}
            </Form.Item>
          ))}
          {meta.fks.map((fk) => (
            <Form.Item
              key={fk.name}
              name={fk.name}
              label={fk.label}
              rules={fk.required ? [{ required: true, message: `Selecione ${fk.label.toLowerCase()}` }] : []}
            >
              <Select
                placeholder={`Selecione ${fk.label.toLowerCase()}`}
                options={(fkOptions[fk.name] ?? []).map((opt) => ({
                  value: opt.id,
                  label: opt[fk.optionLabelField] ?? opt.id,
                }))}
                showSearch
                optionFilterProp="label"
              />
            </Form.Item>
          ))}
        </Form>
      </Drawer>
    </Card>
  );
}
