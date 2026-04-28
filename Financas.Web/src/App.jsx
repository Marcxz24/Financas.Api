import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";
import Dashboard from "./pages/Dashboard/Dashboard";

function App() {
  return (
    // Componente que habilita o histórico de navegação do navegador no React
    <BrowserRouter>
      {/* Container que envolve todas as definições de rotas da aplicação */}
      <Routes>
        {/* Define que o componente Login será renderizado no caminho raiz do site */}
        <Route path="/" element={<Login />} />
        
        {/* Define o caminho explícito para a tela de Login */}
        <Route path="/login" element={<Login />} />
        
        {/* Define o caminho para a tela de registro/criação de conta */}
        <Route path="/criar-conta" element={<Register />} />

        {/* Define o caminho para a tela principal (Dashboard) após a autenticação */}
        <Route path="/dashboard" element={<Dashboard />} />

        {/* Captura qualquer URL não definida e redireciona o usuário para a raiz (Login) */}
        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}

// Exporta o componente App para ser o ponto de entrada da interface
export default App;