using System;

namespace DesafioMutant
{
    class Program
    {
        static void Main(string[] args)
        {
            string op = "";

            do
            {
                Console.WriteLine("\n\n-------------------------------");
                Console.WriteLine("1 - Baixar Dados");
                Console.WriteLine("2 - Salvar Dados");
                Console.WriteLine("3 - Sair");
                Console.WriteLine("-------------------------------\n\n");

                op = Console.ReadLine();
                Console.Clear();

                switch (op)
                {
                    case "1":
                        Console.WriteLine("Aguarde...\n\n");
                        Console.WriteLine(Dados.BaixarDados("https://jsonplaceholder.typicode.com/users"));
                        break;
                    case "2":
                        var list = Dados.BaixarDados("https://jsonplaceholder.typicode.com/users");
                        Dados.SalvarDados(list);
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            while (op != "3");
        }
    }
}
