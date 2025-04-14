using System.Collections.Generic;

namespace DG.Heuristic.Graphs
{
    public interface IGraphTraveler<TData> where TData : IPositionedData
    {
        void Add(TData data);

        List<TData> CalculateRoute(out double totalDistance);
    }
}
