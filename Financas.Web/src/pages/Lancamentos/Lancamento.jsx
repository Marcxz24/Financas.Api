import { useState, useEffect } from "react";
import api from "../../services/api";
import "./Lancamento.css";
import { useNavigate, useParams, Link } from "react-router-dom";

function Lancamento() {
  // Estados para mensagens de erro e navegação
  const [erro, setErro] = useState("");
  const navigate = useNavigate();
  const { id } = useParams();

  // Define se o formulário está em modo de criação ou edição baseado no ID da URL
  const modo = id ? "editar" : "criar";

  // Estados dos campos do formulário
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [data, setData] = useState(new Date().toISOString().split("T")[0]);
  const [tipo, setTipo] = useState(1);
  const [categoriaId, setCategoriaId] = useState("");
  const [contaBancariaId, setContaBancariaId] = useState(0);
  const [cartaoCreditoId, setCartaoCreditoId] = useState(0);

  // Estados para armazenar as listas de opções carregadas da API
  const [categorias, setCategorias] = useState([]);
  const [contas, setContas] = useState([]);
  const [cartoes, setCartoes] = useState([]);

  // useEffect para carregar os dados iniciais ao abrir a página
  useEffect(() => {
    const carregarDadosIniciais = async () => {
      try {
        // Busca as listas de categorias, contas e cartões simultaneamente
        const [resCat, resContas, resCartoes] = await Promise.all([
          api.get("/categorias/listar-categorias"),
          api.get("/contas-bancarias/listar-conta-bancaria"),
          api.get("/cartoes-credito/listar-cartoes-credito"),
        ]);

        setCategorias(resCat.data);
        setContas(resContas.data);
        setCartoes(resCartoes.data);

        // Se houver um ID, carrega os dados do lançamento para edição
        if (id) {
          const response = await api.get(
            `/lancamentos/visualizar-lancamentos/${id}`,
          );
          const d = response.data;
          setDescricao(d.descricao);
          setValor(d.valor);
          setData(d.data.split("T")[0]);
          setTipo(d.tipo);
          setCategoriaId(d.categoriaId);
          setContaBancariaId(d.contaBancariaId || 0);
          setCartaoCreditoId(d.cartaoCreditoId || 0);
        }
      } catch {
        setErro("Erro ao sincronizar informações com o servidor.");
      }
    };
    carregarDadosIniciais();
  }, [id]);

  // Função para processar o envio do formulário (Salvar/Atualizar)
  const handleSalvar = async (e) => {
    e.preventDefault();
    setErro("");

    // Montagem do objeto de dados com conversões necessárias para o Backend C#
    const dadosParaEnviar = {
      descricao: descricao,
      valor: Number(valor),
      data: new Date(data).toISOString(),
      tipo: Number(tipo),
      // Converte para null se não houver categoria selecionada
      categoriaId: categoriaId ? Number(categoriaId) : null,
      // Garante que IDs 0 sejam enviados como null para campos opcionais
      contaBancariaId:
        Number(contaBancariaId) === 0 ? null : Number(contaBancariaId),
      cartaoCreditoId:
        Number(cartaoCreditoId) === 0 ? null : Number(cartaoCreditoId),
    };

    try {
      if (modo === "criar") {
        await api.post("/lancamentos/criar-lancamento", dadosParaEnviar);
      } else {
        await api.put(`/Lancamento/${id}`, {
          ...dadosParaEnviar,
          id: Number(id),
        });
      }
      navigate("/dashboard");
    } catch (error) {
      // Exibe logs no console e trata o erro para exibição na tela sem quebrar o React
      console.error("Erro da API:", error.response?.data);

      const apiErro = error.response?.data;
      let mensagemExibivel = "Erro ao processar o lançamento.";

      if (typeof apiErro === "object" && apiErro !== null) {
        mensagemExibivel =
          apiErro.title || JSON.stringify(apiErro.errors) || "Dados inválidos.";
      }

      setErro(mensagemExibivel);
    }
  };

  // Função para excluir o lançamento atual
  const handleExcluir = async () => {
    if (window.confirm("Deseja realmente excluir este lançamento?")) {
      try {
        await api.delete(`/Lancamento/${id}`);
        navigate("/dashboard");
      } catch {
        setErro("Erro ao excluir o lançamento.");
      }
    }
  };

  return (
    <div className="lancamento-page">
      <div className="lancamento-card">
        <header className="lancamento-header">
          <h1>{modo === "criar" ? "Novo Lançamento" : "Editar Lançamento"}</h1>
          <p className="descricao-header">
            {modo === "criar"
              ? "Adicione uma nova movimentação financeira."
              : "Altere os detalhes do lançamento selecionado."}
          </p>
        </header>

        <form onSubmit={handleSalvar}>
          <div className="select-group">
            <label>Descrição:</label>
            <input
              type="text"
              placeholder="Descrição"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              required
            />
          </div>

          <div className="select-group">
            <label>Valor:</label>
            <input
              type="number"
              step="0.01"
              placeholder="Valor (0,00)"
              value={valor}
              onChange={(e) => setValor(e.target.value)}
              required
            />
          </div>

          <div className="select-group">
            <label>Tipo:</label>
            <select
              value={tipo}
              onChange={(e) => setTipo(Number(e.target.value))}
            >
              <option value={1}>Receita (+)</option>
              <option value={2}>Despesa (-)</option>
            </select>
          </div>

          <div className="select-group">
            <label>Data:</label>
            <input
              type="date"
              value={data}
              onChange={(e) => setData(e.target.value)}
              required
            />
          </div>

          <div className="select-group">
            <label>Categoria (Opcional):</label>
            <select
              value={categoriaId}
              onChange={(e) => setCategoriaId(e.target.value)}
            >
              <option value="">Selecione uma categoria</option>
              {categorias.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.nome}
                </option>
              ))}
            </select>
          </div>

          <div className="select-group">
            <label>Conta Bancária (Opcional):</label>
            <select
              value={contaBancariaId}
              onChange={(e) => setContaBancariaId(e.target.value)}
            >
              <option value={0}>Nenhuma / Dinheiro</option>
              {contas.map((conta) => (
                <option key={conta.id} value={conta.id}>
                  {conta.nome}
                </option>
              ))}
            </select>
          </div>

          <div className="select-group">
            <label>Cartão de Crédito (Opcional):</label>
            <select
              value={cartaoCreditoId}
              onChange={(e) => setCartaoCreditoId(e.target.value)}
            >
              <option value={0}>Nenhum</option>
              {cartoes.map((cartao) => (
                <option key={cartao.id} value={cartao.id}>
                  {cartao.nome}
                </option>
              ))}
            </select>
          </div>

          <div className="acoes-form">
            <button type="submit" className="btn-salvar">
              {modo === "criar" ? "Realizar Lançamento" : "Salvar Alterações"}
            </button>
            {modo === "editar" && (
              <button
                type="button"
                className="btn-deletar"
                onClick={handleExcluir}
              >
                Excluir Registro
              </button>
            )}
          </div>
          {erro && <p className="mensagem-erro">{erro}</p>}
        </form>

        <footer className="voltar-dashboard">
          <Link to="/dashboard">Voltar para o Dashboard</Link>
        </footer>
      </div>
    </div>
  );
}

export default Lancamento;