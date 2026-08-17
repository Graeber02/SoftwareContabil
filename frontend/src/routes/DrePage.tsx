import { useEffect, useState } from "react";
import { Card, Table, Typography } from "antd";
import EmpresaSelect from "../components/EmpresaSelect";
import { gerencialApi } from "../api/modules";

const { Title } = Typography;

export default function DrePage() {
  const [cliforId, setCliforId] = useState<string>();
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!cliforId) { setData([]); return; }
    setLoading(true);
    gerencialApi.dre(cliforId).then(setData).finally(() => setLoading(false));
  }, [cliforId]);

  const columns = [
    { title: "Grupo", dataIndex: "grupo" },
    {
      title: "Valor",
      dataIndex: "valorTotal",
      align: "right" as const,
      render: (v: number) => Number(v).toLocaleString("pt-BR", { style: "currency", currency: "BRL" }),
    },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>DRE — Demonstração do Resultado do Exercício</Title>}
      extra={<EmpresaSelect value={cliforId} onChange={setCliforId} />}
    >
      <Table rowKey="ordem" loading={loading} dataSource={data} columns={columns} pagination={false} />
    </Card>
  );
}
