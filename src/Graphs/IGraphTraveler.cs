using System.Collections.Generic;

namespace DG.Heuristic.Graphs
{
    public interface IGraphTraveler<TData> where TData : IGraphPoint<TData>
    {
        List<TData> CalculateRoute(out double totalDistance);
    }
}
