// Removidos: useState, useEffect e api
import "./dashboard.css";
// Mantemos apenas o que é essencial para o layout e navegação
import { Link, useNavigate, Outlet } from "react-router-dom";

function Dashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    // Remove o token para deslogar
    localStorage.removeItem("token");
    // Redireciona para a tela de login
    navigate("/");
  };

  return (
    <div className="dashboard-page">
      <header className="dashboard-header">
        <div className="logo-area">
          <h1>Finanças</h1>
          <span>Painel Financeiro</span>
        </div>
        <nav className="menu-top">
          {/* Links de navegação interna */}
          <Link to="/dashboard">Dashboard</Link>
          <Link to="/dashboard/lancamento">Transações</Link>
          <Link to="#">Relatórios</Link>
          <button className="btn-logout" onClick={handleLogout}>
            Sair
          </button>
        </nav>
      </header>

      <main className="dashboard-content">
        {/* Aqui o React Router injeta o ResumoFinanceiro ou o Lancamento */}
        <Outlet />
      </main>
    </div>
  );
}

export default Dashboard;