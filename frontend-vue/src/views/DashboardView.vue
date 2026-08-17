<script setup lang="ts">
import { BankOutlined, ShoppingCartOutlined, HomeOutlined, BarChartOutlined } from "@ant-design/icons-vue";
</script>

<template>
  <div>
    <h2>Bem-vindo</h2>
    <p style="color: rgba(0,0,0,0.45)">
      Visão geral do sistema. Use o menu à esquerda para acessar os módulos de
      Financeiro, Contábil, Compra e Venda, Patrimônio e os cadastros gerais.
    </p>
    <a-row :gutter="16">
      <a-col :span="6">
        <a-card>
          <a-statistic title="Financeiro" value="Contas a pagar / receber" :value-style="{ fontSize: '16px' }">
            <template #prefix><bank-outlined /></template>
          </a-statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card>
          <a-statistic title="Compra e Venda" value="Movimentação de estoque" :value-style="{ fontSize: '16px' }">
            <template #prefix><shopping-cart-outlined /></template>
          </a-statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card>
          <a-statistic title="Patrimônio" value="Bens e depreciação" :value-style="{ fontSize: '16px' }">
            <template #prefix><home-outlined /></template>
          </a-statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card>
          <a-statistic title="Gerencial" value="DRE e balancete" :value-style="{ fontSize: '16px' }">
            <template #prefix><bar-chart-outlined /></template>
          </a-statistic>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>
