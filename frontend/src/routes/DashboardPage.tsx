import { Card, Col, Row, Statistic, Typography } from "antd";
import {
  BankOutlined, HomeOutlined, ShoppingCartOutlined, BarChartOutlined,
} from "@ant-design/icons";

const { Title, Paragraph } = Typography;

export default function DashboardPage() {
  return (
    <div>
      <Title level={3}>Bem-vindo</Title>
      <Paragraph type="secondary">
        Visão geral do sistema. Use o menu à esquerda para acessar os módulos de
        Financeiro, Contábil, Compra e Venda, Patrimônio e os cadastros gerais.
      </Paragraph>
      <Row gutter={16}>
        <Col span={6}>
          <Card>
            <Statistic title="Financeiro" prefix={<BankOutlined />} value="Contas a pagar / receber" valueStyle={{ fontSize: 16 }} />
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Statistic title="Compra e Venda" prefix={<ShoppingCartOutlined />} value="Movimentação de estoque" valueStyle={{ fontSize: 16 }} />
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Statistic title="Patrimônio" prefix={<HomeOutlined />} value="Bens e depreciação" valueStyle={{ fontSize: 16 }} />
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Statistic title="Gerencial" prefix={<BarChartOutlined />} value="DRE e balancete" valueStyle={{ fontSize: 16 }} />
          </Card>
        </Col>
      </Row>
    </div>
  );
}
