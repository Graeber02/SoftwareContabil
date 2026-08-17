import { useEffect, useState } from "react";
import { Card, Tree, Button, Drawer, Form, Input, InputNumber, Switch, Select, message, Typography } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import EmpresaSelect from "../components/EmpresaSelect";
import { contaApi } from "../api/modules";

const { Title } = Typography;

function toTreeData(nodes: any[]): any[] {
  return nodes.map((n) => ({
    key: n.id,
    title: `${n.descricao}${n.sintetica ? "" : "  (analítica)"}`,
    children: n.filhos?.length ? toTreeData(n.filhos) : undefined,
  }));
}

export default function PlanoDeContasPage() {
  const [cliforId, setCliforId] = useState<string>();
  const [arvore, setArvore] = useState<any[]>([]);
  const [contasFlat, setContasFlat] = useState<any[]>([]);
  const [drawerOpen, setDrawerOpen] = useState(false);
  const [form] = Form.useForm();

  async function reload() {
    if (!cliforId) return;
    const [tree, flat] = await Promise.all([contaApi.arvore(cliforId), contaApi.list()]);
    setArvore(tree);
    setContasFlat(flat.filter((c: any) => c.cliforid === cliforId));
  }

  useEffect(() => {
    setArvore([]);
    reload();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [cliforId]);

  async function handleSubmit() {
    const values = await form.validateFields();
    try {
      await contaApi.create({ ...values, cliforid: cliforId });
      message.success("Conta criada.");
      setDrawerOpen(false);
      form.resetFields();
      reload();
    } catch {
      message.error("Não foi possível criar a conta.");
    }
  }

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>Plano de Contas</Title>}
      extra={
        <div style={{ display: "flex", gap: 8 }}>
          <EmpresaSelect value={cliforId} onChange={setCliforId} />
          <Button type="primary" icon={<PlusOutlined />} disabled={!cliforId} onClick={() => setDrawerOpen(true)}>
            Nova conta
          </Button>
        </div>
      }
    >
      {cliforId ? (
        <Tree treeData={toTreeData(arvore)} defaultExpandAll showLine />
      ) : (
        <Typography.Text type="secondary">Selecione uma empresa/cliente para ver o plano de contas.</Typography.Text>
      )}

      <Drawer
        title="Nova conta contábil"
        open={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        width={400}
        extra={<Button type="primary" onClick={handleSubmit}>Salvar</Button>}
      >
        <Form layout="vertical" form={form}>
          <Form.Item name="descricao" label="Descrição" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="ordem" label="Ordem" rules={[{ required: true }]}>
            <InputNumber style={{ width: "100%" }} />
          </Form.Item>
          <Form.Item name="sintetica" label="Sintética (agrupadora)" valuePropName="checked">
            <Switch />
          </Form.Item>
          <Form.Item name="contapai" label="Conta pai (opcional)">
            <Select
              allowClear
              options={contasFlat.map((c) => ({ value: c.id, label: c.descricao }))}
              showSearch
              optionFilterProp="label"
            />
          </Form.Item>
        </Form>
      </Drawer>
    </Card>
  );
}
