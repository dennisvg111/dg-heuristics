using DG.Heuristic.Graphs;
using System;

namespace DG.Heuristic.Examples
{
    public class GraphPoint : IGraphPoint<GraphPoint>
    {
        private readonly int _x;
        private readonly int _y;

        public GraphPoint() : this(0, 0)
        {
        }

        public GraphPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X => _x;
        public int Y => _y;

        public double DistanceTo(GraphPoint data)
        {
            return Math.Sqrt(Math.Abs((_x * _x + _y * _y) - (data._x * data._x + data._y * data._y)));
        }

        private static GraphPoint _zero = new GraphPoint(0, 0);
        public static GraphPoint Zero => _zero;
    }
}
