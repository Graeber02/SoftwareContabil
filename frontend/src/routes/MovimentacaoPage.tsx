import { useEffect, useState } from "react";
import {
  Card, Table, Button, Form, Input, DatePicker, Select, InputNumber,
  Space, message, Typography, Divider,
} from "antd";
import { PlusOutlined, DeleteOutlined, SaveOutlined } from "@ant-design/icons";
import dayjs from "dayjs";
import EmpresaSelect from "../components/EmpresaSelect";
import { movimentacaoApi, MovItemPayload } from "../api/modules";
import { createCrudApi } from "../api/crudFactory";

const { Title } = Typography;
const produtoApi = createCrudApi("produto");
const localApi = createCrudApi("local");
const cliforApi = createCrudApi("cliFor");

interface Props {
  tipo: "C" | "V";
  titulo: string;
}

export default function MovimentacaoPage({ tipo, titulo }: Props) {
  const [cliforId, setCliforId] = useState<string>();
  const [empresaId, setEmpresaId] = useState<string>();
  const [notaFiscal, setNotaFiscal] = useState<string>();
  const [data, setData] = useState(dayjs());
  const [itens, setItens] = useState<MovItemPayload[]>([]);
  const [produtos, setProdutos] = useState<any[]>([]);
  const [locais, setLocais] = useState<any[]>([]);
  const [clifores, setClifores] = useState<any[]>([]);
  const [historico, setHistorico] = useState<any[]>([]);
  const [salvando, setSalvando] = useState(false);

  useEffect(() => {
    produtoApi.list().then(setProdutos);
    localApi.list().then(setLocais);
    cliforApi.list().then(setClifores);
  }, []);

  useEffect(() => {
    movimentacaoApi.list().then((list: any[]) => setHistorico(list.filter((m) => m.tipo === tipo)));
  }, [tipo]);

  function addItem() {
    setItens([...itens, { produtoId: 0, localId: 0, quantidade: 1, valor: 0 }]);
  }

  function updateItem(idx: number, patch: Partial<MovItemPayload>) {
    setItens(itens.map((it, i) => (i === idx ? { ...it, ...patch } : it)));
  }

  function removeItem(idx: number) {
    setItens(itens.filter((_, i) => i !== idx));
  }

  async function salvar() {
    if (!cliforId || !empresaId) return message.warning("Informe cliente/fornecedor e empresa.");
    if (itens.length === 0) return message.warning("Adicione ao menos um item.");
    setSalvando(true);
    try {
      await movimentacaoApi.criarCompleta({
        notaFiscal,
        tipo,
        data: data.toISOString(),
        cliForId: cliforId,
        empresaId,
        itens,
      });
      message.success(`${titulo} registrada com sucesso.`);
      setItens([]);
      setNotaFiscal(undefined);
      movimentacaoApi.list().then((list: any[]) => setHistorico(list.filter((m) => m.tipo === tipo)));
    } catch {
      message.error(`Não foi possível registrar a ${titulo.toLowerCase()}.`);
    } finally {
      setSalvando(false);
    }
  }

  const total = itens.reduce((acc, it) => acc + it.quantidade * it.valor, 0);

  const columnsHistorico = [
    { title: "Nota Fiscal", dataIndex: "notafiscal" },
    { title: "Data", dataIndex: "data", render: (v: string) => dayjs(v).format("DD/MM/YYYY") },
    { title: "Valor Total", dataIndex: "valortotal", render: (v: number) => v?.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }) },
  ];

  return (
    <Card title={<Title level={4} style={{ margin: 0 }}>{titulo}</Title>}>
      <Space wrap style={{ marginBottom: 16 }}>
        <EmpresaSelect value={empresaId} onChange={setEmpresaId} style={{ width: 220 }} />
        <Select
          placeholder={tipo === "C" ? "Fornecedor" : "Cliente"}
          style={{ width: 220 }}
          value={cliforId}
          onChange={setCliforId}
          options={clifores.map((c) => ({ value: c.id, label: c.nome }))}
          showSearch
          optionFilterProp="label"
        />
        <Input placeholder="Nota fiscal" style={{ width: 160 }} value={notaFiscal} onChange={(e) => setNotaFiscal(e.target.value)} />
        <DatePicker value={data} onChange={(d) => d && setData(d)} format="DD/MM/YYYY" />
      </Space>

      <Divider orientation="left">Itens</Divider>
      <Space direction="vertical" style={{ width: "100%" }}>
        {itens.map((item, idx) => (
          <Space key={idx} wrap>
            <Select
              placeholder="Produto"
              style={{ width: 220 }}
              value={item.produtoId || undefined}
              onChange={(v) => updateItem(idx, { produtoId: v })}
              options={produtos.map((p) => ({ value: p.id, label: p.nome }))}
              showSearch
              optionFilterProp="label"
            />
            <Select
              placeholder="Local"
              style={{ width: 160 }}
              value={item.localId || undefined}
              onChange={(v) => updateItem(idx, { localId: v })}
              options={locais.map((l) => ({ value: l.id, label: l.descricao }))}
            />
            <InputNumber placeholder="Qtde" min={0.01} value={item.quantidade} onChange={(v) => updateItem(idx, { quantidade: v ?? 0 })} />
            <InputNumber placeholder="Valor unitário" min={0} prefix="R$" value={item.valor} onChange={(v) => updateItem(idx, { valor: v ?? 0 })} />
            <Button danger icon={<DeleteOutlined />} onClick={() => removeItem(idx)} />
          </Space>
        ))}
        <Button icon={<PlusOutlined />} onClick={addItem}>Adicionar item</Button>
      </Space>

      <Divider />
      <Space style={{ width: "100%", justifyContent: "space-between" }}>
        <Title level={5}>Total: {total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}</Title>
        <Button type="primary" icon={<SaveOutlined />} loading={salvando} onClick={salvar}>
          Registrar {titulo.toLowerCase()}
        </Button>
      </Space>

      <Divider orientation="left">Histórico</Divider>
      <Table rowKey="id" dataSource={historico} columns={columnsHistorico} pagination={{ pageSize: 10 }} />
    </Card>
  );
}
