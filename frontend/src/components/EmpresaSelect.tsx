import { useEffect, useState } from "react";
import { Select } from "antd";
import { createCrudApi } from "../api/crudFactory";

const cliforApi = createCrudApi("cliFor");

interface Props {
  value?: string;
  onChange: (value: string) => void;
  style?: React.CSSProperties;
}

/**
 * A maioria dos módulos de negócio (financeiro, contábil, patrimônio) é
 * escopada por cliente/empresa (campo `cliforid`, herdado do sistema
 * original). Este seletor é reutilizado em todas essas telas.
 */
export default function EmpresaSelect({ value, onChange, style }: Props) {
  const [options, setOptions] = useState<{ value: string; label: string }[]>([]);

  useEffect(() => {
    cliforApi.list().then((list: any[]) => {
      setOptions(list.map((c) => ({ value: c.id, label: c.nome ?? c.id })));
    });
  }, []);

  return (
    <Select
      placeholder="Selecione a empresa/cliente"
      style={{ width: 280, ...style }}
      value={value}
      onChange={onChange}
      options={options}
      showSearch
      optionFilterProp="label"
    />
  );
}
