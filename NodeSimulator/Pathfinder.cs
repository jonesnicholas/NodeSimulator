using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSimulator
{
    public class Pathfinder
    {
        /// <summary>
        /// Uses Dijkstra's algorithm to find the shortest path from one node to another
        /// </summary>
        /// <param name="layout">The layout of the graph</param>
        /// <param name="start">The start node for pathfinding</param>
        /// <param name="end">The destination node for pathfinding</param>
        /// <returns>A list of (Node, double)s representing the shortest path from start to end, along with the total
        /// distances remaining from each step in the path until the end</returns>
        public static List<(Node, double)> DijkstraPath(NodeLayout layout, Node start, Node end)
        {
            PriorityQueue<PathNode> frontier = new PriorityQueue<PathNode>();
            Dictionary<Node, PathNode> explored = new Dictionary<Node, PathNode>();

            PathNode startPathNode = new PathNode(start, null, 0.0);

            frontier.Enqueue(startPathNode, 0.0);
            bool foundEnd = false;
            while (!foundEnd && frontier.Count > 0)
            {
                (PathNode NextNode, double dist) = frontier.Dequeue();
                if (explored.ContainsKey(NextNode.node))
                {
                    continue;
                }
                explored.Add(NextNode.node, NextNode);
                if (NextNode.node == end)
                {
                    foundEnd = true;
                    break;
                }
                foreach (Connection connection in NextNode.node.getOutgoingConnections().Where(con => !explored.ContainsKey(con.getDestination)))
                {
                    double totalDist = connection.getLength + dist;
                    double pri = totalDist;
                    PathNode newFrontier = new PathNode(connection.getDestination, connection.getSource, totalDist);
                    frontier.Enqueue(newFrontier, pri);
                }
            }
            if (!foundEnd)
            {
                throw new Exception($"Can't find any path between nodes {start} and {end}");
            }

            List<(Node, double)> path = new List<(Node, double)>();
            Node pathTracer = end;
            while (pathTracer != start)
            {
                PathNode pathNode = explored[pathTracer];
                path.Add((pathTracer, pathNode.totalDist));
                pathTracer = pathNode.from;
            }
            path.Add((start, 0.0));
            return path;
        }

        /// <summary>
        /// Recursively searches for every possible path from start to end to find the shortest one, while explicitly
        /// avoiding nodes in the "visited" list to avoid visiting nodes more than once
        /// </summary>
        /// <param name="layout">The layout of the graph</param>
        /// <param name="start">The start node for pathfinding</param>
        /// <param name="end">The destination node for pathfinding</param>
        /// <param name="visited">Nodes 'previously visited' which need to be avoided</param>
        /// <returns>A list of (Node, double)s representing the shortest path from start to end, along with the total
        /// distances remaining from each step in the path until the end</returns>
        public static List<(Node, double)> ExhaustivePath(NodeLayout layout, Node start, Node end, List<Node> visited = null)
        {
            if (start == end)
            {
                return new List<(Node, double)>() { (end, 0.0) };
            }
            visited = visited ?? new List<Node>();
            List<Connection> nextOptions = start.getOutgoingConnections().Where(con => !visited.Contains(con.getDestination)).ToList();
            if (nextOptions.Count == 0)
            {
                return new List<(Node, double)>() { (end, double.MaxValue) };
            }

            double shortest = double.MaxValue;
            List<(Node, double)> shortPath = new List<(Node, double)>() { (end, double.MaxValue) };
            foreach(Connection con in nextOptions)
            {
                List<Node> recVisited = new List<Node>(visited);
                recVisited.Add(start);
                List<(Node, double)> recPath = ExhaustivePath(layout, con.getDestination, end, recVisited);
                double length = recPath[0].Item2;
                if (length + con.getLength < shortest && con.getLength <= double.MaxValue - length)
                {
                    shortest = length + con.getLength;
                    shortPath = new List<(Node, double)>();
                    shortPath.Add((start, shortest));
                    shortPath.AddRange(recPath);
                }
            }
            return shortPath;
        }

        public static List<(Node, double)> AStar(NodeLayout layout, Node start, Node end, Dictionary<Node, double> heuristic)
        {
            Dictionary<Node, PathNode> explored = new Dictionary<Node, PathNode>();
            PriorityQueue<PathNode> frontier = new PriorityQueue<PathNode>();
            frontier.Enqueue(new PathNode(start, null, 0.0), 0.0);

            // reminder, for an admissible output, H(n) must never overestimage the actual distance from n to end
            // for an optimal output, for every edge (x,y), h(x) <= d(x,y) + h(y)
            foreach(Node node in layout.nodes.Values)
            {
                if (!heuristic.ContainsKey(node))
                {
                    throw new Exception($"A* requires all nodes have heuristic value. node {node} is lacking one");
                }
            }

            bool foundEnd = false;
            while (frontier.Count > 0)
            {
                (PathNode NextNode, double dist) = frontier.Dequeue();
                if (!explored.ContainsKey(NextNode.node))
                {
                    explored.Add(NextNode.node, NextNode);
                }
                if (NextNode.node == end)
                {
                    foundEnd = true;
                    break;
                }
                foreach (Connection connection in NextNode.node.getOutgoingConnections())
                {
                    double totalDist = connection.getLength + dist;
                    double pri = totalDist + heuristic[connection.getDestination];
                    PathNode newFrontier = new PathNode(connection.getDestination, connection.getSource, totalDist);
                    frontier.Enqueue(newFrontier, pri);
                }
            }

            if (!foundEnd)
            {
                throw new Exception($"Can't find any path between nodes {start} and {end}");
            }

            List<(Node, double)> path = new List<(Node, double)>();
            Node pathTracer = end;
            while (pathTracer != start)
            {
                PathNode pathNode = explored[pathTracer];
                path.Add((pathTracer, pathNode.totalDist));
                pathTracer = pathNode.from;
            }
            path.Add((start, 0.0));
            return path;
        }

        private class PathNode
        {
            public Node node;
            public Node from;
            public double totalDist;

            public PathNode(Node toNode, Node fromNode, double dist)
            {
                node = toNode; from = fromNode; totalDist = dist;
            }
        }
    }

}
