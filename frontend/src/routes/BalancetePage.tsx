import { useEffect, useState } from "react";
import { Card, Table, Typography } from "antd";
import EmpresaSelect from "../components/EmpresaSelect";
import { gerencialApi } from "../api/modules";

const { Title } = Typography;

export default function BalancetePage() {
  const [cliforId, setCliforId] = useState<string>();
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!cliforId) { setData([]); return; }
    setLoading(true);
    gerencialApi.balancete(cliforId).then(setData).finally(() => setLoading(false));
  }, [cliforId]);

  const columns = [
    { title: "Conta", dataIndex: "descricao" },
    { title: "Hierarquia", dataIndex: "hierarquia" },
    {
      title: "Saldo",
      dataIndex: "saldo",
      align: "right" as const,
      render: (v: number) => Number(v).toLocaleString("pt-BR", { style: "currency", currency: "BRL" }),
    },
  ];

  return (
    <Card
      title={<Title level={4} style={{ margin: 0 }}>Balancete de Verificação</Title>}
      extra={<EmpresaSelect value={cliforId} onChange={setCliforId} />}
    >
      <Table rowKey="id" loading={loading} dataSource={data} columns={columns} pagination={{ pageSize: 20 }} />
    </Card>
  );
}
