import { useEffect, useState } from "react";
import {
  Card, Table, Button, Tag, Modal, Form, InputNumber, DatePicker,
  Select, message, Typography,
} from "antd";
import { DollarOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect";
import { contaReceberApi } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const { Title } = Typography;
const especieApi = createCrudApi("especie");
const contaCorrenteApi = createCrudApi("contaCorrente");

export default function ContasReceberPage() {
  const [cliforId, setCliforId] = useState<string>();
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [recebendo, setRecebendo] = useState<any | null>(null);
  const [especies, setEspecies] = useState<any[]>([]);
  const [contasCorrentes, setContasCorrentes] = useState<any[]>([]);
  const [form] = Form.useForm();

  useEffect(() => {
    especieApi.list().then(setEspecies);
    contaCorrenteApi.list().then(setContasCorrentes);
  }, []);

  useEffect(() => {
    if (!cliforId) { setData([]); return; }
    setLoading(true);
    contaReceberApi.emAberto(cliforId).then(setData).finally(() => setLoading(false));
  }, [cliforId]);

  function openReceber(record: any) {
    setRecebendo(record);
    form.setFieldsValue({ valorRecebido: record.saldo, dataRecebimento: dayjs() });
  }

  async function confirmarRecebimento() {
    const values = await form.validateFields();
    try {
      await contaReceberApi.receber(recebendo.id, {
        valorRecebido: values.valorRecebido,
        dataRecebimento: values.dataRecebimento.toISOString(),
        especieId: values.especieId,
        contaCorrenteId: values.contaCorrenteId,
      });
      message.success("Recebimento registrado.");
      setRecebendo(null);
      if (cliforId) contaReceberApi.emAberto(cliforId).then(setData);
    } catch (e: any) {
      message.error(e?.response?.data ?? "Não foi possível registrar o recebimento.");
    }
  }

  const columns = [
    { title: "Descrição", dataIndex: "descricao" },
    { title: "Vencimento", dataIndex: "datavencimento", render: (v: string) => dayjs(v).format("DD/MM/YYYY") },
    { title: "Valor", dataIndex: "valor", render: (v: number) => v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) },
    {
      title: "Saldo", dataIndex: "saldo", render: (v: number) => (
        <Tag color={v > 0 ? "orange" : "green"}>{v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</Tag>
      ),
    },
    {
      title: "Ações", key: "actions", render: (_: any, record: any) => (
        <Button size="small" type="primary" icon={<DollarOutlined />} onClick={() => openReceber(record)}>
          Receber
        </Button>
      ),
    },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>Contas a Receber</Title>}
      extra={<EmpresaSelect value={cliforId} onChange={setCliforId} />}
    >
      <Table rowKey="id" loading={loading} dataSource={data} columns={columns as any} />

      <Modal
        title="Registrar recebimento"
        open={!!recebendo}
        onCancel={() => setRecebendo(null)}
        onOk={confirmarRecebimento}
        okText="Confirmar recebimento"
      >
        <Form layout="vertical" form={form}>
          <Form.Item name="valorRecebido" label="Valor recebido" rules={[{ required: true }]}>
            <InputNumber style={{ width: "100%" }} min={0.01} step={0.01} prefix="R$" />
          </Form.Item>
          <Form.Item name="dataRecebimento" label="Data do recebimento" rules={[{ required: true }]}>
            <DatePicker style={{ width: "100%" }} format="DD/MM/YYYY" />
          </Form.Item>
          <Form.Item name="especieId" label="Espécie" rules={[{ required: true }]}>
            <Select options={especies.map((e) => ({ value: e.id, label: e.descricao }))} />
          </Form.Item>
          <Form.Item name="contaCorrenteId" label="Conta corrente" rules={[{ required: true }]}>
            <Select options={contasCorrentes.map((c) => ({ value: c.id, label: c.descricao }))} />
          </Form.Item>
        </Form>
      </Modal>
    </Card>
  );
}
