using System.Collections.Generic;

namespace DG.Heuristic.Graphs
{
    public interface IGraphTraveler<TData> where TData : IGraphPoint<TData>
    {
        /// <summary>
        /// <para>Calculates a route that reaches all points in a graph.</para>
        /// <para>The result should be an open circuit, AKA the starting point is not the same as the ending point.</para>
        /// </summary>
        /// <param name="totalDistance"></param>
        /// <returns></returns>
        List<TData> CalculateRoute(out double totalDistance);
    }
}
