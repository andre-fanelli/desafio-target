namespace DesafioTargetQ3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal valor;
            decimal juros = 0.025m;
            DateTime dataVencimento;

            Console.WriteLine("Digite o valor a ser pago: ");
            if (!decimal.TryParse(Console.ReadLine(), out valor) || valor <= 0)
            {
                Console.WriteLine("Valor inválido. Deve ser maior que zero.");
                return;
            }
            Console.WriteLine("Digite a data de vencimento (dd/MM/yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out dataVencimento))
            {
                Console.WriteLine("Data inválida. Operação cancelada.");
                return;
            }
            DateTime dataAtual = DateTime.Now;
            if (dataVencimento < dataAtual)
            {
                TimeSpan diferenca = dataAtual - dataVencimento;
                int diasAtraso = diferenca.Days;
                decimal valorJuros = valor * juros * diasAtraso;
                decimal valorTotal = valor + valorJuros;

                Console.WriteLine(" ---- DADOS DO PAGAMENTO ---- ");
                Console.WriteLine($"Valor Original: R$ {valor:F2}");
                Console.WriteLine($"Data de Vencimento: {dataVencimento:dd/MM/yyyy}");
                Console.WriteLine($"Data Atual: {dataAtual:dd/MM/yyyy}");
                Console.WriteLine($"Dias em Atraso: {diasAtraso} dia(s)");
                Console.WriteLine($"Juros Aplicados: R$ {valorJuros:F2}");
                Console.WriteLine($"Valor Total a Pagar: R$ {valorTotal:F2}");
            }
            else
            {
                Console.WriteLine(" ---- DADOS DO PAGAMENTO ---- ");
                Console.WriteLine($"Valor a Pagar: R$ {valor:F2}");
                Console.WriteLine("Pagamento realizado dentro do prazo. Sem juros aplicados.");
            }
        }
    }
}