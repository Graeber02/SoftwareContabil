import { useEffect, useState } from "react";
import {
  Card, Table, Button, Drawer, Form, Input, InputNumber, DatePicker, Select,
  Switch, Modal, message, Typography, Tag, Space,
} from "antd";
import { PlusOutlined, ExportOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect";
import { patrimonioApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const { Title } = Typography;
const centroCustoApi = createCrudApi("centroCusto");
const grupoBemApi = createCrudApi("grupoBem");
const estadoConservacaoApi = createCrudApi("estadoConservacao");
const motivoBaixaApi = createCrudApi("motivoBaixa");
const produtoApi = createCrudApi("produto");
const cliforApi = createCrudApi("cliFor");

export default function PatrimonioPage() {
  const [cliforId, setCliforId] = useState<string>();
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [baixando, setBaixando] = useState<any | null>(null);
  const [refs, setRefs] = useState<Record<string, any[]>>({});
  const [form] = Form.useForm();
  const [formBaixa] = Form.useForm();

  useEffect(() => {
    Promise.all([
      centroCustoApi.list(), grupoBemApi.list(), estadoConservacaoApi.list(),
      motivoBaixaApi.list(), produtoApi.list(), cliforApi.list(),
    ]).then(([centroCusto, grupoBem, estadoConservacao, motivoBaixa, produto, clifor]) => {
      setRefs({ centroCusto, grupoBem, estadoConservacao, motivoBaixa, produto, clifor });
    });
  }, []);

  async function reload() {
    if (!cliforId) { setData([]); return; }
    setLoading(true);
    try {
      setData(await patrimonioApi.ativos(cliforId));
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
      await patrimonioApi.create({
        ...values,
        dataaquisicao: values.dataaquisicao.toISOString(),
        cliforid: cliforId,
        baixado: 0,
      });
      message.success("Bem cadastrado.");
      setDrawerOpen(false);
      form.resetFields();
      reload();
    } catch {
      message.error("Não foi possível cadastrar o bem.");
    }
  }

  function openBaixa(record: any) {
    setBaixando(record);
    formBaixa.setFieldsValue({ data: dayjs(), valor: record.valor });
  }

  async function confirmarBaixa() {
    const values = await formBaixa.validateFields();
    try {
      await patrimonioApi.baixar(baixando.id, {
        data: values.data.toISOString(),
        valor: values.valor,
        observacao: values.observacao,
        motivoBaixaId: values.motivoBaixaId,
      });
      message.success("Bem baixado.");
      setBaixando(null);
      reload();
    } catch {
      message.error("Não foi possível baixar o bem.");
    }
  }

  const columns = [
    { title: "Data aquisição", dataIndex: "dataaquisicao", render: (v: string) => dayjs(v).format("DD/MM/YYYY") },
    { title: "Valor", dataIndex: "valor", render: (v: number) => v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) },
    { title: "Observação", dataIndex: "observacao" },
    {
      title: "Depreciável", dataIndex: "depreciavel", render: (v: boolean) => <Tag color={v ? "blue" : "default"}>{v ? "Sim" : "Não"}</Tag>,
    },
    {
      title: "Ações", key: "actions", render: (_: any, record: any) => (
        <Button size="small" danger icon={<ExportOutlined />} onClick={() => openBaixa(record)}>
          Dar baixa
        </Button>
      ),
    },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>Bens Patrimoniais</Title>}
      extra={
        <Space>
          <EmpresaSelect value={cliforId} onChange={setCliforId} />
          <Button type="primary" icon={<PlusOutlined />} disabled={!cliforId} onClick={() => setDrawerOpen(true)}>
            Novo bem
          </Button>
        </Space>
      }
    >
      <Table rowKey="id" loading={loading} dataSource={data} columns={columns as any} />

      <Drawer
        title="Novo bem patrimonial"
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        width={420}
        extra={<Button type="primary" onClick={handleSubmit}>Salvar</Button>}
      >
        <Form layout="vertical" form={form}>
          <Form.Item name="dataaquisicao" label="Data de aquisição" rules={[{ required: true }]}>
            <DatePicker style={{ width: "100%" }} format="DD/MM/YYYY" />
          </Form.Item>
          <Form.Item name="valor" label="Valor" rules={[{ required: true }]}>
            <InputNumber style={{ width: "100%" }} prefix="R$" min={0} />
          </Form.Item>
          <Form.Item name="observacao" label="Observação">
            <Input.TextArea rows={2} />
          </Form.Item>
          <Form.Item name="depreciavel" label="Depreciável" valuePropName="checked">
            <Switch />
          </Form.Item>
          <Form.Item name="produtoid" label="Produto">
            <Select allowClear options={(refs.produto ?? []).map((p) => ({ value: p.id, label: p.nome }))} showSearch optionFilterProp="label" />
          </Form.Item>
          <Form.Item name="centrocustoid" label="Centro de custo" rules={[{ required: true }]}>
            <Select options={(refs.centroCusto ?? []).map((c) => ({ value: c.id, label: c.nome }))} />
          </Form.Item>
          <Form.Item name="grupobemid" label="Grupo do bem" rules={[{ required: true }]}>
            <Select options={(refs.grupoBem ?? []).map((g) => ({ value: g.id, label: g.descricao }))} />
          </Form.Item>
          <Form.Item name="estadoconservacaoid" label="Estado de conservação" rules={[{ required: true }]}>
            <Select options={(refs.estadoConservacao ?? []).map((e) => ({ value: e.id, label: e.descricao }))} />
          </Form.Item>
          <Form.Item name="fornecedorid" label="Fornecedor" rules={[{ required: true }]}>
            <Select options={(refs.clifor ?? []).map((c) => ({ value: c.id, label: c.nome }))} showSearch optionFilterProp="label" />
          </Form.Item>
        </Form>
      </Drawer>

      <Modal title="Dar baixa no bem" open={!!baixando} onCancel={() => setBaixando(null)} onOk={confirmarBaixa} okText="Confirmar baixa" okButtonProps={{ danger: true }}>
        <Form layout="vertical" form={formBaixa}>
          <Form.Item name="data" label="Data da baixa" rules={[{ required: true }]}>
            <DatePicker style={{ width: "100%" }} format="DD/MM/YYYY" />
          </Form.Item>
          <Form.Item name="valor" label="Valor" rules={[{ required: true }]}>
            <InputNumber style={{ width: "100%" }} prefix="R$" min={0} />
          </Form.Item>
          <Form.Item name="motivoBaixaId" label="Motivo da baixa" rules={[{ required: true }]}>
            <Select options={(refs.motivoBaixa ?? []).map((m) => ({ value: m.id, label: m.descricao }))} />
          </Form.Item>
          <Form.Item name="observacao" label="Observação">
            <Input.TextArea rows={2} />
          </Form.Item>
        </Form>
      </Modal>
    </Card>
  );
}
