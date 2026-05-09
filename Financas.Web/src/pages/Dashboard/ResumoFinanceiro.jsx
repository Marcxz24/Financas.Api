import { useState, useEffect } from "react";
import api from "../../services/api";
import "./dashboard.css";

function ResumoFinanceiro() {
  // Estado para armazenar a lista de últimas transações
  const [transacoes, setTransacoes] = useState([]);
  
  // Estado para armazenar os valores totais do dashboard
  const [resumo, setResumo] = useState({
    totalReceitas: 0,
    totalDespesas: 0,
    saldoBancarioTotal: 0,
  });

  // Carrega os dados do dashboard ao montar o componente
  useEffect(() => {
    const carregarDados = async () => {
      try {
        // Chamada ao endpoint que retorna o compilado mensal
        const response = await api.get("/Dashboard/resumo-mensal");
        const dados = response.data;

        // Atualiza a lista de lançamentos e os cards de resumo
        setTransacoes(dados.ultimosLancamentos || []);
        setResumo({
          totalReceitas: dados.totalReceitas || 0,
          totalDespesas: dados.totalDespesas || 0,
          saldoBancarioTotal: dados.saldoBancarioTotal || 0,
        });
      } catch (error) {
        // Log de erro no console em caso de falha na comunicação com a API
        console.error("Erro ao carregar dados do suporte:", error);
      }
    };

    carregarDados();
  }, []);

  return (
    <>
      {/* Seção de Cards de Resumo */}
      <div className="resumo-cards">
        <div className="card">
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
      </div>

      {/* Seção da Listagem de Transações Recentes */}
      <div className="ultimas-transacoes">
        <h2>Últimas Transações</h2>
        <div className="transacoes-box">
          {transacoes.length > 0 ? (
            transacoes.map((t, index) => (
              <div key={index} className="item-transacao">
                <div className="info-esquerda">
                  {/* Bloco de Data e Hora: Formata a data ISO vinda do banco */}
                  <div className="data-wrapper">
                    <span className="data-txt">
                      {new Date(t.dataLancamento).toLocaleDateString("pt-BR")}
                    </span>
                    <span className="hora-txt">
                      {" "}
                      {new Date(t.dataLancamento).toLocaleTimeString("pt-BR", {
                        hour: "2-digit",
                        minute: "2-digit",
                      })}
                    </span>
                  </div>

                  {/* Bloco de Descrição e Detalhes de Origem */}
                  <div className="detalhes-wrapper">
                    <div className="desc-linha">
                      <strong className="desc-txt">{t.descricao}</strong>
                    </div>
                    
                    <div className="origem-info">
                      {/* Exibe o nome da conta bancária ou fallback 'Nenhuma' */}
                      <div className="info-item">
                        <small>Conta: </small> 
                        <span className="origem-txt">
                          {t.contaBancariaNome || "Nenhuma"}
                        </span>
                      </div>

                      {/* Exibe o nome do cartão ou fallback 'Nenhum' */}
                      <div className="info-item">
                        <small>Cartão: </small> 
                        <span className="origem-txt">
                          {t.cartaoCreditoNome || "Nenhum"}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Valor da transação com cor dinâmica baseada no tipo */}
                <span
                  className={t.tipo === "Receita" ? "positivo" : "negativo"}
                >
                  {t.tipo === "Receita" ? "+" : "-"} R${" "}
                  {t.valor.toLocaleString("pt-BR", {
                    minimumFractionDigits: 2,
                  })}
                </span>
              </div>
            ))
          ) : (
            // Mensagem caso a API retorne uma lista vazia
            <p className="empty-msg">
              Nenhuma transação encontrada no momento.
            </p>
          )}
        </div>
      </div>
    </>
  );
}

export default ResumoFinanceiro;