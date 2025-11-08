using System;
using System.Collections.Generic;
using System.Reflection;

Tela tela = new Tela();
ProjetoCRUD projetoCRUD = new ProjetoCRUD(tela);
RecursoCRUD recursoCRUD = new RecursoCRUD(tela);


while (true)
{
    Console.Clear();
    string barra = new string('═', 66);
    Console.WriteLine("╔" + barra + "╗");
    Console.WriteLine("║" + Centraliza("Project Portfolio Management", 66) + "║");
    Console.WriteLine("║" + new string(' ', 66) + "║");
    Console.Write("║ Informe seu usuário e senha:".PadRight(67) + "║\n");
    Console.WriteLine("║" + new string(' ', 66) + "║");

    Console.Write("║ Usuário: ".PadRight(67));
    string usuario = Console.ReadLine();
    Console.Write("║ Senha: ".PadRight(67));
    string senha = Console.ReadLine();
    Console.Write("║ Confirmar Dados?(S/N): ".PadRight(67));
    string conf = Console.ReadLine() ?? "N";
    Console.WriteLine("╚" + barra + "╝");

    if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(senha)
        && conf.Trim().ToUpper() == "S")
        break;

    Console.Clear();
    Console.WriteLine("╔" + barra + "╗");
    Console.WriteLine("║" + Centraliza("Project Portfolio Management", 66) + "║");
    Console.WriteLine("║" + new string(' ', 66) + "║");
    Console.WriteLine("║ Usuário ou senha incorretos. Informe Novamente:".PadRight(67) + "║");
    Console.WriteLine("║" + new string(' ', 66) + "║");
    Console.Write("║ Pressione uma tecla...".PadRight(67) + "║\n");
    Console.WriteLine("╚" + barra + "╝");
    Console.ReadKey();
}

// ===== MENU PRINCIPAL =====
string opcao;
List<string> opcoes = new List<string>();
opcoes.Add("     Menu      ");
opcoes.Add("1 - Cadastrar Projeto");
opcoes.Add("2 - Exibir Projetos");
opcoes.Add("3 - Dashboard de Projetos");
opcoes.Add("4 - Fechamento Formal de Projeto");
opcoes.Add("5 - Cadastrar Recurso");
opcoes.Add("6 - Alocar Recurso");
opcoes.Add("7 - Balanço de Utilização de Recursos");
opcoes.Add("8 - Balanço Consolidado do Portfólio");
opcoes.Add("0 - Sair");

while (true)
{
    tela.PrepararTela("Project Portfolio Management");
    opcao = tela.MostrarMenu(opcoes, 2, 2);

    if (opcao == "0") break;
    else if (opcao == "1") projetoCRUD.GetType().GetMethod("CadastrarProjeto", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "2") projetoCRUD.GetType().GetMethod("ListarProjetos", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "3") projetoCRUD.GetType().GetMethod("DashboardProjetos", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "4") projetoCRUD.GetType().GetMethod("FechamentoFormal", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(projetoCRUD, null);
    else if (opcao == "5") recursoCRUD.GetType().GetMethod("Cadastrar", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD, null);
    else if (opcao == "6") recursoCRUD.GetType().GetMethod("Alocar", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD, null);
    else if (opcao == "7") recursoCRUD.GetType().GetMethod("Balanco", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(recursoCRUD, null);
    else if (opcao == "8") BalancoConsolidadoPlaceholder();
    else
    {
        tela.MostrarMensagem("Opção inválida. Pressione uma tecla para continuar...");
        Console.ReadKey();
    }
}

// ===== Helpers locais =====
void BalancoConsolidadoPlaceholder()
{
    Console.Clear();
    string barra = new string('═', 66);
    Console.WriteLine("╔" + barra + "╗");
    Console.WriteLine("║" + Centraliza("Balanço Consolidado do Portfólio", 66) + "║");
    Console.WriteLine("║" + new string(' ', 66) + "║");
    Console.WriteLine("║ Total de Projetos: ".PadRight(67) + "║");
    Console.WriteLine("║ Projetos em Análise: ".PadRight(67) + "║");
    Console.WriteLine("║ Projetos em Execução: ".PadRight(67) + "║");
    Console.WriteLine("║ Projetos Encerrados: ".PadRight(67) + "║");
    Console.WriteLine("║".PadRight(67) + "║");
    Console.Write("║ Voltar (V): ".PadRight(67) + "║\n");
    Console.WriteLine("╚" + barra + "╝");
    Console.ReadLine();
}

string Centraliza(string texto, int largura)
{
    if (texto.Length > largura) texto = texto.Substring(0, largura);
    int pad = (largura - texto.Length) / 2;
    return new string(' ', pad) + texto + new string(' ', largura - pad - texto.Length);
}
