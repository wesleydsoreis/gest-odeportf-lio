using System;
using System.Collections.Generic;
using System.Text;

public class Tela
{

    private int largura;
    private int altura;
    private int colunaInicial;
    private int linhaInicial;
    private bool telaCheia;

    private const char H = '═';
    private const char V = '║';
    private const char CSE = '╔';
    private const char CIE = '╚';
    private const char CSD = '╗';
    private const char CID = '╝';
    public Tela()
    {
        this.largura = 80;
        this.altura = 25;
        this.colunaInicial = 0;
        this.linhaInicial = 0;
        this.telaCheia = true;
    }

    public void PrepararTela(string titulo = "")
    {
        Console.Clear();

        MontarMoldura(this.colunaInicial, this.linhaInicial,
                      this.colunaInicial + this.largura - 1,
                      this.linhaInicial + this.altura - 1);

        MontarMoldura(this.colunaInicial, this.linhaInicial,
                      this.colunaInicial + this.largura - 1,
                      this.linhaInicial + 2);

        Centralizar(this.colunaInicial, this.colunaInicial + this.largura - 1, this.linhaInicial + 1, titulo);
    }

    public string MostrarMenu(List<string> opcoes, int ci, int li)
    {
        int linha = li + 2;

        if (opcoes.Count > 0)
        {
            Centralizar(ci, this.colunaInicial + this.largura - 2, li + 1, opcoes[0]);
            linha++;
        }

        for (int i = 1; i < opcoes.Count; i++)
        {
            Console.SetCursorPosition(ci + 2, linha++);
            Console.Write(opcoes[i]);
        }

    Console.SetCursorPosition(ci + 2, linha + 1);
    Console.Write("Opção : ");
    string? op = LerLinhaOuEsc();
    return op == null ? "ESC" : op.Trim();
    }

    public void MontarMoldura(int ci, int li, int cf, int lf)
    {
        if (cf <= ci || lf <= li) return;

        for (int col = ci + 1; col < cf; col++)
        {
            Console.SetCursorPosition(col, li);
            Console.Write(H);
            Console.SetCursorPosition(col, lf);
            Console.Write(H);
        }

        for (int lin = li + 1; lin < lf; lin++)
        {
            Console.SetCursorPosition(ci, lin);
            Console.Write(V);
            Console.SetCursorPosition(cf, lin);
            Console.Write(V);
        }

        Console.SetCursorPosition(ci, li); Console.Write(CSE);
        Console.SetCursorPosition(ci, lf); Console.Write(CIE);
        Console.SetCursorPosition(cf, li); Console.Write(CSD);
        Console.SetCursorPosition(cf, lf); Console.Write(CID);
    }

    public void ApagarArea(int ci, int li, int cf, int lf)
    {
        for (int linha = li; linha <= lf; linha++)
        {
            Console.SetCursorPosition(ci, linha);
            Console.Write(new string(' ', Math.Max(0, cf - ci + 1)));
        }
    }

    public void Centralizar(int ci, int cf, int lin, string msg)
    {
        int col = (cf - ci - msg.Length) / 2 + ci;
        Console.SetCursorPosition(col, lin);
        Console.Write(msg);
    }

    public void MostrarMensagem(string msg)
    {
        int ci = this.colunaInicial;
        int cf = this.colunaInicial + this.largura - 1;
        int li = this.linhaInicial + this.altura - 3;
        int lf = this.linhaInicial + this.altura - 1;

        MontarMoldura(ci, li, cf, lf);

        int linhaTexto = lf - 1;
        int col = (this.largura - msg.Length) / 2 + this.colunaInicial;

        ApagarArea(this.colunaInicial + 1, linhaTexto, this.colunaInicial + this.largura - 2, linhaTexto);
        Console.SetCursorPosition(Math.Max(this.colunaInicial + 1, col), linhaTexto);
        Console.Write(msg);
    }
    public string? LerLinhaOuEsc()
{
    var sb = new StringBuilder();

    while (true)
    {
        var k = Console.ReadKey(intercept: true);

        if (k.Key == ConsoleKey.Escape)
        {
            return null;
        }
        else if (k.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            return sb.ToString();
        }
        else if (k.Key == ConsoleKey.Backspace)
        {
            if (sb.Length > 0)
            {
                sb.Length--;
                Console.Write("\b \b");
            }
        }
        else if (!char.IsControl(k.KeyChar))
        {
            sb.Append(k.KeyChar);
            Console.Write(k.KeyChar);
        }
    }
}
public void MostrarRodapePadrao()
{
    int lin = this.linhaInicial + this.altura - 1;
    ApagarArea(this.colunaInicial + 1, lin, this.colunaInicial + this.largura - 1, lin);

    string msg = "Pressione ESC para voltar";
    int col = (this.largura - msg.Length) / 2 + this.colunaInicial;
    Console.SetCursorPosition(Math.Max(this.colunaInicial + 1, col), lin);
    Console.Write(msg);
}

}