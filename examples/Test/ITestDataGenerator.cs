using System;

namespace DG.Heuristic.Examples.Test
{
    public interface ITestDataGenerator<TData>
    {
        TData GenerateTestData(Random random);
    }
}