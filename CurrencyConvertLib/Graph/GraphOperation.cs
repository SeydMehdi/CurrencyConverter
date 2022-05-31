using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConvertLib.Graph
{
    public class GraphOperation<T>
    {
        private Graph<T> graph;
        public GraphOperation(IEnumerable<Tuple<T, T>> map)
        {
            var vertices = map.Select(m => m.Item1).Distinct().Union(map.Select(m => m.Item2).Distinct()).OrderBy(m => m);
            graph = new Graph<T>(vertices, map);
        }

        public List<T> ShortestPath(T from, T to)
        {
            var shortestPath = ShortestPathFunction(from);
            return shortestPath(to).ToList();
        }

        public Func<T, IEnumerable<T>> ShortestPathFunction(T start)
        {
            var previous = new Dictionary<T, T>();
            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in graph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v =>
            {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    if (!previous.ContainsKey(current))
                        return new List<T>(); // no path exist
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse();

                return path;
            };

            return shortestPath;
        }

    }


}
