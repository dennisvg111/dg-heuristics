namespace DG.Heuristic.Graphs.Tsp
{
    /// <summary>
    /// Represents the configuration options for the Ant Colony Optimization algorithm used in the AntColonyTraveler.
    /// </summary>
    public class AntColonyOptions
    {
        /// <summary>
        /// Gets or sets the number of ants used in the algorithm. 
        /// This value determines how many ants are concurrently exploring the solution space.
        /// </summary>
        public int AntCount { get; set; } = 30;

        /// <summary>
        /// Gets or sets the influence of pheromone in the decision-making process.
        /// <para>
        /// This value controls how strongly ants are influenced by pheromone levels when selecting the next point. 
        /// Higher values of <c>Alpha</c> make ants more likely to follow pheromone trails, whereas lower values make the search more random.
        /// </para>
        /// Example:
        /// - <c>Alpha = 1.0</c>: Equal influence of pheromone and distance.
        /// - <c>Alpha = 5.0</c>: Stronger emphasis on pheromone, leading to shorter exploration paths.
        /// - <c>Alpha = 0.1</c>: Less emphasis on pheromone, leading to more exploration and potentially finding better global solutions.
        /// </summary>
        public double Alpha { get; set; } = 1.0;

        /// <summary>
        /// Gets or sets the influence of distance in the decision-making process.
        /// <para>
        /// This value controls how strongly the distance between points influences the selection of the next point.
        /// Higher values of <c>Beta</c> make ants more likely to choose shorter paths, whereas lower values allow for more exploration.
        /// </para>
        /// Example:
        /// - <c>Beta = 1.0</c>: Equal importance of pheromone and distance.
        /// - <c>Beta = 5.0</c>: Stronger emphasis on selecting shorter paths, making ants more greedy.
        /// - <c>Beta = 0.1</c>: Less emphasis on distance, encouraging more exploratory behavior.
        /// </summary>
        public double Beta { get; set; } = 5.0;

        /// <summary>
        /// Gets or sets the rate at which pheromone evaporates over time.
        /// <para>
        /// The evaporation rate determines how quickly pheromones fade. A higher evaporation rate makes ants forget pheromone trails more quickly,
        /// causing the algorithm to be more exploratory. A lower rate makes pheromones last longer, leading to exploitation of good paths.
        /// </para>
        /// Example:
        /// - <c>EvaporationRate = 0.5</c>: Pheromones evaporate at a moderate rate, balancing exploration and exploitation.
        /// - <c>EvaporationRate = 0.1</c>: Pheromones last longer, encouraging the ants to follow established trails.
        /// - <c>EvaporationRate = 0.9</c>: Pheromones evaporate quickly, encouraging the ants to explore new paths more often.
        /// </summary>
        public double EvaporationRate { get; set; } = 0.5;

        /// <summary>
        /// Gets or sets the constant used to deposit pheromones based on the distance of the route.
        /// <para>
        /// This value influences how much pheromone an ant deposits on the path it travels. Higher values of <c>Q</c> result in ants depositing more pheromone,
        /// which can make shorter paths attract more ants. Smaller values of <c>Q</c> make the pheromone deposits less influential.
        /// </para>
        /// Example:
        /// - <c>Q = 100.0</c>: A high pheromone deposit, leading to quicker convergence to short paths.
        /// - <c>Q = 10.0</c>: A smaller pheromone deposit, encouraging ants to explore more diverse paths.
        /// - <c>Q = 1.0</c>: Very small pheromone deposit, making ants more likely to explore novel solutions.
        /// </summary>
        public double Q { get; set; } = 100.0;

        /// <summary>
        /// Gets or sets the number of iterations the algorithm will run before stopping.
        /// <para>
        /// This value sets the upper limit on the number of iterations the algorithm will perform. A higher value means the algorithm will have more time to find a better solution,
        /// but it will also take longer to run. The early exit condition based on stagnation can shorten the actual runtime.
        /// </para>
        /// Example:
        /// - <c>Iterations = 100</c>: The algorithm will run for up to 100 iterations.
        /// - <c>Iterations = 200</c>: The algorithm will run for up to 200 iterations, potentially finding a better solution but taking longer.
        /// </summary>
        public int Iterations { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum number of consecutive iterations without progress before the simulation is stopped.
        /// <para>
        /// This parameter allows for early termination of the algorithm if it has not found a better solution within a specified number of iterations.
        /// If no improvement in the best distance is observed for <c>StagnationMax</c> iterations, the algorithm will terminate early, improving efficiency.
        /// </para>
        /// Example:
        /// - <c>StagnationMax = 25</c>: If no improvement in the best route is found for 25 iterations, the algorithm will stop early.
        /// - <c>StagnationMax = 50</c>: The algorithm will be more patient, allowing for 50 iterations without improvement before stopping.
        /// </summary>
        public int StagnationMax { get; set; } = 25;
    }

}
