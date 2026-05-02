import { useState } from "react";
// Importa a instância configurada do cliente HTTP (Axios)
import api from "../../services/api";
// Importa o arquivo de estilos CSS específico deste componente
import "./login.css";
// Importa o componente Link para navegação entre rotas
import { Link , useNavigate} from "react-router-dom";

function Login() {
  // 
  const [erro, setErro] = useState("");

  // Define o estado para armazenar o valor do campo de e-mail
  const [email, setEmail] = useState("");
  // Define o estado para armazenar o valor do campo de senha
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  // Define a função assíncrona que processa a submissão do formulário
  const handleLogin = async (e) => {
  e.preventDefault();

  console.log("1 - Clique detectado");

  setErro("");

  try {
    const response = await api.post("/api/usuarios/login", {
      email,
      password,
    });

    console.log("2 - API respondeu sucesso");
    console.log("response.data:", response.data);

    localStorage.setItem(
      "token",
      response.data.token || response.data
    );

    console.log("3 - Token salvo");
    console.log("4 - Navegando dashboard");

    navigate("/dashboard");

    } catch (error) {
      console.log("X - Caiu no catch");

      console.error(error);

      setErro(
        error.response?.data ||
        "Erro ao realizar login."
      );
    }
  };

  return (
    // Container principal da página de login
    <div className="login-page">
      {/* Container do cartão central de login */}
      <div className="login-card">
        
        {/* Cabeçalho contendo o título e a descrição */}
        <header className="login-header">
          <h1>Finanças</h1>
          <p className="descricao-login">
            Faça login para acessar sua conta e gerenciar suas finanças de forma fácil e segura.
          </p>
        </header>

        {/* Formulário que dispara a função handleLogin ao ser submetido */}
        <form onSubmit={handleLogin}>
          {/* Campo de entrada para o e-mail com vínculo ao estado correspondente */}
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          {/* Campo de entrada para a senha com vínculo ao estado correspondente */}
          <input
            type="password"
            placeholder="Senha"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          {/* Botão de envio do formulário */}
          <button type="submit">Entrar</button>
          {erro && <p className="mensagem-erro">{erro}</p>}
        </form>

        {/* Seção inferior para redirecionamento à página de cadastro */}
        <header className="criar-conta">
          <p>
            Não tem uma conta?{" "}
            {/* Link que aponta para a rota de criação de conta */}
            <Link to="/criar-conta">Crie uma aqui</Link>
          </p>
        </header>
      </div>
    </div>
  );
}

// Exporta o componente Login para ser utilizado em outras partes da aplicação
export default Login;