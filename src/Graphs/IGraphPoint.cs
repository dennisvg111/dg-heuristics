namespace DG.Heuristic.Graphs
{
    public interface IGraphPoint<TData> where TData : IGraphPoint<TData>
    {
        double DistanceTo(TData data);
    }
}
