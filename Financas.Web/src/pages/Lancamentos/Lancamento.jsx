import { useState, useEffect } from "react";
import api from "../../services/api";
import { useNavigate, useParams, Link } from "react-router-dom";
import "../Lancamentos/Lancamento.css";

function Lancamento() {
  const [erro, setErro] = useState("");
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [data, setData] = useState(new Date().toISOString().split("T")[0]);
  const [tipo, setTipo] = useState(1);
  const [categoriaId, setCategoriaId] = useState("");
  const [contaBancariaId, setContaBancariaId] = useState(0);
  const [cartaoCreditoId, setCartaoCreditoId] = useState(0);

  const [categorias, setCategorias] = useState([]);
  const [contas, setContas] = useState([]);
  const [cartoes, setCartoes] = useState([]);

  const navigate = useNavigate();
  const { id } = useParams();
  const modo = id ? "editar" : "criar";

  useEffect(() => {
    const carregarDadosIniciais = async () => {
      try {
        const [resCat, resContas, resCartoes] = await Promise.all([
          api.get("/categorias/listar-categorias"),
          api.get("/contas-bancarias/listar-conta-bancaria"),
          api.get("/cartoes-credito/listar-cartoes-credito"),
        ]);

        setCategorias(resCat.data);
        setContas(resContas.data);
        setCartoes(resCartoes.data);

        if (id && id !== "undefined") {
          const response = await api.get(
            `/lancamentos/visualizar-lancamento/${id}`,
          );

          const d = response.data;

          setDescricao(d.descricao || "");
          setValor(d.valor || "");

          setData(
            d.data
              ? d.data.split("T")[0]
              : new Date().toISOString().split("T")[0],
          );

          setTipo(d.tipo || 1);

          setCategoriaId(d.categoriaId || "");

          setContaBancariaId(d.contaBancariaId || 0);

          setCartaoCreditoId(d.cartaoCreditoId || 0);
        }
      } catch (error) {
        console.error("Erro de sincronização:", error);
        setErro("Erro ao sincronizar informações com o servidor.");
      }
    };

    carregarDadosIniciais();
  }, [id]);

  const handleSalvar = async (e) => {
    e.preventDefault();
    setErro("");

    const dadosParaEnviar = {
      descricao: descricao,
      valor: Number(valor),
      data: new Date(data).toISOString(),
      tipo: Number(tipo),
      categoriaId: categoriaId ? Number(categoriaId) : null,
      contaBancariaId:
        Number(contaBancariaId) === 0 ? null : Number(contaBancariaId),
      cartaoCreditoId:
        Number(cartaoCreditoId) === 0 ? null : Number(cartaoCreditoId),
    };

    try {
      if (modo === "criar") {
        await api.post("/lancamentos/criar-lancamento", dadosParaEnviar);
      } else {
        await api.patch(
          `/lancamentos/atualizar-lancamentos/${id}`,
          dadosParaEnviar,
        );
      }
      navigate("/dashboard");
    } catch (error) {
      console.error("Erro da API:", error.response?.data);
      setErro("Erro ao processar o lançamento.");
    }
  };

  const handleExcluir = async () => {
  if (window.confirm("Deseja realmente excluir este lançamento?")) {
    try {
      // Alterado de .patch para .delete
      await api.delete(`/lancamentos/deletar-lancamento/${id}`);
      
      // Feedback opcional para o usuário
      alert("Lançamento removido com sucesso!");
      
      // Redireciona para a listagem (Dashboard) após excluir
      navigate("/dashboard");
    } catch (error) {
      console.error("Erro ao excluir:", error);
      setErro("Erro ao excluir o lançamento. Verifique a conexão.");
    }
  }
};

  return (
    <div className="lancamento-page">
      <div className="dashboard-content">
        <div className="lancamento-card">
          <header className="lancamento-header">
            <h1>
              {modo === "criar" ? "Novo Lançamento" : "Gerenciar Lançamento"}
            </h1>
            <p className="descricao-header">
              {modo === "criar"
                ? "Adicione uma nova movimentação financeira."
                : "Revise os dados para alterar ou excluir o registro."}
            </p>
          </header>

          <form onSubmit={handleSalvar} className="lancamentos-box">
            <div className="detalhes-wrapper">
              <label>Descrição</label>
              <input
                type="text"
                className="item-lancamento"
                placeholder="Ex: Aluguel, Supermercado..."
                value={descricao}
                onChange={(e) => setDescricao(e.target.value)}
                required
              />
            </div>

            <div className="grid-form">
              <div className="detalhes-wrapper">
                <label>Valor</label>
                <input
                  type="number"
                  step="0.01"
                  className="item-lancamento"
                  value={valor}
                  onChange={(e) => setValor(e.target.value)}
                  required
                />
              </div>
              <div className="detalhes-wrapper">
                <label>Tipo</label>
                <select
                  className="item-lancamento"
                  value={tipo}
                  onChange={(e) => setTipo(Number(e.target.value))}
                >
                  <option value={1} style={{ background: "#1a1f29" }}>
                    Receita (+)
                  </option>
                  <option value={2} style={{ background: "#1a1f29" }}>
                    Despesa (-)
                  </option>
                </select>
              </div>
            </div>

            <div className="grid-form">
              <div className="detalhes-wrapper">
                <label>Data</label>
                <input
                  type="date"
                  className="item-lancamento"
                  value={data}
                  onChange={(e) => setData(e.target.value)}
                  required
                />
              </div>
              <div className="detalhes-wrapper">
                <label>Categoria</label>
                <select
                  className="item-lancamento"
                  value={categoriaId}
                  onChange={(e) => setCategoriaId(e.target.value)}
                >
                  <option value="" style={{ background: "#1a1f29" }}>
                    Selecione uma categoria
                  </option>
                  {categorias.map((cat) => (
                    <option
                      key={cat.id}
                      value={cat.id}
                      style={{ background: "#1a1f29" }}
                    >
                      {cat.nome}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="grid-form">
              <div className="detalhes-wrapper">
                <label>Conta Bancária</label>
                <select
                  className="item-lancamento"
                  value={contaBancariaId}
                  onChange={(e) => setContaBancariaId(e.target.value)}
                >
                  <option value={0} style={{ background: "#1a1f29" }}>
                    Nenhuma / Dinheiro
                  </option>
                  {contas.map((conta) => (
                    <option
                      key={conta.id}
                      value={conta.id}
                      style={{ background: "#1a1f29" }}
                    >
                      {conta.nome}
                    </option>
                  ))}
                </select>
              </div>
              <div className="detalhes-wrapper">
                <label>Cartão de Crédito</label>
                <select
                  className="item-lancamento"
                  value={cartaoCreditoId}
                  onChange={(e) => setCartaoCreditoId(e.target.value)}
                >
                  <option value={0} style={{ background: "#1a1f29" }}>
                    Nenhum
                  </option>
                  {cartoes.map((cartao) => (
                    <option
                      key={cartao.id}
                      value={cartao.id}
                      style={{ background: "#1a1f29" }}
                    >
                      {cartao.nome}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="acoes-form-container">
              <button type="submit" className="btn-salvar">
                {modo === "criar" ? "Realizar Lançamento" : "Salvar Alterações"}
              </button>

              {modo === "editar" && (
                <button
                  type="button"
                  onClick={handleExcluir}
                  className="btn-deletar"
                >
                  Excluir Registro
                </button>
              )}

              <Link to="/dashboard" className="link-voltar">
                Voltar para o Dashboard
              </Link>
            </div>

            {erro && <p className="mensagem-erro">{erro}</p>}
          </form>
        </div>
      </div>
    </div>
  );
}

export default Lancamento;
