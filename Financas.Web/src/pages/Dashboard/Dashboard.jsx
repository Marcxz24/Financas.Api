import { useEffect, useState } from "react";
import api from "../../services/api";
import "./Dashboard.css";
import { Navigate } from "react-router-dom";

function Dashboard() {
  // Estado para armazenar os dados financeiros que virão da API
  const [resumo, setResumo] = useState({
    saldo: 0,
    receitas: 0,
    despesas: 0
  });

  // useEffect para buscar os dados assim que a página carregar
  useEffect(() => {
    const fetchDados = async () => {
      try {
        // Exemplo de chamada para seu endpoint de finanças no C#
        const response = await api.get("/api/transacoes/resumo");
        setResumo(response.data);
      } catch (error) {
        console.error("Erro ao buscar dados do dashboard:", error);
      }
    };

    fetchDados();
  }, []);

  const HandleLogout = () => {
    localStorage.removeItem("token")
    Navigate("/login");
  }

  return (
    <div className="dashboard-page">

    {/* Topbar */}
    <header className="dashboard-header">
      <div className="logo-area">
        <h1>Finanças</h1>
        <span>Painel Financeiro</span>
      </div>

      <button className="btn-logout" onClick={HandleLogout}>
        Sair
      </button>
    </header>

    {/* Conteúdo */}
    <main className="dashboard-content">

      {/* Cards */}
      <section className="resumo-cards">

        <div className="card saldo">
          <h3>Saldo Atual</h3>
          <p>
            R$ {resumo.saldo.toLocaleString("pt-BR", {
              minimumFractionDigits: 2,
            })}
          </p>
        </div>

        <div className="card receitas">
          <h3>Receitas</h3>
          <p>
            + R$ {resumo.receitas.toLocaleString("pt-BR", {
              minimumFractionDigits: 2,
            })}
          </p>
        </div>

        <div className="card despesas">
          <h3>Despesas</h3>
          <p>
            - R$ {resumo.despesas.toLocaleString("pt-BR", {
              minimumFractionDigits: 2,
            })}
          </p>
        </div>

      </section>

      {/* Transações */}
      <section className="ultimas-transacoes">
        <h2>Últimas Transações</h2>

        <div className="transacoes-box">
          <p>Nenhuma transação encontrada.</p>
        </div>
      </section>

    </main>
  </div>
  );
}

export default Dashboard;