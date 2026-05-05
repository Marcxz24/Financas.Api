import { useEffect, useState } from "react";
import api from "../../services/api";
import "./Dashboard.css";
import { useNavigate } from "react-router-dom";

function Dashboard() {
  const navigate = useNavigate();

  // Estado que armazena os dados financeiros vindos da API
  const [resumo, setResumo] = useState({
    totalReceitas: 0,
    totalDespesas: 0,
    saldoMensal: 0,
    saldoBancarioTotal: 0,
    ultimosLancamentos: [],
  });

  // Efeito para buscar os dados ao carregar a página
  useEffect(() => {
    const fetchDados = async () => {
      try {
        // Chamada ao endpoint de resumo mensal
        const response = await api.get("/api/Dashboard/resumo-mensal");
        setResumo(response.data);
      } catch (error) {
        console.error("Erro ao buscar dados do dashboard:", error);
        // Redireciona para login caso o token seja inválido (401)
        if (error.response?.status === 401) {
          navigate("/login");
        }
      }
    };

    fetchDados();
  }, [navigate]);

  // Remove o token e desloga o usuário
  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div className="dashboard-page">
      {/* Cabeçalho com Logo e Navegação */}
      <header className="dashboard-header">
        <div className="logo-area">
          <h1>Finanças</h1>
          <span>Painel Financeiro</span>
        </div>
        <nav className="menu-top">
          <a href="#">Dashboard</a>
          <a href="#">Transações</a>
          <a href="#">Relatórios</a>
          <button className="btn-logout" onClick={handleLogout}>
            Sair
          </button>
        </nav>
      </header>

      <main className="dashboard-content">
        {/* Seção de Cards com os Totais principais */}
        <section className="resumo-cards">
          <div className="card saldo">
            <h3>Saldo Bancário Total</h3>
            <p>
              R${" "}
              {resumo.saldoBancarioTotal.toLocaleString("pt-BR", {
                minimumFractionDigits: 2,
              })}
            </p>
          </div>

          <div className="card receitas">
            <h3>Receitas (Mês)</h3>
            <p className="positivo">
              + R${" "}
              {resumo.totalReceitas.toLocaleString("pt-BR", {
                minimumFractionDigits: 2,
              })}
            </p>
          </div>

          <div className="card despesas">
            <h3>Despesas (Mês)</h3>
            <p className="negativo">
              - R${" "}
              {resumo.totalDespesas.toLocaleString("pt-BR", {
                minimumFractionDigits: 2,
              })}
            </p>
          </div>
        </section>

        {/* Listagem dinâmica das últimas transações */}
        <section className="ultimas-transacoes">
          <h2>Últimas Transações</h2>
          <div className="transacoes-box">
            {resumo.ultimosLancamentos.length > 0 ? (
              resumo.ultimosLancamentos.map((item, index) => (
                <div key={index} className="item-transacao">
                  <div className="info-esquerda">
                    {/* Badge para exibição da data formatada */}
                    <div className="data-badge">
                      {item.dataLancamento || item.data
                        ? new Date(item.dataLancamento || item.data).toLocaleDateString("pt-BR")
                        : "00/00/0000"}
                    </div>

                    {/* Detalhes do lançamento: Descrição, Tipo e Conta */}
                    <div className="detalhes-texto">
                      <strong className="descricao-principal">
                        {item.descricao}
                      </strong>
                      <div className="sub-info">
                        <span className={`tag-tipo ${item.tipo === "Receita" ? "txt-receita" : "txt-despesa"}`}>
                          {item.tipo}
                        </span>
                        <span className="separador">•</span>
                        <span className="conta-nome">
                          {item.contaBancariaNome || "Sem conta"}
                        </span>
                      </div>
                    </div>
                  </div>

                  {/* Valor do lançamento com cor dinâmica */}
                  <div className="info-direita">
                    <strong className={item.tipo === "Receita" ? "positivo" : "negativo"}>
                      {item.tipo === "Receita" ? "+ " : "- "}
                      R${" "}
                      {item.valor.toLocaleString("pt-BR", {
                        minimumFractionDigits: 2,
                      })}
                    </strong>
                  </div>
                </div>
              ))
            ) : (
              <p className="empty-msg">Nenhuma transação encontrada.</p>
            )}
          </div>
        </section>
      </main>
    </div>
  );
}

export default Dashboard;