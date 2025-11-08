using System;

public class Projeto
{
    // Básicos
    public int id;
    public string nome;
    public string gerente;
    public string sponsor;

    // Custos e prazos
    public decimal orcamentoAprovado;  // R$
    public decimal custoReal;          // R$
    public DateTime? prazoInicial;     // Data planejada
    public DateTime? novoPrazo;        // Data replanejada
    public int progresso;              // 0..100 (%)

    // Critérios de avaliação (TODOS 0..100)
    public int roi;            // 0..100
    public int risco;          // 0..100
    public int alinhamento;    // 0..100
    public int urgencia;       // 0..100

    // Derivados
    public int score;          // 0..100 (média ponderada)
    public string aprovacao;   // "Arovado" / "Não Aprovado"
    public string status;      // "Em Análise" / "Em Execução" / "Encerrado"

    public Projeto()
    {
        this.nome = "";
        this.gerente = "";
        this.sponsor = "";
        this.status = "Em Análise";
        this.progresso = 0;
    }
public void RecalcularScoreEAprovacao()
{
    score = (alinhamento * 2
           + roi         * 3
           + urgencia    * 3
           + risco       * 2) / 10;

    aprovacao = (score >= 70) ? "Aprovado" : "Não Aprovado";
}
    public string DesvioCustoTexto()
    {
        if (orcamentoAprovado <= 0 || custoReal <= 0) return "";
        decimal desvio = (custoReal - orcamentoAprovado) / orcamentoAprovado * 100m;
        return (desvio >= 0 ? "+" : "") + Math.Round(desvio, 1) + "%";
    }

    public string DesvioPrazoTexto()
    {
        if (!prazoInicial.HasValue || !novoPrazo.HasValue) return "";
        int dias = (novoPrazo.Value - prazoInicial.Value).Days;
        if (dias == 0) return "0 dia";
        return (dias > 0 ? "+" : "") + Math.Abs(dias) + (Math.Abs(dias) == 1 ? " dia" : " dias");
    }
}
