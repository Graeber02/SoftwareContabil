import { useEffect, useState } from "react";
import {
  Card, Table, Button, Drawer, Form, InputNumber, DatePicker, Select,
  Input, message, Typography, Tag,
} from "antd";
import { PlusOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect";
import { contaApi, lancamentoContabilApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const { Title } = Typography;
const centroCustoApi = createCrudApi("centroCusto");

export default function LancamentosContabeisPage() {
  const [cliforId, setCliforId] = useState<string>();
  const [data, setData] = useState<any[]>([]);
  const [contas, setContas] = useState<any[]>([]);
  const [centrosCusto, setCentrosCusto] = useState<any[]>([]);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();

  useEffect(() => {
    centroCustoApi.list().then(setCentrosCusto);
  }, []);

  async function reload() {
    if (!cliforId) { setData([]); return; }
    setLoading(true);
    try {
      const [lancamentos, contasList] = await Promise.all([lancamentoContabilApi.list(), contaApi.list()]);
      setData(lancamentos.filter((l: any) => l.cliforid === cliforId));
      setContas(contasList.filter((c: any) => c.cliforid === cliforId && !c.sintetica));
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    reload();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [cliforId]);

  async function handleSubmit() {
    const values = await form.validateFields();
    try {
      await lancamentoContabilApi.create({
        ...values,
        datahora: values.datahora.toISOString(),
        cliforid: cliforId,
      });
      message.success("Lançamento registrado.");
      setDrawerOpen(false);
      form.resetFields();
      reload();
    } catch {
      message.error("Não foi possível registrar o lançamento.");
    }
  }

  const columns = [
    { title: "Data", dataIndex: "datahora", render: (v: string) => dayjs(v).format("DD/MM/YYYY HH:mm") },
    { title: "Histórico", dataIndex: "historico" },
    {
      title: "Tipo", dataIndex: "tipo", render: (t: string) => (
        <Tag color={t === "D" ? "red" : "green"}>{t === "D" ? "Débito" : "Crédito"}</Tag>
      ),
    },
    { title: "Valor", dataIndex: "valor", render: (v: number) => v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>Lançamentos Contábeis</Title>}
      extra={
        <div style={{ display: "flex", gap: 8 }}>
          <EmpresaSelect value={cliforId} onChange={setCliforId} />
          <Button type="primary" icon={<PlusOutlined />} disabled={!cliforId} onClick={() => setDrawerOpen(true)}>
            Novo lançamento
          </Button>
        </div>
      }
    >
      <Table rowKey="id" loading={loading} dataSource={data} columns={columns as any} />

      <Drawer
        title="Novo lançamento contábil"
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        width={420}
        extra={<Button type="primary" onClick={handleSubmit}>Salvar</Button>}
      >
        <Form layout="vertical" form={form}>
          <Form.Item name="idconta" label="Conta" rules={[{ required: true }]}>
            <Select options={contas.map((c) => ({ value: c.id, label: c.descricao }))} showSearch optionFilterProp="label" />
          </Form.Item>
          <Form.Item name="centrocustoid" label="Centro de custo">
            <Select allowClear options={centrosCusto.map((c) => ({ value: c.id, label: c.nome }))} />
          </Form.Item>
          <Form.Item name="tipo" label="Tipo" rules={[{ required: true }]}>
            <Select options={[{ value: "D", label: "Débito" }, { value: "C", label: "Crédito" }]} />
          </Form.Item>
          <Form.Item name="valor" label="Valor" rules={[{ required: true }]}>
            <InputNumber style={{ width: "100%" }} min={0.01} step={0.01} prefix="R$" />
          </Form.Item>
          <Form.Item name="datahora" label="Data/hora" rules={[{ required: true }]}>
            <DatePicker showTime style={{ width: "100%" }} format="DD/MM/YYYY HH:mm" />
          </Form.Item>
          <Form.Item name="historico" label="Histórico">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Drawer>
    </Card>
  );
}
