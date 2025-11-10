using System;
using System.Collections.Generic;
using System.Linq; 

public class RecursoCRUD
{
    private readonly Tela tela;
    private readonly List<Recurso> recursos = new List<Recurso>();
    
    // ====================================================================
    // SIMULAÇÃO: Lista de Projetos para lookup do Gerente (DEVE ESTAR DENTRO DA CLASSE)
    private readonly List<Projeto> mockProjetos = new List<Projeto>
    {
        new Projeto { id = 1001, nome = "Expansao Digital", gerente = "Alice", orcamentoAprovado = 150000m, status = "Em Execução" },
        new Projeto { id = 1002, nome = "Migracao Cloud", gerente = "Carlos", orcamentoAprovado = 50000m, status = "Em Análise" },
        new Projeto { id = 1003, nome = "Sistema CRM", gerente = "Eduardo", orcamentoAprovado = 80000m, status = "Em Execução" }
    };
    // ====================================================================

    public RecursoCRUD(Tela tela) 
    {
        this.tela = tela;
    }

    public void Executar()
    {
        while (true)
        {
            tela.PrepararTela("Project Portfolio Management - Recursos");
            var ops = new List<string>
            {
                "     Menu Recursos             ",
                "1 - Cadastrar Recurso         ",
                "2 - Alocar Recurso (simples)  ",
                "3 - Balanço de Utilização     ",
                "0 - Voltar                    "
            };
            string op = tela.MostrarMenu(ops, 2, 2);

            if (op == "0") break;
            else if (op == "1") Cadastrar();
            else if (op == "2") Alocar();
            else if (op == "3") Balanco();
            else
            {
                tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private void Cadastrar()
    {
        Console.Clear();
        
        // --- Configuração da Tela ---
        int x = 2, y = 1, w = 83, h = 18;
        DesenhaQuadro(x, y, w, h, "Cadastrar Recurso");

        int colRot = x + 3; // Coluna inicial dos rótulos
        int lin = y + 3;
        
        var r = new Recurso();
        
        // --- Captura de Dados (Posicionamento Dinâmico) ---
        
        // 1. Nome:
        string labelNome = "Nome: ";
        Texto(colRot, lin, labelNome);
        r.nome = LerNaPos(colRot + labelNome.Length, lin++);

        // 2. Área/Departamento:
        string labelArea = "Área/Departamento: ";
        Texto(colRot, lin, labelArea);
        r.areaDepartamento = LerNaPos(colRot + labelArea.Length, lin++);

        // 3. Cargo/Função:
        string labelFuncao = "Cargo/Função: ";
        Texto(colRot, lin, labelFuncao);
        r.funcao = LerNaPos(colRot + labelFuncao.Length, lin++);
        
        r.alocacaoPercent = 0;

        // --- Salvar ---
        lin++;
        int linSalvar = lin; 
        string labelSalvar = "Salvar cadastro?(S/N): ";
        Texto(colRot, linSalvar, labelSalvar);

        Console.SetCursorPosition(colRot + labelSalvar.Length, linSalvar);
        
        string salvar = Console.ReadLine() ?? "N";
        if (salvar.Equals("S", StringComparison.OrdinalIgnoreCase))
        {
            recursos.Add(r);
            Texto(colRot, linSalvar + 1, "Recurso salvo.");
        }
        else
        {
            Texto(colRot, linSalvar + 1, "Cadastro cancelado.");
        }

        Console.ReadKey();
    }


    private void Alocar()
    {
        if (recursos.Count == 0)
        {
            tela.MostrarMensagem("Nenhum recurso cadastrado.");
            Console.ReadKey();
            return;
        }
        
        tela.PrepararTela("Project Portfolio Management - Alocar Recurso");

        // --- Constantes de Alinhamento (Larguras Totais) ---
        const int W_NUM = 4;
        const int W_NOME = 17; 
        const int W_AREA = 17; 
        const int W_FUNCAO = 17; 
        const int W_ALOCACAO = 9;
        
        int col = 2; 
        int lin = 3; 
        
        // 1. Cabeçalho formatado
        string formatHeader = 
            $"{"Nº",-W_NUM}" +
            $"{"NOME",-W_NOME}" +
            $"{"ÁREA",-W_AREA}" + 
            $"{"FUNÇÃO",-W_FUNCAO}" + 
            $"{"ALOCAÇÃO",-W_ALOCACAO}";
        
        Texto(col, lin++, formatHeader);
        
        // 2. Linha separadora
        const int SEPARATOR_WIDTH = 83;
        string separator = new string('═', SEPARATOR_WIDTH);
        Texto(col, lin++, separator);
        
        // 3. Listar Recursos
        for (int i = 0; i < recursos.Count; i++)
        {
            var r = recursos[i];
            string nomeFormatado = Trunc(r.nome, W_NOME);
            string areaFormatada = Trunc(r.areaDepartamento, W_AREA);
            string funcaoFormatada = Trunc(r.funcao, W_FUNCAO);
            string alocacaoStr = $"{r.alocacaoPercent}%";

            string line = 
                $"{i + 1,-W_NUM}" +
                $"{nomeFormatado,-W_NOME}" +
                $"{areaFormatada,-W_AREA}" +
                $"{funcaoFormatada,-W_FUNCAO}" +
                $"{alocacaoStr,-W_ALOCACAO}";
            
            Texto(col, lin++, line);
        }

        lin++; // Espaçamento
        
        // 4. Input e Lógica (Alinhamento Corrigido: colRot = col = 2)
        
        int colRot = col; // Coluna inicial para as labels, alinhada com 'Nº'
        bool salvamentoPendente = false;
        
        // Escolha o nº do recurso:
        string labelEscolha = "Escolha o nº do recurso: ";
        Texto(colRot, lin, labelEscolha);

        int idx;
        Console.SetCursorPosition(colRot + labelEscolha.Length, lin);
        if (!int.TryParse(Console.ReadLine(), out idx) || idx < 1 || idx > recursos.Count)
        {
            tela.MostrarMensagem("Seleção inválida. Pressione uma tecla para voltar...");
            Console.ReadKey();
            return;
        }
        var sel = recursos[idx - 1];
        lin++;

        // Projeto:
        string labelProjeto = "Projeto: ";
        Texto(colRot, lin, labelProjeto);
        string nomeProjeto = LerNaPos(colRot + labelProjeto.Length, lin++);
        
        // === Lógica de Preenchimento Automático do Gerente ===
        Projeto projetoEncontrado = mockProjetos.FirstOrDefault(p => p.nome.Equals(nomeProjeto, StringComparison.OrdinalIgnoreCase));
        
        string gerenteProjeto = "[NÃO ENCONTRADO]"; 
        if (projetoEncontrado != null)
        {
            gerenteProjeto = projetoEncontrado.gerente;
        }

        // Gerente de Projeto: (Exibição)
        string labelGerente = "Gerente de Projeto: ";
        Texto(colRot, lin, labelGerente);
        Texto(colRot + labelGerente.Length, lin, gerenteProjeto); 
        lin++; 
        
        lin++;
        
        // Nova alocação (%):
        string labelNovaAlocacao = "Nova alocação (%): ";
        Texto(colRot, lin, labelNovaAlocacao);

        int nova = 0;
        Console.SetCursorPosition(colRot + labelNovaAlocacao.Length, lin);
        if (int.TryParse(Console.ReadLine(), out nova))
        {
            if (nova < 0) nova = 0;
            if (nova > 100) nova = 100;

            if (sel.alocacaoPercent + nova > 100)
            {
                // Cenário de alocação indisponível
                lin++;
                Console.ForegroundColor = ConsoleColor.Red;
                Texto(colRot, lin++, "Capacidade máxima de alocação atingida.");
                Console.ResetColor();
                
                // Pergunta de confirmação de sobrecarga
                string labelConfirma = "Cadastrar nova alocação (Sobrecarga)?(S/N): ";
                Texto(colRot, lin, labelConfirma);
                
                Console.SetCursorPosition(colRot + labelConfirma.Length, lin);
                string confirma = Console.ReadLine() ?? "N";

                if (confirma.Equals("S", StringComparison.OrdinalIgnoreCase))
                {
                    sel.alocacaoPercent += nova; 
                    salvamentoPendente = true;
                    lin++;
                }
                else
                {
                    lin++;
                    Texto(colRot, lin++, "Alocação cancelada.");
                }

            }
            else
            {
                // Alocação normal
                sel.alocacaoPercent += nova;
                salvamentoPendente = true;
            }
        }

        // --- Lógica de SALVAMENTO FINAL (Padrão das outras telas) ---
        
        if (salvamentoPendente)
        {
            lin++; // Espaçamento
            
            // Label Salvar Alocação? (S/N)
            string labelSalvar = "Salvar Alocação?(S/N): ";
            int linSalvar = lin; 
            Texto(colRot, linSalvar, labelSalvar);

            Console.SetCursorPosition(colRot + labelSalvar.Length, linSalvar);
            string salvar = Console.ReadLine() ?? "N";

            if (salvar.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                Texto(colRot, linSalvar + 1, "Alocação salva com sucesso.");
            }
            else
            {
                // Reverte a alteração se o usuário cancelar
                sel.alocacaoPercent -= nova;
                Texto(colRot, linSalvar + 1, "Alocação cancelada.");
            }

            // Espera para visualização da mensagem
            Console.ReadKey();
        }
        
        // Retorna ao menu principal
    }

    private void Balanco()
    {
        tela.PrepararTela("Project Portfolio Management");
        
        // --- Constantes de Alinhamento ---
        const int W_NOME = 17;        
        const int W_AREA = 17;        
        const int W_FUNCAO = 17;      
        const int W_ALOCACAO = 9;         

        int col = 2; 
        int lin = 3; 

        // 1. Cabeçalho formatado
        string formatHeader = 
            $"{"NOME",-W_NOME}" +
            $"{"ÁREA",-W_AREA}" + 
            $"{"FUNÇÃO",-W_FUNCAO}" +
            $"{"ALOCAÇÃO",-W_ALOCACAO}";
        
        // Exibir o cabeçalho
        Texto(col, lin++, formatHeader);
        
        // 2. Linha separadora
        const int SEPARATOR_WIDTH = 83; 
        string separator = new string('═', SEPARATOR_WIDTH);
        Texto(col, lin++, separator);
        
        // 3. Exibir os recursos
        foreach (var r in recursos)
        {
            // Truncagem e Formatação
            string nomeFormatado = Trunc(r.nome, W_NOME);
            string areaFormatada = Trunc(r.areaDepartamento, W_AREA);
            string funcaoFormatada = Trunc(r.funcao, W_FUNCAO);
            string alocacaoStr = $"{r.alocacaoPercent}%";

            string line = 
                $"{nomeFormatado,-W_NOME}" +
                $"{areaFormatada,-W_AREA}" +
                $"{funcaoFormatada,-W_FUNCAO}" +
                $"{alocacaoStr,-W_ALOCACAO}";
            
            Texto(col, lin++, line);
        }

        // 4. Totais e Rodapé
        lin++; 
        Texto(col, lin++, $"Total de Recursos: {recursos.Count}");
        
        tela.MostrarRodapePadrao();
        Console.ReadKey(true); 
    }

    // --- Métodos Auxiliares ---

    private string Trunc(string s, int max) => string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s.Substring(0, max));
    
    private void Moldura(string titulo)
    {
        string barra = new string('═', 66);
        Console.WriteLine("╔" + barra + "╗");
        Console.WriteLine("║" + Centraliza(titulo, 66) + "║");
        Console.WriteLine("║" + new string(' ', 66) + "║");
    }
    
    private string Centraliza(string texto, int largura)
    {
        if (texto.Length > largura) texto = texto.Substring(0, largura);
        int pad = (largura - texto.Length) / 2;
        return new string(' ', pad) + texto + new string(' ', largura - pad - texto.Length);
    }
    
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
        // Limpa a linha antes de ler (por exemplo, 40 espaços para cobrir o campo)
        Console.Write(new string(' ', 40)); 
        Console.SetCursorPosition(col, lin);
        return Console.ReadLine() ?? "";
    }
}