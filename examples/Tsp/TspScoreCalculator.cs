using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp
{
    public class TspScoreCalculator : ITestScoreCalculator<List<GraphPoint>, double>
    {
        private static readonly TspScoreCalculator _instance = new TspScoreCalculator();

        public static TspScoreCalculator Instance => _instance;

        public double CalculateTestScore(List<GraphPoint> data, double testResult)
        {
            if (data.Count <= 1)
                return 1.0;

            double[,] distanceMatrix = BuildDistanceMatrix(data);
            double mstWeight = CalculateMstWeight(distanceMatrix);
            return mstWeight / testResult;
        }

        private double[,] BuildDistanceMatrix(List<GraphPoint> points)
        {
            int count = points.Count;
            var matrix = new double[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i == j) matrix[i, j] = 0;
                    else matrix[i, j] = points[i].DistanceTo(points[j]);
                }
            }

            return matrix;
        }

        private double CalculateMstWeight(double[,] distanceMatrix)
        {
            int count = distanceMatrix.GetLength(0);
            bool[] visited = new bool[count];
            double[] minEdge = new double[count];

            for (int i = 0; i < count; i++)
                minEdge[i] = double.MaxValue;

            minEdge[0] = 0;
            double totalWeight = 0;

            for (int i = 0; i < count; i++)
            {
                int u = -1;
                for (int j = 0; j < count; j++)
                {
                    if (!visited[j] && (u == -1 || minEdge[j] < minEdge[u]))
                    {
                        u = j;
                    }
                }

                visited[u] = true;
                totalWeight += minEdge[u];

                for (int v = 0; v < count; v++)
                {
                    if (!visited[v] && distanceMatrix[u, v] < minEdge[v])
                    {
                        minEdge[v] = distanceMatrix[u, v];
                    }
                }
            }

            return totalWeight;
        }
    }
}
