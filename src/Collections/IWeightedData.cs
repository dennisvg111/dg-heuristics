namespace DG.Heuristic.Collections
{
    /// <summary>
    /// Represents data that has a specific "weight".
    /// </summary>
    public interface IWeightedData
    {
        /// <summary>
        /// The relative weight of this item.
        /// </summary>
        int Weight { get; }
    }
}
