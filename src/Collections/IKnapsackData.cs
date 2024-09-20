namespace DG.Heuristic.Collections
{
    public interface IKnapsackData : IWeightedData
    {
        /// <summary>
        /// The relative value of this item.
        /// </summary>
        double Value { get; }
    }
}
