using System.Collections.Generic;

namespace DG.Heuristic
{
    /// <summary>
    /// Represents a collection where items can be added or cleared.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMutableCollection<T>
    {
        /// <summary>
        /// <para>Adds the given <paramref name="item"/> to this collection.</para>
        /// </summary>
        /// <param name="item">The item to be added</param>
        /// <returns>Returns a value indicating indicating if this <paramref name="item"/> was added (<see langword="true"/>), or ignored (<see langword="false"/>).</returns>

        bool Add(T item);

        /// <summary>
        /// Removes all items from this collection.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Provides extension methods for types that implement <see cref="IMutableCollection{T}"/>.
    /// </summary>
    public static class IMutableCollectionExtensions
    {
        /// <summary>
        /// Adds multiple items to the given <paramref name="collection"/> at once.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection the items will be added to.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRange<T>(this IMutableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
