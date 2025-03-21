namespace DG.Heuristic.Collections
{
    /// <summary>
    /// Represents data that has a both a "weight" and a "value".
    /// </summary>
    public interface IKnapsackData : IWeightedData
    {
        /// <summary>
        /// The relative value of this item.
        /// </summary>
        double Value { get; }
    }
}
