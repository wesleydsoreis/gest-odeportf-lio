using System;
using System.Collections.Generic;
using System.Linq;

public class ProjetoCRUD
{
    private readonly Tela tela;
    private readonly List<Projeto> projetos = new List<Projeto>();

    public ProjetoCRUD(Tela tela)
    {
        this.tela = tela;
        // Adicionando alguns projetos de exemplo para testes
        projetos.Add(new Projeto { id = 1001, nome = "Expansao Digital", gerente = "Alice", sponsor = "Bob", orcamentoAprovado = 150000m, prazoInicial = new DateTime(2025, 12, 31), roi = 85, risco = 20, alinhamento = 90, urgencia = 70, status = "Em Execução", progresso = 50 });
        projetos.Add(new Projeto { id = 1002, nome = "Migracao Cloud", gerente = "Carlos", sponsor = "Daniela", orcamentoAprovado = 50000m, prazoInicial = new DateTime(2025, 10, 15), roi = 60, risco = 50, alinhamento = 70, urgencia = 60, status = "Em Análise", progresso = 0 });
        projetos.Add(new Projeto { id = 1003, nome = "Sistema CRM", gerente = "Eduardo", sponsor = "Fernanda", orcamentoAprovado = 80000m, prazoInicial = new DateTime(2026, 03, 01), roi = 95, risco = 10, alinhamento = 95, urgencia = 90, status = "Encerrado", progresso = 100 });
        projetos.ForEach(p => p.RecalcularScoreEAprovacao());
    }

    public void Executar()
    {
        while (true)
        {
            tela.PrepararTela("Project Portfolio Management - Projetos");
            var ops = new List<string>
            {
                "      Menu Projetos        ",
                "1 - Cadastrar Projeto      ",
                "2 - Exibir Projetos        ",
                "3 - Dashboard de Projetos  ",
                "4 - Fechamento Formal      ",
                "0 - Voltar                 "
            };
            string op = tela.MostrarMenu(ops, 2, 2);

            if (op == "0") break;
            else if (op == "1") CadastrarProjeto();
            else if (op == "2") ListarProjetos();
            else if (op == "3") DashboardProjetos();
            else if (op == "4") FechamentoFormal();
            else
            {
                tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private void CadastrarProjeto()
    {
        // Largura do quadro ajustada para 83 (76 + 7)
        int x = 2, y = 1, w = 83, h = 28;
        DesenhaQuadro(x, y, w, h, "Cadastrar Projeto");

        int colRot = x + 3;
        int colInp = x + 28;

        int lin = y + 3;
        Texto(colRot, lin++, "Nome:");
        Texto(colRot, lin++, "ID:");
        Texto(colRot, lin++, "Gerente de Projeto:");
        Texto(colRot, lin++, "Sponsor:");
        Texto(colRot, lin++, "Orçamento Aprovado($):");
        Texto(colRot, lin++, "Prazo (dd/mm/aaaa):");

        Texto(colRot, lin++, "ROI(%):");
        Texto(colRot, lin++, "Risco:");
        Texto(colRot, lin++, "Alinhamento Estratégico:");
        Texto(colRot, lin++, "Urgência:");

        lin++;
        int linScore = lin; Texto(colRot, lin++, "Score:");
        int linApr = lin; Texto(colRot, lin++, "Aprovação:");
        int linStat = lin; Texto(colRot, lin++, "Status:");

        lin++;
        int linSalvar = lin; Texto(colRot, linSalvar, "Salvar cadastro?(S/N): ");

        string nome;
        while (true)
        {
            nome = LerNaPos(colInp, y + 3);
            if (!ExisteNome(nome)) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(colInp, y + 4, "! Projeto já cadastrado. Insira outro nome !");
            Console.ResetColor();
            Texto(colInp, y + 3, new string(' ', 40));
        }
        Texto(colInp, y + 4, new string(' ', 45));

        int id;
        while (true)
        {
            string idStr = LerNaPos(colInp, y + 4);
            if (int.TryParse(idStr, out id) && !ExisteId(id)) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(colInp, y + 5, "! ID inválido ou já cadastrado. Insira outro ID !");
            Console.ResetColor();
            Texto(colInp, y + 4, new string(' ', 20));
        }
        Texto(colInp, y + 5, new string(' ', 50));

        string gerente = LerNaPos(colInp, y + 5);
        string sponsor = LerNaPos(colInp, y + 6);

        decimal orcamento;
        while (true)
        {
            string s = LerNaPos(colInp, y + 7);
            if (decimal.TryParse(s, out orcamento)) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(colInp, y + 8, "Valor inválido.");
            Console.ResetColor();
            Texto(colInp, y + 7, new string(' ', 30));
        }
        Texto(colInp, y + 8, new string(' ', 30));

        DateTime prazoInicial;
        while (true)
        {
            string s = LerNaPos(colInp, y + 8);
            if (DateTime.TryParse(s, out prazoInicial)) break;

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(colInp, y + 9, "Data inválida.");
            Console.ResetColor();
            Texto(colInp, y + 8, new string(' ', 30));
        }
        Texto(colInp, y + 9, new string(' ', 30));

        int roi = Ler0a100NaPos(colInp, y + 9);
        int risco = Ler0a100NaPos(colInp, y + 10);
        int alinh = Ler0a100NaPos(colInp, y + 11);
        int urg = Ler0a100NaPos(colInp, y + 12);

        var p = new Projeto
        {
            id = id,
            nome = nome,
            gerente = gerente,
            sponsor = sponsor,
            orcamentoAprovado = orcamento,
            prazoInicial = prazoInicial,
            roi = roi,
            risco = risco,
            alinhamento = alinh,
            urgencia = urg,
            status = "Em Análise",
            custoReal = 0,
            novoPrazo = null,
            progresso = 0
        };
        p.RecalcularScoreEAprovacao();

        Texto(colInp, linScore, p.score.ToString());
        Texto(colInp, linApr, p.aprovacao);
        Texto(colInp, linStat, p.status);

        Console.SetCursorPosition(colRot + "Salvar cadastro?(S/N): ".Length, linSalvar);
        string salvar = Console.ReadLine() ?? "N";
        if (salvar.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            projetos.Add(p);
            Texto(colRot, linSalvar + 1, "Projeto salvo.");
        }
        else
        {
            Texto(colRot, linSalvar + 1, "Cadastro cancelado.");
        }

        Console.ReadKey();
    }

    private void ListarProjetos()
    {
        tela.PrepararTela("Project Portfolio Management - Exibir Projetos");
        
        // --- Constantes de Alinhamento ---
        const int W_ID = 8;        
        const int W_NOME = 19;     
        const int W_STATUS = 13;   
        const int W_APROV = 15;    
        const int W_SCORE = 7;     
        const int W_PROG = 11;     
        const int W_PRAZO = 10;    

        // O conteúdo da tabela começará na coluna 2 (margem de 1 espaço).
        int col = 2; 
        int lin = 3; 

        // 1. Cabeçalho formatado
        string formatHeader = 
            $"{"ID",-W_ID}" +
            $"{"NOME",-W_NOME}" +
            $"{"STATUS",-W_STATUS}" +
            $"{"APROVAÇÃO",-W_APROV}" +
            $"{"SCORE",-W_SCORE}" +
            $"{"PROGRESSO",-W_PROG}" +
            $"{"PRAZO",-W_PRAZO}";
        
        // Exibir o cabeçalho
        Texto(col, lin++, formatHeader);
        
        // 2. Linha separadora
        // CORREÇÃO: Define a largura para 83, que é o espaço entre a coluna 2 e a coluna 84 (1 espaço da margem direita).
        const int SEPARATOR_WIDTH = 83;
        string separator = new string('═', SEPARATOR_WIDTH);
        Texto(col, lin++, separator);
        
        // 3. Exibir os projetos
        foreach (var p in projetos)
        {
            // Truncagem e Formatação
            string nomeFormatado = Trunc(p.nome, W_NOME);
            
            string prazoStr = Trunc(p.prazoInicial?.ToString("dd/MM/yyyy") ?? "", W_PRAZO); 
            
            string progressoStr = $"{p.progresso}%";

            string line = 
                $"{p.id,-W_ID}" +
                $"{nomeFormatado,-W_NOME}" +
                $"{p.status,-W_STATUS}" +
                $"{p.aprovacao,-W_APROV}" +
                $"{p.score,-W_SCORE}" +
                $"{progressoStr,-W_PROG}" +
                $"{prazoStr,-W_PRAZO}";
            
            Texto(col, lin++, line);
        }

        // Adiciona um espaçamento de uma linha entre a lista e a label de input
        lin++; 

        // 4. Rodapé e Input
        if (projetos.Count > 0)
        {
            // Pede o ID na linha atual (lin)
            Console.SetCursorPosition(col, lin);
            
            // LInt é chamado. O prompt está dentro do LInt (que usa Console.Write).
            int id = LInt("Digite o ID do projeto que deseja visualizar (ou pressione ESC para voltar)", 0);
            
            if (id == 0) return;

            var proj = projetos.Find(x => x.id == id);
            if (proj == null)
            {
                tela.MostrarMensagem("ID não encontrado. Pressione uma tecla para continuar...");
                Console.ReadKey();
                return;
            }

            VisualizarAtualizar(proj);
        }
        else
        {
            tela.MostrarMensagem("Nenhum projeto cadastrado. Pressione uma tecla para voltar...");
            Console.ReadKey();
        }
    }

    private void VisualizarAtualizar(Projeto p)
    {
        while (true)
        {
            Console.Clear();
            // Largura do quadro ajustada para 83 (76 + 7)
            DesenhaQuadro(2, 1, 83, 28, "Visualizar Projeto");
            
            int col = 4;
            int lin = 3;

            Texto(col, lin++, $"Nome: {p.nome}");
            Texto(col, lin++, $"ID: {p.id}");
            Texto(col, lin++, $"Gerente de Projeto: {p.gerente}");
            Texto(col, lin++, $"Sponsor: {p.sponsor}");
            Texto(col, lin++, $"Orçamento Aprovado: R$ {p.orcamentoAprovado:n2}");
            Texto(col, lin++, $"Custo Real: R$ {(p.custoReal == 0 ? "" : p.custoReal.ToString("n2"))}");
            Texto(col, lin++, $"Desvio de Custo: {p.DesvioCustoTexto()}");
            Texto(col, lin++, $"Prazo Inicial: {(p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "")}");
            Texto(col, lin++, $"Novo Prazo: {(p.novoPrazo.HasValue ? p.novoPrazo.Value.ToString("dd/MM/yyyy") : "")}");
            Texto(col, lin++, $"Desvio de Prazo: {p.DesvioPrazoTexto()}");
            Texto(col, lin++, $"ROI(%): {p.roi}");
            Texto(col, lin++, $"Risco: {p.risco}");
            Texto(col, lin++, $"Alinhamento Estratégico: {p.alinhamento}");
            Texto(col, lin++, $"Urgência: {p.urgencia}");
            lin++;
            Texto(col, lin++, $"Score: {p.score}");
            Texto(col, lin++, $"Aprovação: {p.aprovacao}");
            Texto(col, lin++, $"Status: {p.status}");
            Texto(col, lin++, $"Progresso: {p.progresso}%");
            lin++;

            if (!Confirma("Atualizar Projeto?(S/N)").Equals("S", StringComparison.OrdinalIgnoreCase))
                break;

            p.nome = Perg("Nome");
            p.gerente = Perg("Gerente de Projeto");
            p.sponsor = Perg("Sponsor");
            p.orcamentoAprovado = LDec("Orçamento Aprovado (R$)");
            p.custoReal = LDec("Custo Real (R$)");
            p.prazoInicial = LDate("Prazo Inicial (dd/mm/aaaa)");
            p.novoPrazo = LDate("Novo Prazo (dd/mm/aaaa)");

            p.roi         = LerValorDeZeroACem("ROI");
            p.risco       = LerValorDeZeroACem("Risco");
            p.alinhamento = LerValorDeZeroACem("Alinhamento Estratégico");
            p.urgencia    = LerValorDeZeroACem("Urgência");

            p.RecalcularScoreEAprovacao();
            p.status = "Em Execução";
            p.progresso = Limita(LInt("Progresso (%)", 0, 100), 0, 100);

            tela.MostrarMensagem("Alterações salvas.");
            Console.ReadKey();
        }
    }

    private void DashboardProjetos()
    {
        // CORREÇÃO: Usar tela.PrepararTela para a moldura principal (consistente com o menu principal).
        tela.PrepararTela("Project Portfolio Management - Dashboard de Projetos");

        int lin = 3;
        int col = 4;
        
        Texto(col, lin++, "Projetos em Análise: " + projetos.Count(p => p.status == "Em Análise"));
        Texto(col, lin++, "Projetos Aprovados e em Execução: " + projetos.Count(p => p.status == "Em Execução"));
        Texto(col, lin++, "Projetos Encerrados: " + projetos.Count(p => p.status == "Encerrado"));
        lin++;
        
        int totalProjetos = projetos.Count;
        if (totalProjetos > 0)
        {
            double mediaScore = projetos.Average(p => p.score);
            Texto(col, lin++, "Média de Score dos Projetos: " + Math.Round(mediaScore, 1));

            decimal orcamentoTotal = projetos.Sum(p => p.orcamentoAprovado);
            Texto(col, lin++, "Orçamento Total Aprovado: $" + orcamentoTotal.ToString("N2"));

            decimal custoRealTotal = projetos.Sum(p => p.custoReal);
            Texto(col, lin++, "Custo Real Total: $" + custoRealTotal.ToString("N2"));

            int projetosAtrasados = projetos.Count(p => p.novoPrazo.HasValue && p.prazoInicial.HasValue && p.novoPrazo.Value > p.prazoInicial.Value);
            Texto(col, lin++, "Projetos com Atraso (Novo Prazo > Prazo Inicial): " + projetosAtrasados);

            int projetosDesvioPositivo = projetos.Count(p => p.orcamentoAprovado > 0 && p.custoReal < p.orcamentoAprovado);
            Texto(col, lin++, "Projetos com Desvio Positivo de Custo: " + projetosDesvioPositivo);
        }
        else
        {
            Texto(col, lin++, "Não há projetos cadastrados para gerar o Dashboard.");
        }

        // A mensagem "Pressione ESC para voltar:" agora será exibida DENTRO da moldura
        // devido à correção em MostrarRodapePadrao().
        tela.MostrarRodapePadrao();
        Console.ReadKey(true); // Espera por uma tecla para sair da tela.
    }

private void FechamentoFormal()
{
    Console.Clear();

    // Garantir altura mínima para evitar erros de buffer
    int minHeight = 42;
    if (Console.BufferHeight < minHeight) Console.BufferHeight = minHeight;
    int targetWindow = Math.Min(minHeight, Console.LargestWindowHeight);
    if (Console.WindowHeight < targetWindow) Console.WindowHeight = targetWindow;

    // Quadro principal
    int x = 2, y = 1, w = 83, h = 28;
    DesenhaQuadro(x, y, w, h, "Fechamento Formal de Projeto");

    int colRot = x + 2;
    int lin = y + 2;

    // ===== Entrada do ID (na mesma linha do texto) =====
    string label = "ID do Projeto para Fechamento:";
    Texto(colRot, lin, label);
    Console.SetCursorPosition(colRot + label.Length + 1, lin);
    string idStr = Console.ReadLine()?.Trim() ?? "";

    if (!int.TryParse(idStr, out int id))
    {
        Texto(colRot, lin + 1, "ID inválido.");
        Console.ReadKey();
        return;
    }

    Projeto? p = projetos.Find(proj => proj.id == id);
    if (p == null)
    {
        Texto(colRot, lin + 1, "Projeto não encontrado.");
        Console.ReadKey();
        return;
    }

    // Pular apenas uma linha antes de exibir os dados
    lin += 2;

    // ===== Exibir dados do projeto =====
    Texto(colRot, lin++, $"Nome: {p.nome}");
    Texto(colRot, lin++, $"Status Atual: {p.status}");
    Texto(colRot, lin++, $"Prazo Inicial: {(p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "")}");
    Texto(colRot, lin++, $"Orçamento Aprovado: R$ {p.orcamentoAprovado:N2}");
    lin++;

    // ===== Custo Real =====
    {
        string lbl = "Custo Real ($):";
        Texto(colRot, lin, lbl);
        int xIn = colRot + lbl.Length + 1;

        decimal custoReal;
        while (true)
        {
            Console.SetCursorPosition(xIn, lin);
            string s = Console.ReadLine() ?? "";

            if (decimal.TryParse(s, out custoReal))
                break;

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(xIn, lin + 1, "Valor inválido.");
            Console.ResetColor();
            Texto(xIn, lin, new string(' ', 30));
            Texto(xIn, lin + 1, new string(' ', 30));
        }
        lin += 2;
        p.custoReal = custoReal;
    }

    // ===== Novo Prazo =====
    {
        string lbl = "Novo Prazo (dd/mm/aaaa):";
        Texto(colRot, lin, lbl);
        int xIn = colRot + lbl.Length + 1;

        DateTime? novoPrazo = null;
        while (true)
        {
            Console.SetCursorPosition(xIn, lin);
            string s = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(s))
                break;

            if (DateTime.TryParse(s, out DateTime np))
            {
                novoPrazo = np;
                break;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Texto(xIn, lin + 1, "Data inválida. Use dd/mm/aaaa ou deixe em branco.");
            Console.ResetColor();
            Texto(xIn, lin, new string(' ', 30));
            Texto(xIn, lin + 1, new string(' ', 55));
        }
        lin += 2;
        p.novoPrazo = novoPrazo;
    }

    // ===== Progresso =====
    {
        string lbl = "Progresso (%):";
        Texto(colRot, lin, lbl);
        int xIn = colRot + lbl.Length + 1;

        int progresso = Ler0a100NaPos(xIn, lin);
        lin += 2;
        p.progresso = progresso;
    }

    // ===== Finalizar =====
    string lblFin = "Finalizar Projeto? (S/N):";
    Texto(colRot, lin, lblFin);
    Console.SetCursorPosition(colRot + lblFin.Length + 1, lin);
    string finalizar = (Console.ReadLine() ?? "N").Trim().ToUpperInvariant();

    // ===== Mensagem final =====
    int linMsg = y + h - 2;
    if (finalizar == "S")
    {
        p.status = "Encerrado";
        Texto(colRot, linMsg, "Projeto encerrado com sucesso. Pressione ESC para voltar ao MENU...");
    }
    else
    {
        Texto(colRot, linMsg, "Fechamento não confirmado. Pressione ESC para voltar ao MENU...");
    }

    Console.ReadKey();
}

    private int LerValorDeZeroACem(string label)
    {
        int valor;
        bool valido = false;

        do
        {
            Console.Write($"Informe {label} (0 a 100): ");
            string entrada = Console.ReadLine();

            if (int.TryParse(entrada, out valor))
            {
                if (valor >= 0 && valor <= 100)
                {
                    valido = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Valor inválido! Digite um número entre 0 e 100.\n");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida! Digite apenas números.\n");
                Console.ResetColor();
            }

        } while (!valido);

        return valor;
    }

    // Métodos Auxiliares
    private string Trunc(string s, int max) => string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s.Substring(0, max));

    private void Moldura(string titulo)
    {
        // Largura da barra aumentada de 74 para 81 (74 + 7)
        string barra = new string('═', 81);
        Console.WriteLine("╔" + barra + "╗");
        Console.WriteLine("║" + Centraliza(titulo, 81) + "║");
        Console.WriteLine("║" + new string(' ', 81) + "║");
    }

    private string Centraliza(string texto, int largura)
    {
        if (texto.Length > largura) texto = texto.Substring(0, largura);
        int pad = (largura - texto.Length) / 2;
        return new string(' ', pad) + texto + new string(' ', largura - pad - texto.Length);
    }

    private void MsgLinha(string msg) => Console.WriteLine(msg);

    private string Perg(string rotulo)
    {
        Console.Write(rotulo + ": ");
        return Console.ReadLine() ?? "";
    }

    private int LInt(string rotulo, int min = int.MinValue, int max = int.MaxValue)
    {
        int v;
        while (true)
        {
            Console.Write(rotulo + ": ");
            if (int.TryParse(Console.ReadLine(), out v) && v >= min && v <= max) return v;
            Console.WriteLine("Valor inválido.");
        }
    }

    private decimal LDec(string rotulo)
    {
        decimal v;
        while (true)
        {
            Console.Write(rotulo + ": ");
            if (decimal.TryParse(Console.ReadLine(), out v)) return v;
            Console.WriteLine("Valor inválido.");
        }
    }

    private DateTime LDate(string rotulo)
    {
        DateTime v;
        while (true)
        {
            Console.Write(rotulo + ": ");
            if (DateTime.TryParse(Console.ReadLine(), out v)) return v;
            Console.WriteLine("Data inválida.");
        }
    }

    private int Limita(int v, int min, int max) => Math.Min(Math.Max(v, min), max);

    private string Confirma(string rotulo)
    {
        Console.Write(rotulo + " ");
        var s = Console.ReadLine() ?? "N";
        return string.IsNullOrEmpty(s) ? "N" : s.Trim();
    }

    private bool ExisteId(int id) => projetos.Exists(p => p.id == id);
    private bool ExisteNome(string nome) => projetos.Exists(p => p.nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

    void DesenhaQuadro(int x, int y, int w, int h, string titulo)
    {
        string horiz = new string('═', w - 2);

        Console.SetCursorPosition(x, y);         Console.Write('╔');
        Console.Write(horiz);                    Console.Write('╗');

        for (int i = 1; i < h - 1; i++)
        {
            Console.SetCursorPosition(x,     y + i); Console.Write('║');
            Console.SetCursorPosition(x+w-1, y + i); Console.Write('║');
        }

        Console.SetCursorPosition(x, y + h - 1); Console.Write('╚');
        Console.Write(horiz);                    Console.Write('╝');

        int cx = x + (w - 2 - titulo.Length)/2 + 1;
        Console.SetCursorPosition(cx, y + 1);
        Console.Write(titulo);
    }

    void Texto(int col, int lin, string s)
    {
        Console.SetCursorPosition(col, lin);
        Console.Write(s);
    }

    string LerNaPos(int col, int lin)
    {
        Console.SetCursorPosition(col, lin);
        return Console.ReadLine() ?? "";
    }

    int Ler0a100NaPos(int col, int lin, int larguraMsg = 50)
    {
        while (true)
        {
            Console.SetCursorPosition(col, lin);
            string entrada = Console.ReadLine() ?? "";

            if (int.TryParse(entrada, out int v) && v >= 0 && v <= 100)
            {
                Console.SetCursorPosition(col, lin + 1);
                Console.Write(new string(' ', larguraMsg));
                return v;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(col, lin + 1);
            Console.Write("Valor inválido! Digite um número entre 0 e 100.");
            Console.ResetColor();
        }
    }
}