-- ============================================================
-- Software Contábil - Schema PostgreSQL
-- Gerado a partir do modelo de dados original (migração Java -> .NET)
-- ============================================================

CREATE TABLE estado (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    sigla varchar(255) NOT NULL
);

CREATE TABLE cidade (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    estadoid integer NOT NULL,
    CONSTRAINT fk_cidade_estadoid FOREIGN KEY (estadoid) REFERENCES estado(id) ON DELETE RESTRICT
);

CREATE TABLE endereco (
    id serial PRIMARY KEY,
    cep varchar(255) NOT NULL,
    rua varchar(255) NOT NULL,
    bairro varchar(255) NOT NULL,
    numero integer NOT NULL,
    complemento varchar(255),
    cidadeid integer NOT NULL,
    CONSTRAINT fk_endereco_cidadeid FOREIGN KEY (cidadeid) REFERENCES cidade(id) ON DELETE RESTRICT
);

CREATE TABLE clifor (
    id varchar(20) PRIMARY KEY,
    cnpj varchar(255),
    cpf varchar(255),
    tipopessoa char(1) NOT NULL,
    nome varchar(255) NOT NULL,
    nomefantasia varchar(255),
    email varchar(255),
    telefone varchar(255),
    celular varchar(255),
    tipocliente varchar(255),
    enderecoid integer,
    CONSTRAINT fk_clifor_enderecoid FOREIGN KEY (enderecoid) REFERENCES endereco(id) ON DELETE SET NULL
);

CREATE TABLE especie (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL
);

CREATE TABLE historico (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    historico varchar(255) NOT NULL
);

CREATE TABLE unidademedida (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    sigla varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_unidademedida_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE marca (
    id serial PRIMARY KEY,
    nome varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_marca_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE grupo (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_grupo_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE local (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_local_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE grupobem (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    taxadepreciacao numeric(18,4) NOT NULL,
    vidautil numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_grupobem_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE estadoconservacao (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_estadoconservacao_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE motivobaixa (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_motivobaixa_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE centrocusto (
    id serial PRIMARY KEY,
    nome varchar(255) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_centrocusto_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE conta (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    sintetica boolean NOT NULL,
    ordem integer NOT NULL,
    hierarquia varchar(255),
    cliforid varchar(20) NOT NULL,
    contapai integer,
    CONSTRAINT fk_conta_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_conta_contapai FOREIGN KEY (contapai) REFERENCES conta(id) ON DELETE SET NULL
);

CREATE TABLE contacorrente (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    agencia numeric(18,4) NOT NULL,
    conta varchar(255) NOT NULL,
    codbanco numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_contacorrente_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE contapagar (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    numdocumento numeric(18,4) NOT NULL,
    seriedocumento numeric(18,4) NOT NULL,
    datadocumento timestamp NOT NULL,
    datavencimento timestamp NOT NULL,
    datapagamento timestamp,
    valor numeric(18,4) NOT NULL,
    saldo numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    fornecedorid varchar(20) NOT NULL,
    CONSTRAINT fk_contapagar_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_contapagar_fornecedorid FOREIGN KEY (fornecedorid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE contareceber (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    numdocumento numeric(18,4) NOT NULL,
    datadocumento timestamp NOT NULL,
    datavencimento timestamp NOT NULL,
    datarecebimento timestamp,
    valor numeric(18,4) NOT NULL,
    saldo numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_contareceber_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE pagamento (
    id serial PRIMARY KEY,
    valorpago numeric(18,4) NOT NULL,
    datapagamento timestamp NOT NULL,
    descricao varchar(255),
    contapagarid integer NOT NULL,
    especieid integer NOT NULL,
    contacorrenteid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_pagamento_contapagarid FOREIGN KEY (contapagarid) REFERENCES contapagar(id) ON DELETE RESTRICT,
    CONSTRAINT fk_pagamento_especieid FOREIGN KEY (especieid) REFERENCES especie(id) ON DELETE RESTRICT,
    CONSTRAINT fk_pagamento_contacorrenteid FOREIGN KEY (contacorrenteid) REFERENCES contacorrente(id) ON DELETE RESTRICT,
    CONSTRAINT fk_pagamento_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE recebimento (
    id serial PRIMARY KEY,
    valorrecebido numeric(18,4) NOT NULL,
    datarecebimento timestamp NOT NULL,
    descricao varchar(255),
    contareceberid integer NOT NULL,
    especieid integer NOT NULL,
    contacorrenteid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_recebimento_contareceberid FOREIGN KEY (contareceberid) REFERENCES contareceber(id) ON DELETE RESTRICT,
    CONSTRAINT fk_recebimento_especieid FOREIGN KEY (especieid) REFERENCES especie(id) ON DELETE RESTRICT,
    CONSTRAINT fk_recebimento_contacorrenteid FOREIGN KEY (contacorrenteid) REFERENCES contacorrente(id) ON DELETE RESTRICT,
    CONSTRAINT fk_recebimento_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE lancamentocontabil (
    id serial PRIMARY KEY,
    valor numeric(18,4) NOT NULL,
    datahora timestamp NOT NULL,
    historico varchar(255),
    tipo varchar(255) NOT NULL,
    idconta integer NOT NULL,
    centrocustoid integer,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_lancamentocontabil_idconta FOREIGN KEY (idconta) REFERENCES conta(id) ON DELETE RESTRICT,
    CONSTRAINT fk_lancamentocontabil_centrocustoid FOREIGN KEY (centrocustoid) REFERENCES centrocusto(id) ON DELETE SET NULL,
    CONSTRAINT fk_lancamentocontabil_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE produto (
    id serial PRIMARY KEY,
    nome varchar(255) NOT NULL,
    descricao varchar(255) NOT NULL,
    quantidademinima numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    grupoid integer NOT NULL,
    marcaid integer NOT NULL,
    unidademedidaid integer NOT NULL,
    fornecedorid varchar(20) NOT NULL,
    CONSTRAINT fk_produto_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_produto_grupoid FOREIGN KEY (grupoid) REFERENCES grupo(id) ON DELETE RESTRICT,
    CONSTRAINT fk_produto_marcaid FOREIGN KEY (marcaid) REFERENCES marca(id) ON DELETE RESTRICT,
    CONSTRAINT fk_produto_unidademedidaid FOREIGN KEY (unidademedidaid) REFERENCES unidademedida(id) ON DELETE RESTRICT,
    CONSTRAINT fk_produto_fornecedorid FOREIGN KEY (fornecedorid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE preco (
    id serial PRIMARY KEY,
    valor numeric(18,4) NOT NULL,
    datavigente timestamp NOT NULL,
    produtoid integer,
    CONSTRAINT fk_preco_produtoid FOREIGN KEY (produtoid) REFERENCES produto(id) ON DELETE SET NULL
);

CREATE TABLE saldo (
    id serial PRIMARY KEY,
    qtde numeric(18,4) NOT NULL,
    valor numeric(18,4) NOT NULL,
    produtoid integer NOT NULL,
    localid integer NOT NULL,
    CONSTRAINT fk_saldo_produtoid FOREIGN KEY (produtoid) REFERENCES produto(id) ON DELETE RESTRICT,
    CONSTRAINT fk_saldo_localid FOREIGN KEY (localid) REFERENCES local(id) ON DELETE RESTRICT
);

CREATE TABLE fechaestoque (
    id serial PRIMARY KEY,
    data timestamp NOT NULL,
    customedio numeric(18,4) NOT NULL,
    qtde integer NOT NULL,
    idproduto integer NOT NULL,
    CONSTRAINT fk_fechaestoque_idproduto FOREIGN KEY (idproduto) REFERENCES produto(id) ON DELETE RESTRICT
);

CREATE TABLE movimentacao (
    id serial PRIMARY KEY,
    notafiscal varchar(255),
    tipo char(1) NOT NULL,
    data timestamp NOT NULL,
    valortotal numeric(18,4) NOT NULL,
    cliforid varchar(20) NOT NULL,
    empresaid varchar(20) NOT NULL,
    CONSTRAINT fk_movimentacao_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_movimentacao_empresaid FOREIGN KEY (empresaid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE movitens (
    id serial PRIMARY KEY,
    sequencia integer NOT NULL,
    quantidade numeric(18,4) NOT NULL,
    valor numeric(18,4) NOT NULL,
    movimentacaoid integer NOT NULL,
    produtoid integer NOT NULL,
    localid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_movitens_movimentacaoid FOREIGN KEY (movimentacaoid) REFERENCES movimentacao(id) ON DELETE RESTRICT,
    CONSTRAINT fk_movitens_produtoid FOREIGN KEY (produtoid) REFERENCES produto(id) ON DELETE RESTRICT,
    CONSTRAINT fk_movitens_localid FOREIGN KEY (localid) REFERENCES local(id) ON DELETE RESTRICT,
    CONSTRAINT fk_movitens_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE patrimonio (
    id serial PRIMARY KEY,
    dataaquisicao timestamp NOT NULL,
    valor numeric(18,4) NOT NULL,
    observacao varchar(255),
    baixado integer NOT NULL,
    depreciavel boolean NOT NULL,
    centrocustoid integer NOT NULL,
    fornecedorid varchar(20) NOT NULL,
    cliforid varchar(20) NOT NULL,
    estadoconservacaoid integer NOT NULL,
    grupobemid integer NOT NULL,
    produtoid integer,
    CONSTRAINT fk_patrimonio_centrocustoid FOREIGN KEY (centrocustoid) REFERENCES centrocusto(id) ON DELETE RESTRICT,
    CONSTRAINT fk_patrimonio_fornecedorid FOREIGN KEY (fornecedorid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_patrimonio_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT,
    CONSTRAINT fk_patrimonio_estadoconservacaoid FOREIGN KEY (estadoconservacaoid) REFERENCES estadoconservacao(id) ON DELETE RESTRICT,
    CONSTRAINT fk_patrimonio_grupobemid FOREIGN KEY (grupobemid) REFERENCES grupobem(id) ON DELETE RESTRICT,
    CONSTRAINT fk_patrimonio_produtoid FOREIGN KEY (produtoid) REFERENCES produto(id) ON DELETE SET NULL
);

CREATE TABLE baixabem (
    id serial PRIMARY KEY,
    data timestamp NOT NULL,
    valor numeric(18,4) NOT NULL,
    observacao varchar(255),
    motivobaixaid integer NOT NULL,
    patrimonioid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_baixabem_motivobaixaid FOREIGN KEY (motivobaixaid) REFERENCES motivobaixa(id) ON DELETE RESTRICT,
    CONSTRAINT fk_baixabem_patrimonioid FOREIGN KEY (patrimonioid) REFERENCES patrimonio(id) ON DELETE RESTRICT,
    CONSTRAINT fk_baixabem_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE depreciacao (
    id serial PRIMARY KEY,
    mes integer NOT NULL,
    ano integer NOT NULL,
    valordepreciado numeric(18,4) NOT NULL,
    valorreavaliado numeric(18,4) NOT NULL,
    vidautil numeric(18,4) NOT NULL,
    taxadepreciacaomensal numeric(18,4) NOT NULL,
    taxadepreciacaoanual numeric(18,4) NOT NULL,
    valoratualizado numeric(18,4) NOT NULL,
    depreciacao integer,
    datadepreciacao timestamp NOT NULL,
    valoranual numeric(18,4) NOT NULL,
    valormes numeric(18,4) NOT NULL,
    patrimonioid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_depreciacao_patrimonioid FOREIGN KEY (patrimonioid) REFERENCES patrimonio(id) ON DELETE RESTRICT,
    CONSTRAINT fk_depreciacao_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE despesainvestimento (
    id serial PRIMARY KEY,
    data timestamp NOT NULL,
    observacao varchar(255),
    valor numeric(18,4) NOT NULL,
    tipo varchar(255) NOT NULL,
    patrimonioid integer NOT NULL,
    cliforid varchar(20) NOT NULL,
    CONSTRAINT fk_despesainvestimento_patrimonioid FOREIGN KEY (patrimonioid) REFERENCES patrimonio(id) ON DELETE RESTRICT,
    CONSTRAINT fk_despesainvestimento_cliforid FOREIGN KEY (cliforid) REFERENCES clifor(id) ON DELETE RESTRICT
);

CREATE TABLE relatorio (
    id serial PRIMARY KEY,
    nome varchar(255) NOT NULL,
    excluido boolean NOT NULL,
    sqlquery varchar(255),
    cliforid varchar(255)
);

CREATE TABLE filtrorelatorio (
    id serial PRIMARY KEY,
    nome varchar(255) NOT NULL,
    excluido boolean NOT NULL,
    sqlwhere varchar(255),
    relatorioid integer NOT NULL,
    CONSTRAINT fk_filtrorelatorio_relatorioid FOREIGN KEY (relatorioid) REFERENCES relatorio(id) ON DELETE RESTRICT
);

CREATE TABLE solicitacaorelatorio (
    id serial PRIMARY KEY,
    descricao varchar(255) NOT NULL,
    tipo integer NOT NULL,
    cliforid varchar(255),
    relatorioid integer NOT NULL,
    CONSTRAINT fk_solicitacaorelatorio_relatorioid FOREIGN KEY (relatorioid) REFERENCES relatorio(id) ON DELETE RESTRICT
);

CREATE TABLE auditoria (
    id serial PRIMARY KEY,
    tabela varchar(255) NOT NULL,
    valorantigo varchar(255),
    valornovo varchar(255),
    cliforid varchar(255)
);

CREATE TABLE usuario (
    id serial PRIMARY KEY,
    email varchar(255) NOT NULL,
    senha varchar(255) NOT NULL
);
