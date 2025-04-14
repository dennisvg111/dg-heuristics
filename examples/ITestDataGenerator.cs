using System;

namespace DG.Heuristic.Examples
{
    public interface ITestDataGenerator<TData>
    {
        TData GenerateTestData(Random random);
    }
}