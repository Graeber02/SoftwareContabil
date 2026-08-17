-- ============================================================
-- Funções de negócio (portadas do sistema original)
-- ============================================================

CREATE OR REPLACE FUNCTION ativopassivo(integer) RETURNS text
AS
'select ativopassivo from (
WITH RECURSIVE conts (id, level, descricao, name_path) AS (
    SELECT  id, 0, descricao, ARRAY[cast(descricao as text)]
    FROM    conta
    WHERE   contapai is null

    UNION ALL

    SELECT  p.id, t0.level + 1, p.descricao, ARRAY_APPEND(t0.name_path, cast(p.descricao as text))
    FROM    conta p
            INNER JOIN conts t0 ON t0.id = p.contapai
)
SELECT  id, name_path[1] AS ativopassivo
FROM    conts) as dados where id = $1;'
LANGUAGE SQL
    IMMUTABLE
    RETURNS NULL ON NULL INPUT;


CREATE OR REPLACE FUNCTION hierarquia(text, text) RETURNS TABLE (id integer, hierarquia text)
    AS
$$
select id, hierarquia from (
WITH RECURSIVE conts(id, descricao, ordem, name_path) AS
  (SELECT id,
          descricao,
          ordem, ARRAY[cast(ordem AS text)]
   FROM conta
   WHERE contapai IS NULL
     AND cliforid = $1
   UNION ALL SELECT p.id, p.descricao, p.ordem, ARRAY_APPEND(t0.name_path, cast(p.ordem AS text))
   FROM conta p
   INNER JOIN conts t0 ON t0.id = p.contapai
   WHERE p.cliforid = $1)
SELECT conts.id, cast(ARRAY_TO_STRING(name_path, '.') AS text) hierarquia, conts.descricao, vdd.cliforid, vdd.sintetica, conts.ordem, vdd.contapai
FROM conts
INNER JOIN conta vdd on(conts.id = vdd.id)
WHERE conts.descricao = $2
ORDER BY hierarquia
) as dados
$$ LANGUAGE SQL;


CREATE OR REPLACE FUNCTION contas_filhas(text, text)
 RETURNS TABLE(id integer)
 LANGUAGE sql
AS $function$
select id from (
WITH RECURSIVE conts(id, descricao, ordem, name_path) AS
  (SELECT id,
          descricao,
          ordem, ARRAY[cast(ordem AS text)]
   FROM conta
   WHERE contapai IS NULL
     AND cliforid = $1
   UNION ALL SELECT p.id, p.descricao, p.ordem, ARRAY_APPEND(t0.name_path, cast(p.ordem AS text))
   FROM conta p
   INNER JOIN conts t0 ON t0.id = p.contapai
   WHERE p.cliforid = $1)
SELECT conts.id, cast(ARRAY_TO_STRING(name_path, '.') AS text) hierarquia, conts.descricao, vdd.cliforid, vdd.sintetica, conts.ordem, vdd.contapai
FROM conts
INNER JOIN conta vdd on(conts.id = vdd.id)
ORDER BY hierarquia
) as dados where contapai = (select id from hierarquia($1,$2)) or id = (select id from hierarquia($1,$2))
$function$;


CREATE OR REPLACE FUNCTION calcula_dre(character varying)
 RETURNS TABLE(ordem integer, valortotal numeric, grupo text)
 LANGUAGE plpgsql
AS $function$
DECLARE
    RECEITA_OPERACIONAL_BRUTA decimal;
    DEDUCOES_DA_RECEITA_BRUTA decimal;
    RECEITA_OPERACIONAL_LIQUIDA decimal;
    CUSTO_DAS_MERCADORIAS_E_SERVICOS decimal;
    RESULTADO_OPERACIONAL_BRUTO decimal;
    DESPESAS_OPERACIONAIS decimal;
    DESPESAS_FINANCEIRAS decimal;
    RECEITAS_FINANCEIRAS decimal;
    DESPESAS_FINANCEIRAS_RECEITAS_FINANCEIRAS decimal;
    OUTRAS_RECEITAS decimal;
    OUTRAS_DESPESAS decimal;
    OUTRAS_RECEITAS_E_DESPESAS decimal;
    RESULTADO_OPERACIONAL_ANTES_IR_CSLL decimal;
    IR_CSLL decimal;
    RESULTADO_LIQUIDO_EXERCICIO decimal;
BEGIN
    RECEITA_OPERACIONAL_BRUTA := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'VENDA DE MERCADORIAS')));
    RECEITA_OPERACIONAL_BRUTA := RECEITA_OPERACIONAL_BRUTA + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'VENDA DE PRODUTOS')));
    RECEITA_OPERACIONAL_BRUTA := RECEITA_OPERACIONAL_BRUTA + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'VENDA DE SERVIÇOS')));

    DEDUCOES_DA_RECEITA_BRUTA := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'ABATIMENTOS')));
    DEDUCOES_DA_RECEITA_BRUTA := DEDUCOES_DA_RECEITA_BRUTA + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'DEVOLUÇÕES')));
    DEDUCOES_DA_RECEITA_BRUTA := DEDUCOES_DA_RECEITA_BRUTA + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'IMPOSTOS SOBRE VENDAS')));

    RECEITA_OPERACIONAL_LIQUIDA := RECEITA_OPERACIONAL_BRUTA - DEDUCOES_DA_RECEITA_BRUTA;

    CUSTO_DAS_MERCADORIAS_E_SERVICOS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'CUSTO DAS MERCADORIAS VENDIDAS (CMV)')));
    CUSTO_DAS_MERCADORIAS_E_SERVICOS := CUSTO_DAS_MERCADORIAS_E_SERVICOS + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'CUSTO DOS PRODUTOS VENDIDOS (CPV)')));
    CUSTO_DAS_MERCADORIAS_E_SERVICOS := CUSTO_DAS_MERCADORIAS_E_SERVICOS + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'CUSTO DOS SERVIÇOS PRESTADOS (CSP)')));

    RESULTADO_OPERACIONAL_BRUTO := RECEITA_OPERACIONAL_BRUTA - DEDUCOES_DA_RECEITA_BRUTA - CUSTO_DAS_MERCADORIAS_E_SERVICOS;

    DESPESAS_OPERACIONAIS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'DESPESAS COM VENDAS')));
    DESPESAS_OPERACIONAIS := DESPESAS_OPERACIONAIS + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'DESPESAS ADMINISTRATIVAS')));
    DESPESAS_OPERACIONAIS := DESPESAS_OPERACIONAIS + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'PRÓ-LABORE')));

    RECEITAS_FINANCEIRAS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'RECEITAS FINANCEIRAS')));
    DESPESAS_FINANCEIRAS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'DESPESAS FINANCEIRAS')));
    DESPESAS_FINANCEIRAS_RECEITAS_FINANCEIRAS := RECEITAS_FINANCEIRAS - DESPESAS_FINANCEIRAS;

    OUTRAS_RECEITAS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'RECEITAS EVENTUAIS')));
    OUTRAS_DESPESAS := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'DESPESAS EVENTUAIS')));
    OUTRAS_RECEITAS_E_DESPESAS := OUTRAS_RECEITAS - OUTRAS_DESPESAS;

    RESULTADO_OPERACIONAL_ANTES_IR_CSLL := RESULTADO_OPERACIONAL_BRUTO - DESPESAS_OPERACIONAIS - DESPESAS_FINANCEIRAS_RECEITAS_FINANCEIRAS - OUTRAS_RECEITAS_E_DESPESAS;

    IR_CSLL := (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'IMPOSTO DE RENDA A PAGAR')));
    IR_CSLL := IR_CSLL + (select coalesce(SUM(valor),0) from lancamentocontabil where idconta in (select id from contas_filhas($1,'CSLL - CONTRIBUIÇÃO SOCIAL SOBRE O LUCRO LÍQUIDO')));

    RESULTADO_LIQUIDO_EXERCICIO := RESULTADO_OPERACIONAL_ANTES_IR_CSLL - IR_CSLL;

    DROP TABLE IF EXISTS DADOS_DRE_TEMP;
    CREATE TEMP TABLE DADOS_DRE_TEMP(
      ordem int,
      valortotal decimal,
      grupo text
    );
    INSERT INTO DADOS_DRE_TEMP VALUES (1, RECEITA_OPERACIONAL_BRUTA, 'RECEITA OPERACIONAL BRUTA');
    INSERT INTO DADOS_DRE_TEMP VALUES (2, DEDUCOES_DA_RECEITA_BRUTA, '(-) DEDUÇÕES DA RECEITA BRUTA');
    INSERT INTO DADOS_DRE_TEMP VALUES (3, RECEITA_OPERACIONAL_LIQUIDA, '= RECEITA OPERACIONAL LÍQUIDA');
    INSERT INTO DADOS_DRE_TEMP VALUES (4, CUSTO_DAS_MERCADORIAS_E_SERVICOS, '(-) CUSTO DAS MERCADORIAS E SERVIÇOS');
    INSERT INTO DADOS_DRE_TEMP VALUES (5, RESULTADO_OPERACIONAL_BRUTO, '= RESULTADO OPERACIONAL BRUTO');
    INSERT INTO DADOS_DRE_TEMP VALUES (6, DESPESAS_OPERACIONAIS, '(-) DESPESAS OPERACIONAIS');
    INSERT INTO DADOS_DRE_TEMP VALUES (7, DESPESAS_FINANCEIRAS_RECEITAS_FINANCEIRAS, '(+/-) DESPESAS FINANCEIRAS/RECEITAS FINANCEIRAS');
    INSERT INTO DADOS_DRE_TEMP VALUES (8, OUTRAS_RECEITAS_E_DESPESAS, '(+/-) OUTRAS RECEITAS E DESPESAS');
    INSERT INTO DADOS_DRE_TEMP VALUES (9, RESULTADO_OPERACIONAL_ANTES_IR_CSLL, '= RESULTADO OPERACIONAL ANTES DO IR E CSLL');
    INSERT INTO DADOS_DRE_TEMP VALUES (10, IR_CSLL, '(-) IR E CSLL');
    INSERT INTO DADOS_DRE_TEMP VALUES (11, RESULTADO_LIQUIDO_EXERCICIO, '= RESULTADO LÍQUIDO DO EXERCÍCIO');
    RETURN QUERY SELECT * FROM DADOS_DRE_TEMP;
END;
$function$;
