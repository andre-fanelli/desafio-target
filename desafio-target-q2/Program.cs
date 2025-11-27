using System.Text.Json;


namespace DesafioTargetQ2
{
    internal class Program
    {
        static string filePath = "estoque.json";
        static List<Produto> produtos = new List<Produto>();
        static List<Movimentacao> movimentacoes = new List<Movimentacao>();
        static void Main(string[] args)
        {
            CarregarEstoque();

            Console.WriteLine(" Lista de Produtos em Estoque ");
            ListarProdutos();

            Console.WriteLine(" Nova Movimentação de Produto ");

            Console.Write("Digite o código do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int idProduto))
            {
                Console.WriteLine("Código inválido. Operação cancelada.");
                return;
            }

            Console.Write("Digite a quantidade a movimentar: ");
            if (!int.TryParse(Console.ReadLine(), out int qtd) || qtd <= 0)
            {
                Console.WriteLine("Quantidade inválida. Deve ser maior que zero.");
                return;
            }
            
            Console.WriteLine("Tipo de Movimentação: E para Entrada | S para Saída: ");
            string tipoMovimentacao = Console.ReadLine()?.ToUpper();
            string tipo = (tipoMovimentacao == "E") ? "Entrada" :  "Saída";

            Console.WriteLine("Digite o motivo/descricao: ");
            string motivo = Console.ReadLine();

            bool sucesso = RegistrarMovimentacao(idProduto, qtd, tipo, motivo);

            if (sucesso)
            {
                var produtoAtualizado = produtos.FirstOrDefault(p => p.CodigoProduto == idProduto);

                Console.WriteLine(" ------ RESUMO DA MOVIMENTAÇÃO ------ ");
                Console.WriteLine($"Código do Produto: {produtoAtualizado.CodigoProduto}");
                Console.WriteLine($"Descrição: {produtoAtualizado.DescricaoProduto}");
                Console.WriteLine($"Estoque Atualizado: {produtoAtualizado.Estoque}");
                Console.WriteLine($"Tipo de Movimentação: {tipo}");
                Console.WriteLine($"Motivo: {motivo}");
                
            }

            
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        static bool RegistrarMovimentacao(int idProduto, int qtd, string tipo, string motivo)
        {
            var produto = produtos.FirstOrDefault(p => p.CodigoProduto == idProduto);
            if (produto == null)
            {
                Console.WriteLine($"Produto {idProduto} não encontrado. Operação cancelada.");
                return false;
            }

            if (tipo == "Saída" && produto.Estoque < qtd)
            {
                Console.WriteLine("Estoque insuficiente para a saída. Operação cancelada.");
                return false;
            }

            if (tipo == "Entrada")
                produto.Estoque += qtd;
            else
                produto.Estoque -= qtd;

            int idMovimentacao;
            do
            {
                idMovimentacao = new Random().Next(10000, 99999);
            } 
            while (movimentacoes.Any(m => m.IdMovimentacao == idMovimentacao));

            var log = new Movimentacao
            {
                IdMovimentacao = idMovimentacao,
                NomeProduto = produto.DescricaoProduto,
                Quantidade = qtd,
                Tipo = tipo,
                Motivo = motivo,
                DataHora = DateTime.Now
            };

            movimentacoes.Add(log);
            SalvarEstoque();

            Console.WriteLine($"Movimentação registrada! ID: {log.IdMovimentacao}");
            return true;
        }

        static void CarregarEstoque()
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                produtos = JsonSerializer.Deserialize<List<Produto>>(jsonString, opt) ?? new List<Produto>();
            }
            else
            {
                Console.WriteLine("Arquivo estoque.json não encontrado.");
            }
        }

        static void SalvarEstoque()
        {
            var opt = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(produtos, opt);
            File.WriteAllText(filePath, jsonString);
        }

        static void ListarProdutos()
        {
            foreach (var produto in produtos)
            {
                Console.WriteLine($"Código do Produto: {produto.CodigoProduto} | Descrição: {produto.DescricaoProduto} | Estoque: {produto.Estoque}");
            }
        }
    }
}

public class Produto
{
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; }
    public int Estoque { get; set; }
}

public class Movimentacao
{
    public int IdMovimentacao { get; set; }
    public string NomeProduto { get; set; }
    public int Quantidade { get; set; }
    public string Tipo { get; set; }
    public string Motivo { get; set; }
    public DateTime DataHora { get; set; }
}