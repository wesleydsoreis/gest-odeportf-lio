using System;

public class Projeto
{
    // ==========================
    //   CAMPOS / PROPRIEDADES
    // ==========================
    public int id { get; set; }
    public string nome { get; set; } = "";
    public string gerente { get; set; } = "";
    public string sponsor { get; set; } = "";

    public decimal orcamentoAprovado { get; set; }  // R$
    public decimal custoReal { get; set; }          // R$

    public DateTime? prazoInicial { get; set; }
    public DateTime? novoPrazo { get; set; }

    public int roi { get; set; }          // 0..100
    public int risco { get; set; }        // 0..100 (maior = mais arriscado)
    public int alinhamento { get; set; }  // 0..100
    public int urgencia { get; set; }     // 0..100

    public int score { get; set; }
    public string aprovacao { get; set; } = "—";
    public string status { get; set; } = "Em Análise";
    public int progresso { get; set; }    // 0..100

    // ==========================
    //   MÉTODOS DE CÁLCULO
    // ==========================

    // Recalcula o score e a classificação de aprovação.
    public void RecalcularScoreEAprovacao()
{
    // --- cálculo do score ---
    score = (roi + (100 - risco) + alinhamento + urgencia) / 4;

    // --- nível de aprovação ---
    if (score >= 85)
        aprovacao = "Alto";
    else if (score >= 70)
        aprovacao = "Médio";
    else
        aprovacao = "Baixo";

    // --- definição automática de status ---
    if (score >= 70)
        status = "Aprovado";
    else
        status = "Em Análise";
}


    // Percentual de desvio de custo em relação ao orçamento aprovado.
    // > 0 = acima do orçamento; < 0 = abaixo.
    public double DesvioCustoPercentual()
    {
        if (orcamentoAprovado <= 0 || custoReal <= 0)
            return 0;

        return (double)(custoReal - orcamentoAprovado) / (double)orcamentoAprovado * 100.0;
    }

    // Texto amigável para o desvio de custo.
    public string DesvioCustoTexto()
    {
        double d = DesvioCustoPercentual();

        if (Math.Abs(d) < 0.0001) return "Sem desvio";
        if (d > 0)                 return $"+{d:0.0}% acima";
        return $"{d:0.0}% abaixo";
    }

    // Texto amigável para o desvio de prazo, comparando novoPrazo com prazoInicial.
    public string DesvioPrazoTexto()
    {
        if (!prazoInicial.HasValue && !novoPrazo.HasValue)
            return "Sem dados";

        if (prazoInicial.HasValue && novoPrazo.HasValue)
        {
            int dias = (novoPrazo.Value.Date - prazoInicial.Value.Date).Days;
            if (dias == 0) return "Dentro do prazo";
            if (dias > 0)  return $"+{dias} dia(s) de atraso";
            return $"{dias} dia(s) adiantado";
        }

        return "Sem dados";
    }
}
