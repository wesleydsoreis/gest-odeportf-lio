using System;
using System.Collections.Generic;

public class RecursoCRUD
{
    private readonly Tela tela;
    private readonly List<Recurso> recursos = new List<Recurso>();

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
        r.nome = LerNaPos(colRot + labelNome.Length, lin++); // Input logo após o rótulo

        // 2. Área/Departamento:
        string labelArea = "Área/Departamento: ";
        Texto(colRot, lin, labelArea);
        r.areaDepartamento = LerNaPos(colRot + labelArea.Length, lin++); // Input logo após o rótulo

        // 3. Cargo/Função:
        string labelFuncao = "Cargo/Função: ";
        Texto(colRot, lin, labelFuncao);
        r.funcao = LerNaPos(colRot + labelFuncao.Length, lin++); // Input logo após o rótulo
        
        r.alocacaoPercent = 0; // Inicializa a alocação

        // --- Salvar ---
        lin++;
        int linSalvar = lin; 
        string labelSalvar = "Salvar cadastro?(S/N): ";
        Texto(colRot, linSalvar, labelSalvar);

        // Posiciona o cursor logo após o rótulo de salvar
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
            Console.WriteLine("Nenhum recurso cadastrado.");
            Console.ReadKey();
            return;
        }
        Console.Clear();
        Moldura("Alocar Recurso (simples)");
        Console.WriteLine("Nome                         Função                 Alocação");
        Console.WriteLine("-------------------------------------------------------------");
        for (int i = 0; i < recursos.Count; i++)
        {
            var r = recursos[i];
            Console.WriteLine($"{i+1,-3} {Trunc(r.nome,25),-25} {Trunc(r.funcao,20),-20} {r.alocacaoPercent,3}%");
        }
        Console.WriteLine();
        Console.Write("Escolha o nº do recurso: ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > recursos.Count) return;

        var sel = recursos[idx - 1];
        Console.Write("Nova alocação (%): ");
        if (int.TryParse(Console.ReadLine(), out int nova))
        {
            if (nova < 0) nova = 0;
            if (nova > 100) nova = 100;
            if (sel.alocacaoPercent + nova > 100)
            {
                Console.WriteLine("Capacidade de alocação atingida.");
            }
            else
            {
                sel.alocacaoPercent = nova;
                Console.WriteLine("Alocação salva.");
            }
        }
        Console.ReadKey();
    }

    private void Balanco()
    {
        Console.Clear();
        Moldura("Balanço de Utilização de Recursos");
        Console.WriteLine("Nome do Recurso           Função                    Alocação");
        Console.WriteLine("-------------------------------------------------------------");
        foreach (var r in recursos)
        {
            Console.WriteLine($"{Trunc(r.nome,25),-25} {Trunc(r.funcao,22),-22} {r.alocacaoPercent,3}%");
        }
        Console.WriteLine();
        Console.WriteLine($"Total de Recursos: {recursos.Count}");
        Console.WriteLine();
        Console.Write("Voltar (V): "); Console.ReadLine();
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
    
    // Métodos copiados de ProjetoCRUD para Desenho e Input
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