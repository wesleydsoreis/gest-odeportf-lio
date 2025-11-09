using System;
using System.Collections.Generic;

public class ProjetoCRUD
{
    private readonly Tela tela;
    private readonly List<Projeto> projetos = new List<Projeto>();

    public ProjetoCRUD(Tela tela)
    {
        this.tela = tela;
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
        while (true)
        {
            Console.Clear();

            int x = 2, y = 1, w = 76, h = 28;
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

                Texto(colRot, linSalvar + 2, "Cadastrar outro?(S/N): ");
                string outro = Console.ReadLine() ?? "N";
                if (!outro.Equals("S", StringComparison.OrdinalIgnoreCase))
                    break;
            }
        }
    }

    private void ListarProjetos()
    {
        Console.Clear();
        Moldura("Exibir Projetos");

        Console.WriteLine("ID   Nome                  Status         Aprovação    Score  Progresso  Prazo");
        Console.WriteLine("-------------------------------------------------------------------------------");
        foreach (var p in projetos)
        {
            string prazo = p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "";
            Console.WriteLine($"{p.id,-4} {Trunc(p.nome,18),-18} {Trunc(p.status,12),-12} {Trunc(p.aprovacao,10),-10} {p.score,5} {p.progresso,9}%  {prazo,10}");
        }

        Console.WriteLine();
        int id = LInt("Digite o ID do projeto que deseja visualizar (ou ESC para voltar)");
        if (id == 0) return;

        var proj = projetos.Find(x => x.id == id);
        if (proj == null)
        {
            MsgLinha("ID não encontrado.");
            Console.ReadKey();
            return;
        }

        VisualizarAtualizar(proj);
    }

    private void VisualizarAtualizar(Projeto p)
    {
        while (true)
        {
            Console.Clear();
            Moldura("Visualizar Projeto");

            Console.WriteLine($"Nome: {p.nome}");
            Console.WriteLine($"ID: {p.id}");
            Console.WriteLine($"Gerente de Projeto: {p.gerente}");
            Console.WriteLine($"Sponsor: {p.sponsor}");
            Console.WriteLine($"Orçamento Aprovado: R$ {p.orcamentoAprovado:n2}");
            Console.WriteLine($"Custo Real: R$ {(p.custoReal == 0 ? "" : p.custoReal.ToString("n2"))}");
            Console.WriteLine($"Desvio de Custo: {p.DesvioCustoTexto()}");
            Console.WriteLine($"Prazo Inicial: {(p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "")}");
            Console.WriteLine($"Novo Prazo: {(p.novoPrazo.HasValue ? p.novoPrazo.Value.ToString("dd/MM/yyyy") : "")}");
            Console.WriteLine($"Desvio de Prazo: {p.DesvioPrazoTexto()}");
            Console.WriteLine($"ROI(%): {p.roi}");
            Console.WriteLine($"Risco: {p.risco}");
            Console.WriteLine($"Alinhamento Estratégico: {p.alinhamento}");
            Console.WriteLine($"Urgência: {p.urgencia}");
            Console.WriteLine();
            Console.WriteLine($"Score: {p.score}");
            Console.WriteLine($"Aprovação: {p.aprovacao}");
            Console.WriteLine($"Status: {p.status}");
            Console.WriteLine($"Progresso: {p.progresso}%");
            Console.WriteLine();

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

            MsgLinha("Alterações salvas.");
            Console.ReadKey();
        }
    }

    private void DashboardProjetos()
    {
        Console.Clear();
        Moldura("Dashboard de Projetos");
        Console.WriteLine("ID   Nome               Status        CustoReal   DesvioC  Prazo      DesvioP  Prog");
        Console.WriteLine("-------------------------------------------------------------------------------------");
        foreach (var p in projetos)
        {
            string prazo = p.novoPrazo.HasValue ? p.novoPrazo.Value.ToString("dd/MM/yyyy")
                                                : (p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "");
            Console.WriteLine(
                $"{p.id,-4} {Trunc(p.nome,18),-18} {Trunc(p.status,12),-12} " +
                $"{(p.custoReal==0? "": ("R$ "+p.custoReal.ToString("n0"))),-11} " +
                $"{Trunc(p.DesvioCustoTexto(),6),-6} {prazo,10} {Trunc(p.DesvioPrazoTexto(),6),-6} {p.progresso,4}%");
        }
        Console.WriteLine();
        Console.Write("Voltar (V): "); Console.ReadLine();
    }

    private void FechamentoFormal()
    {
        if (projetos.Count == 0)
        {
            MsgLinha("Nenhum projeto cadastrado.");
            Console.ReadKey();
            return;
        }

        int id = LInt("Informe o ID do projeto para fechamento");
        var p = projetos.Find(x => x.id == id);
        if (p == null)
        {
            MsgLinha("ID não encontrado.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Moldura("Fechamento Formal de Projeto");

        Console.WriteLine($"Nome: {p.nome}");
        Console.WriteLine($"ID: {p.id}");
        Console.WriteLine($"Gerente de Projeto: {p.gerente}");
        Console.WriteLine($"Sponsor: {p.sponsor}");
        Console.WriteLine($"Orçamento Aprovado: R$ {p.orcamentoAprovado:n2}");
        Console.WriteLine($"Custo Real: R$ {p.custoReal:n2}");
        Console.WriteLine($"Desvio de Custo: {p.DesvioCustoTexto()}");
        Console.WriteLine($"Prazo Inicial: {(p.prazoInicial.HasValue ? p.prazoInicial.Value.ToString("dd/MM/yyyy") : "")}");
        Console.WriteLine($"Data de Término: {(p.novoPrazo.HasValue ? p.novoPrazo.Value.ToString("dd/MM/yyyy") : "")}");
        Console.WriteLine($"Desvio de Prazo: {p.DesvioPrazoTexto()}");
        Console.WriteLine($"ROI(%): {p.roi}");
        Console.WriteLine($"Risco: {p.risco}");
        Console.WriteLine($"Alinhamento Estratégico: {p.alinhamento}");
        Console.WriteLine($"Urgência: {p.urgencia}");
        Console.WriteLine();
        Console.WriteLine($"Score: {p.score}");
        Console.WriteLine($"Aprovação: {p.aprovacao}");
        Console.WriteLine($"Status: {p.status}");
        Console.WriteLine($"Progresso: {p.progresso}%");
        Console.WriteLine();

        string accite = Confirma("Entrega do Produto (Accite) (S/N)");
        string doc = Confirma("Documentação final anexada? (S/N)");
        string enviar = Confirma("Enviar para aprovação do PMO? (S/N)");
        string fechar = Confirma("Confirmar fechamento do projeto? (S/N)");

        if (accite.Equals("S", StringComparison.OrdinalIgnoreCase) &&
            doc.Equals("S", StringComparison.OrdinalIgnoreCase) &&
            enviar.Equals("S", StringComparison.OrdinalIgnoreCase) &&
            fechar.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            p.status = "Encerrado";
            p.progresso = 100;
            MsgLinha("Projeto encerrado.");
        }
        else
        {
            MsgLinha("Fechamento não concluído.");
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

    private string Trunc(string s, int max) => string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s.Substring(0, max));

    private void Moldura(string titulo)
    {
        string barra = new string('═', 74);
        Console.WriteLine("╔" + barra + "╗");
        Console.WriteLine("║" + Centraliza(titulo, 74) + "║");
        Console.WriteLine("║" + new string(' ', 74) + "║");
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
