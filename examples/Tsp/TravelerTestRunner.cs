using DG.Heuristic.Graphs;
using System;
using System.Collections.Generic;

namespace DG.Heuristic.Examples.Tsp
{
    public class TravelerTestRunner<TTraveler> : ITestRunner<List<GraphPoint>, double> where TTraveler : IGraphTraveler<GraphPoint>
    {
        private readonly Func<List<GraphPoint>, TTraveler> _travelerFactory;

        public TravelerTestRunner(Func<List<GraphPoint>, TTraveler> travelerFactory)
        {
            _travelerFactory = travelerFactory;
        }

        public string Name => typeof(TTraveler).Name.Replace("`1", "");

        public double Run(List<GraphPoint> input)
        {
            var traveler = _travelerFactory(input);
            traveler.CalculateRoute(out double distance);
            return distance;
        }
    }

    public static class TravelerTestRunner
    {
        public static TravelerTestRunner<TTraveler> For<TTraveler>() where TTraveler : IGraphTraveler<GraphPoint>, IMutableCollection<GraphPoint>, new()
        {
            return For((data) =>
            {
                var traveler = new TTraveler();
                traveler.AddRange(data);
                return traveler;
            });
        }

        public static TravelerTestRunner<TTraveler> For<TTraveler>(Func<List<GraphPoint>, TTraveler> travelerFactory) where TTraveler : IGraphTraveler<GraphPoint>
        {
            return new TravelerTestRunner<TTraveler>(travelerFactory);
        }
    }
}
