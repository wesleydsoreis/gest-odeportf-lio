using System;

public class Projeto
{
    
    public int id;
    public string nome;
    public string gerente;
    public string sponsor;

    public decimal orcamentoAprovado;  
    public decimal custoReal;          
    public DateTime? prazoInicial;    
    public DateTime? novoPrazo;        
    public int progresso;             

    
    public int roi;            
    public int risco;          
    public int alinhamento;    
    public int urgencia;      


    public int score;          
    public string aprovacao; 
    public string status;    

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
