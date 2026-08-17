import { useMemo, useState } from "react";
import { Layout, Menu, Avatar, Dropdown, theme, Typography } from "antd";
import {
  DashboardOutlined,
  BankOutlined,
  TeamOutlined,
  ShoppingOutlined,
  SwapOutlined,
  HomeOutlined,
  BarChartOutlined,
  SettingOutlined,
  LogoutOutlined,
  UserOutlined,
} from "@ant-design/icons";
import { Outlet, useNavigate, useLocation } from "react-router-dom";
import { useAuthStore } from "../store/authStore";
import { CADASTRO_MODULES } from "../modules/cadastroModules";

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

function buildMenuItems() {
  const byGroup: Record<string, { key: string; label: string }[]> = {};
  for (const m of CADASTRO_MODULES) {
    byGroup[m.group] = byGroup[m.group] ?? [];
    byGroup[m.group].push({ key: `/cadastro/${m.key}`, label: m.label });
  }

  const groupIcon: Record<string, React.ReactNode> = {
    "Cadastros Gerais": <TeamOutlined />,
    "Cadastros de Produtos": <ShoppingOutlined />,
    "Financeiro": <BankOutlined />,
    "Patrimônio": <HomeOutlined />,
    "Gerencial": <BarChartOutlined />,
    "Sistema": <SettingOutlined />,
  };

  const cadastroChildren = Object.entries(byGroup).map(([group, items]) => ({
    key: group,
    icon: groupIcon[group] ?? <SettingOutlined />,
    label: group,
    children: items,
  }));

  return [
    { key: "/", icon: <DashboardOutlined />, label: "Início" },
    {
      key: "financeiro",
      icon: <BankOutlined />,
      label: "Financeiro",
      children: [
        { key: "/financeiro/contas-pagar", label: "Contas a Pagar" },
        { key: "/financeiro/contas-receber", label: "Contas a Receber" },
      ],
    },
    {
      key: "contabil",
      icon: <BarChartOutlined />,
      label: "Contábil",
      children: [
        { key: "/contabil/plano-de-contas", label: "Plano de Contas" },
        { key: "/contabil/lancamentos", label: "Lançamentos Contábeis" },
        { key: "/contabil/dre", label: "DRE" },
        { key: "/contabil/balancete", label: "Balancete" },
      ],
    },
    {
      key: "compra-venda",
      icon: <SwapOutlined />,
      label: "Compra e Venda",
      children: [
        { key: "/compra-venda/compra", label: "Compra" },
        { key: "/compra-venda/venda", label: "Venda" },
      ],
    },
    {
      key: "patrimonio",
      icon: <HomeOutlined />,
      label: "Patrimônio",
      children: [{ key: "/patrimonio/bens", label: "Bens Patrimoniais" }],
    },
    ...cadastroChildren,
  ];
}

export default function MainLayout() {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { email, logout } = useAuthStore();
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const items = useMemo(buildMenuItems, []);

  const userMenu = {
    items: [
      { key: "logout", icon: <LogoutOutlined />, label: "Sair" },
    ],
    onClick: ({ key }: { key: string }) => {
      if (key === "logout") {
        logout();
        navigate("/login");
      }
    },
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider collapsible collapsed={collapsed} onCollapse={setCollapsed} width={260}>
        <div
          style={{
            height: 56,
            margin: 12,
            color: "#fff",
            fontWeight: 700,
            fontSize: collapsed ? 16 : 18,
            textAlign: "center",
            lineHeight: "56px",
            overflow: "hidden",
            whiteSpace: "nowrap",
          }}
        >
          {collapsed ? "SC" : "Software Contábil"}
        </div>
        <Menu
          theme="dark"
          mode="inline"
          selectedKeys={[location.pathname]}
          items={items}
          onClick={({ key }) => {
            if (key.startsWith("/")) navigate(key);
          }}
        />
      </Sider>
      <Layout>
        <Header
          style={{
            padding: "0 24px",
            background: colorBgContainer,
            display: "flex",
            alignItems: "center",
            justifyContent: "flex-end",
            borderBottom: "1px solid #f0f0f0",
          }}
        >
          <Dropdown menu={userMenu} placement="bottomRight">
            <div style={{ display: "flex", alignItems: "center", gap: 8, cursor: "pointer" }}>
              <Avatar icon={<UserOutlined />} />
              <Text>{email}</Text>
            </div>
          </Dropdown>
        </Header>
        <Content style={{ margin: 24 }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
}
