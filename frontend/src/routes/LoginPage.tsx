import { useState } from "react";
import { Form, Input, Button, Card, Typography, message, Tabs } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/authStore";

const { Title, Text } = Typography;

export default function LoginPage() {
  const [loading, setLoading] = useState(false);
  const [mode, setMode] = useState<"login" | "registrar">("login");
  const { login, registrar } = useAuthStore();
  const navigate = useNavigate();

  async function onFinish(values: { email: string; senha: string }) {
    setLoading(true);
    try {
      if (mode === "login") {
        await login(values.email, values.senha);
        navigate("/");
      } else {
        await registrar(values.email, values.senha);
        message.success("Usuário criado! Faça login.");
        setMode("login");
      }
    } catch {
      message.error(mode === "login" ? "Email ou senha inválidos." : "Não foi possível registrar.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div style={{
      minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center",
      background: "linear-gradient(135deg,#0f2445,#173a68)",
    }}>
      <Card style={{ width: 380 }}>
        <div style={{ textAlign: "center", marginBottom: 16 }}>
          <Title level={3} style={{ marginBottom: 0 }}>Software Contábil</Title>
          <Text type="secondary">Gestão financeira, patrimonial e contábil</Text>
        </div>
        <Tabs
          activeKey={mode}
          centered
          onChange={(k) => setMode(k as "login" | "registrar")}
          items={[
            { key: "login", label: "Entrar" },
            { key: "registrar", label: "Criar conta" },
          ]}
        />
        <Form layout="vertical" onFinish={onFinish}>
          <Form.Item name="email" label="Email" rules={[{ required: true, type: "email" }]}>
            <Input prefix={<UserOutlined />} placeholder="voce@empresa.com" />
          </Form.Item>
          <Form.Item name="senha" label="Senha" rules={[{ required: true, min: 6 }]}>
            <Input.Password prefix={<LockOutlined />} placeholder="••••••••" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" block loading={loading}>
              {mode === "login" ? "Entrar" : "Criar conta"}
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
}
