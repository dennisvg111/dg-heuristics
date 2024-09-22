using DG.Heuristic.Collections;
using System;
using System.Linq;

namespace DG.Heuristic.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string value;
            int target;
            do
            {
                Console.Write("Enter knapsack target: ");
                value = Console.ReadLine();
            } while (!int.TryParse(value, out target));
            SubsetSumKnapsack knapsack = new SubsetSumKnapsack(target);

            string input;
            do
            {
                Console.Write("Enter numbers for knapsack: ");
                input = Console.ReadLine();
                var parsedNumbers = input.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(n => int.TryParse(n, out int r) ? r : 0).Where(n => n != 0);
                if (!parsedNumbers.Any())
                {
                    break;
                }

                knapsack.Clear();
                knapsack.Add(parsedNumbers);

                var solution = knapsack.PickClosest(out int sum);
                Console.WriteLine($"Found solution: [{string.Join(", ", solution)}] = {sum}");
            } while (!string.IsNullOrEmpty(input));



        }
    }
}
