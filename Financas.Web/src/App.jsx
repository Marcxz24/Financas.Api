import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import PrivateRoute from "./routes/PrivateRoute";
import Login from "./pages/Login/Login";
import Register from "./pages/Register/Register";
import Dashboard from "./pages/Dashboard/Dashboard";
import ResumoFinanceiro from "./pages/Dashboard/ResumoFinanceiro";
import Lancamento from "./pages/Lancamentos/Lancamento";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/login" element={<Login />} />
        <Route path="/criar-conta" element={<Register />} />

        {/* Agrupamos tudo que precisa de proteção e do layout do Dashboard aqui */}
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <Dashboard />
            </PrivateRoute>
          }
        >
          {/* Estas são as rotas filhas que aparecem no <Outlet /> */}
          <Route index element={<ResumoFinanceiro />} />
          <Route path="lancamento" element={<Lancamento />} />
          <Route path="lancamento/:id" element={<Lancamento />} />
        </Route>

        <Route path="*" element={<Navigate to="/" />} />
      </Routes>
    </BrowserRouter>
  );
}

// Exporta o componente App para ser o ponto de entrada da interface
export default App;
