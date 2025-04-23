namespace DG.Heuristic.Graphs.Tsp
{
    /// <summary>
    /// Represents the probability that a certain point is chosen as next point in a route.
    /// </summary>
    public class PointProbability
    {
        /// <summary>
        /// The initial index of the point.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// <para>The score this point gets, relative to other possible points to travel to.</para>
        /// <para>A higher score should be better.</para>
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The distance this point is from the previous point.
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Updates the values of this <see cref="PointProbability"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="distance"></param>
        /// <param name="score"></param>
        public void Update(int index, double distance, double score)
        {
            Index = index;
            Distance = distance;
            Score = score;
        }

        /// <summary>
        /// Returns a string representation of this <see cref="PointProbability"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{Index}] {Distance}, score: {Score}";
        }

        /// <summary>
        /// Returns a new instance of <see cref="PointProbability"/>, with all values set to 0.
        /// </summary>
        public static PointProbability Zero
        {
            get
            {
                return new PointProbability()
                {
                    Index = 0,
                    Score = 0,
                    Distance = 0
                };
            }
        }
    }
}
