import { useEffect, useState } from "react";
import api from "../../services/api";
import "./dashboard.css";

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

  return (
    <div className="dashboard-page">
      {/* Barra lateral ou de topo pode ser adicionada aqui depois */}
      <nav className="dashboard-nav">
        <h1>Minhas Finanças</h1>
        <button className="btn-sair">Sair</button>
      </nav>

      <main className="dashboard-content">
        <section className="resumo-cards">
          {/* Card de Saldo Total */}
          <div className="card saldo">
            <h3>Saldo Atual</h3>
            <p>R$ {resumo.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
          </div>

          {/* Card de Receitas */}
          <div className="card receitas">
            <h3>Receitas</h3>
            <p>+ R$ {resumo.receitas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
          </div>

          {/* Card de Despesas */}
          <div className="card despesas">
            <h3>Despesas</h3>
            <p>- R$ {resumo.despesas.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</p>
          </div>
        </section>

        <section className="ultimas-transacoes">
          <h2>Últimas Transações</h2>
          <div className="tabela-transacoes">
            {/* Aqui entrará um map das transações reais da API */}
            <p>Nenhuma transação encontrada.</p>
          </div>
        </section>
      </main>
    </div>
  );
}

export default Dashboard;