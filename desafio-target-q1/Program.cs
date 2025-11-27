using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DesafioTargetQ1;
public class Program
{
    public static void Main(string[] args)
    {
        string filePath = "vendas.json";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("O arquivo vendas.json não foi encontrado.");
            return;
        }

        string jsonString = File.ReadAllText(filePath);

        var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        List<Venda>? ListaVendas = JsonSerializer.Deserialize<List<Venda>>(jsonString, opt) ?? new List<Venda>();

        Console.WriteLine("-- Relatório de Comissões por Vendedor --");

        foreach (var venda in ListaVendas)
        {
            decimal comissao = CalcularComissao(venda.Valor);   
            Console.WriteLine($"{venda.Vendedor}: R$ {venda.Valor:F2} - Comissão: R$ {comissao:F2}"); 
        }

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static decimal CalcularComissao(decimal valor)
    {
        if (valor >= 100 && valor < 500)
            return valor * 0.01m;
        else if (valor >= 500)
            return valor * 0.05m;
        else
            return 0m;
    }

    public class Venda
    {
        public string? Vendedor { get; set; }
        public decimal Valor { get; set; }
        public decimal Comissao { get; set; }
    }
}