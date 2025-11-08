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
        Moldura("Cadastrar Recurso");
        var r = new Recurso();
        Console.Write("Nome: "); r.nome = Console.ReadLine();
        Console.Write("Área/Departamento: "); r.areaDepartamento = Console.ReadLine();
        Console.Write("Cargo/Função: "); r.funcao = Console.ReadLine();
        Console.Write("Salvar cadastro?(S/N): ");
        if ((Console.ReadLine() ?? "N").Trim().ToUpper() == "S")
        {
            recursos.Add(r);
            Console.WriteLine("Recurso salvo.");
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

    // Helpers
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
}
