using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NodeSimulator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*NodeLayout layout = new NodeLayout();
            layout.createRecursiveOctagon(6, 0, 0);
            layout.outputToFile("RecursiveOctagon");

            NodeLayout readLayout = new NodeLayout();
            readLayout.inputFromFile("RecursiveOctagon");*/
            List<double> longestDist = new List<double>();
            List<int> longestPath = new List<int>();
            List<double> ratio = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                GetNums(i, longestDist, longestPath, ratio);
            }
            Application.Run(new PrimaryForm());
        }

        static void GetNums(int i, List<double> longestDist, List<int> longestPath, List<double> ratio)
        {
            NodeLayout layout = new NodeLayout();
            layout.createRecursiveOctagon(i);

            Node startNode = layout.idLookup[1];
            Node furthestNode = startNode;
            double dist = 0;
            foreach (Node node in layout.idLookup.Values)
            {
                double testDist = Math.Pow(Math.Pow(startNode.getX - node.getX, 2) + Math.Pow(startNode.getY - node.getY, 2), 0.5);
                if (testDist > dist)
                {
                    dist = testDist;
                    furthestNode = node;
                }
            }
            longestDist.Add(dist);

            Dictionary<Node, int> distances = new Dictionary<Node, int>();
            Dictionary<int, List<Node>> isodistances = new Dictionary<int, List<Node>>();
            Dictionary<int, List<Node>> unexplored = new Dictionary<int, List<Node>>();
            unexplored[0] = new List<Node>() { startNode };

            while (unexplored.Keys.Any())
            {
                int lowest = unexplored.Keys.Min();
                List<Node> frontier = unexplored[lowest];
                unexplored.Remove(lowest);
                if (frontier.Count == 0)
                    continue;

                if (!unexplored.ContainsKey(lowest + 1))
                {
                    unexplored[lowest + 1] = new List<Node>();
                }
                List<Node> nextFrontier = unexplored[lowest + 1];

                foreach (Node node in frontier)
                {
                    if (!distances.ContainsKey(node))
                    {
                        distances[node] = lowest;
                        if (!isodistances.ContainsKey(lowest))
                        {
                            isodistances[lowest] = new List<Node>();
                        }
                        isodistances[lowest].Add(node);
                        foreach (Node neighbors in node.getAdjNodes())
                        {
                            if (!distances.ContainsKey(neighbors) && !nextFrontier.Contains(neighbors))
                            {
                                nextFrontier.Add(neighbors);
                            }
                        }
                    }
                }
            }
            longestPath.Add(distances.Values.Max());
            ratio.Add((double)longestPath.Last() / longestDist.Last());
        }
    }
}
