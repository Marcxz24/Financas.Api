import { Navigate } from "react-router-dom";

function PrivateRoute({ children }) {
    // Busca o Token salvo após o Login;
    const token = localStorage.getItem("token");

    // Se não existir o token retorna para a página do login.
    if (!token) {
        return <Navigate to="/login" replace/>
    }

    // Se existir token libera a página.
    return children;
}

export default PrivateRoute;