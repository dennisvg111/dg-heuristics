using System;
using System.Collections.Generic;

namespace DG.Heuristic.Examples.Test.Knapsack
{
    public class KnapsackDataGenerator : ITestDataGenerator<KnapsackData>
    {
        public KnapsackData GenerateTestData(Random random)
        {
            var target = random.Next(10, 2000);

            var data = new List<KnapsackData.TestData>();
            for (int i = 0; i < random.Next(10, 500); i++)
            {
                data.Add(new KnapsackData.TestData(random.Next(1, 100)));
            }

            return new KnapsackData(target, data);
        }
    }
}
