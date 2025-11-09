using System;
using System.Collections.Generic;
using System.Reflection;

// =================== INSTÂNCIAS ===================
Tela tela = new Tela();
ProjetoCRUD projetoCRUD = new ProjetoCRUD(tela);
RecursoCRUD recursoCRUD  = new RecursoCRUD(tela);

// =================== LOGIN ===================
MostrarLogin(); // só volta daqui se acertar

// =================== MENU PRINCIPAL ===================
string opcao;
List<string> opcoes = new List<string>
{
    "     Menu      ",
    "1 - Cadastrar Projeto",
    "2 - Exibir Projetos",
    "3 - Dashboard de Projetos",
    "4 - Fechamento Formal de Projeto",
    "5 - Cadastrar Recurso",
    "6 - Alocar Recurso",
    "7 - Balanço de Utilização de Recursos",
    "8 - Balanço Consolidado do Portfólio",
    "0 - Sair"
};

while (true)
{
    tela.PrepararTela("Project Portfolio Management");
    opcao = tela.MostrarMenu(opcoes, 2, 2);

    if (opcao == "0") break;
    else if (opcao == "1") projetoCRUD.GetType().GetMethod("CadastrarProjeto",   BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "2") projetoCRUD.GetType().GetMethod("ListarProjetos",     BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "3") projetoCRUD.GetType().GetMethod("DashboardProjetos",  BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "4") projetoCRUD.GetType().GetMethod("FechamentoFormal",   BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "5") recursoCRUD.GetType().GetMethod("Cadastrar",          BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD,  null);
    else if (opcao == "6") recursoCRUD.GetType().GetMethod("Alocar",             BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD,  null);
    else if (opcao == "7") recursoCRUD.GetType().GetMethod("Balanco",            BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD,  null);
    else if (opcao == "8") BalancoConsolidadoPlaceholder();
    else
    {
        tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
    }
}

// =================== FUNÇÕES AUXILIARES ===================
static void MostrarLogin()
{
    const string USER = "admin";
    const string PASS = "123";

    int largura = 66;
    string barra = new string('═', largura);

    while (true)
    {
        Console.Clear();

        // ------- QUADRO DE LOGIN (um quadrado só) -------
        Console.WriteLine("╔" + barra + "╗");                                // 0
        Console.WriteLine("║" + Centraliza("Project Portfolio Management", largura) + "║"); // 1
        Console.WriteLine("║" + new string(' ', largura) + "║");             // 2

        string msg = " Informe seu usuário e senha:";
        Console.WriteLine("║" + msg + new string(' ', Math.Max(0, largura - msg.Length)) + "║"); // 3

        Console.WriteLine("║" + new string(' ', largura) + "║");             // 4

        string l1 = " Usuário: ";
        Console.WriteLine("║" + l1 + new string(' ', Math.Max(0, largura - l1.Length)) + "║");   // 5

        string l2 = " Senha:   ";
        Console.WriteLine("║" + l2 + new string(' ', Math.Max(0, largura - l2.Length)) + "║");   // 6

        string l3 = " Confirmar Dados? (S/N): ";
        Console.WriteLine("║" + l3 + new string(' ', Math.Max(0, largura - l3.Length)) + "║");   // 7

        Console.WriteLine("╚" + barra + "╝");                               // 8

        // entradas (coluna, linha)
        Console.SetCursorPosition(1 + l1.Length, 5); // após "Usuário: "
        string usuario = Console.ReadLine();

        Console.SetCursorPosition(1 + l2.Length, 6); // após "Senha: "
        string senha = Console.ReadLine();

        Console.SetCursorPosition(1 + l3.Length, 7); // após "Confirmar..."
        string conf = Console.ReadLine() ?? "N";

        bool ok =
            string.Equals(usuario?.Trim(), USER, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(senha?.Trim(),   PASS, StringComparison.Ordinal) &&
            string.Equals(conf?.Trim().ToUpper(), "S", StringComparison.Ordinal);

        if (ok) return;

        // ------- TELA DE USUÁRIO INVÁLIDO (um quadrado só) -------
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔" + barra + "╗"); // 0
            Console.WriteLine("║" + Centraliza("Project Portfolio Management", largura) + "║"); // 1
            Console.WriteLine("║" + new string(' ', largura) + "║"); // 2

            string e1 = " Usuário ou senha incorretos. Informe Novamente:";
            Console.WriteLine("║" + e1 + new string(' ', Math.Max(0, largura - e1.Length)) + "║"); // 3

            Console.WriteLine("║" + new string(' ', largura) + "║"); // 4

            // mesmas linhas dos campos:
            Console.WriteLine("║" + l1 + new string(' ', Math.Max(0, largura - l1.Length)) + "║"); // 5
            Console.WriteLine("║" + l2 + new string(' ', Math.Max(0, largura - l2.Length)) + "║"); // 6
            Console.WriteLine("║" + l3 + new string(' ', Math.Max(0, largura - l3.Length)) + "║"); // 7
            Console.WriteLine("╚" + barra + "╝"); // 8

            Console.SetCursorPosition(1 + l1.Length, 5);
            string u2 = Console.ReadLine();

            Console.SetCursorPosition(1 + l2.Length, 6);
            string p2 = Console.ReadLine();

            Console.SetCursorPosition(1 + l3.Length, 7);
            string c2 = Console.ReadLine() ?? "N";

            bool ok2 =
                string.Equals(u2?.Trim(), USER, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p2?.Trim(), PASS, StringComparison.Ordinal) &&
                string.Equals(c2?.Trim().ToUpper(), "S", StringComparison.Ordinal);

            if (ok2) return;

            // se ainda errar, repete o mesmo box de erro
        }
    }
}

static string Centraliza(string texto, int largura)
{
    if (texto.Length > largura) texto = texto.Substring(0, largura);
    int pad = (largura - texto.Length) / 2;
    return new string(' ', pad) + texto + new string(' ', largura - pad - texto.Length);
}

static void BalancoConsolidadoPlaceholder()
{
    Console.Clear();
    int largura = 66;
    string barra = new string('═', largura);

    Console.WriteLine("╔" + barra + "╗");
    Console.WriteLine("║" + Centraliza("Balanço Consolidado do Portfólio", largura) + "║");
    Console.WriteLine("║" + new string(' ', largura) + "║");

    EscreveLinhaBox(largura, " Total de Projetos: ");
    EscreveLinhaBox(largura, " Projetos em Análise: ");
    EscreveLinhaBox(largura, " Projetos em Execução: ");
    EscreveLinhaBox(largura, " Projetos Encerrados: ");

    Console.WriteLine("║" + new string(' ', largura) + "║");
    EscreveLinhaBox(largura, " Voltar (V): ");
    Console.WriteLine("╚" + barra + "╝");
    Console.ReadLine();
}

static void EscreveLinhaBox(int largura, string texto)
{
    Console.WriteLine("║" + texto + new string(' ', Math.Max(0, largura - texto.Length)) + "║");
}
