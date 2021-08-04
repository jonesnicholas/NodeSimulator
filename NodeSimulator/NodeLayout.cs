using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSimulator
{
    public class NodeLayout
    {
        public Dictionary<Tuple<int, int>, Node> nodes;
        public Dictionary<int, Node> idLookup;
        static int idCounter = 0;

        public NodeLayout()
        {
            init();
        }

        public void init()
        {
            nodes = new Dictionary<Tuple<int, int>, Node>();
            idLookup = new Dictionary<int, Node>();
        }

        public static int nextIdentifier()
        {
            idCounter++;
            return idCounter;
        }

        public void addNode(Node node)
        {
            nodes[new Tuple<int, int>(node.getX, node.getY)] = node;
            idLookup[node.getId] = node;
        }

        public void addConnection(Node node, Node other, bool mutual = false)
        {
            node.AddNeighbor(other, mutual);
        }

        public void addConnection(Node node, int xOther, int yOther, bool mutual = false)
        {
            addConnection(node, nodes[new Tuple<int, int>(xOther, yOther)], mutual);
        }

        public void addConnection(int id1, int id2, bool mutual = false)
        {
            addConnection(idLookup[id1], idLookup[id2], mutual);
        }

        #region Create Sample Layouts
        public void createSimpleNodeGrid(int D = 5)
        {
            for (int x = 0; x < D; x++)
            {
                for (int y = 0; y < D; y++)
                {
                    Node node = new Node(x, y);
                    addNode(node);
                    if (x > 0)
                    {
                        addConnection(node, x - 1, y, mutual: true);
                    }
                    if (y > 0)
                    {
                        addConnection(node, x, y - 1, mutual: true);
                    }
                }
            }
        }

        public void createRecursiveOctagon(int recurse = 0, int x0 = 0, int y0 = 0, List<int> Ws = null, List<int> Hs = null)
        {
            Ws = Ws ?? new List<int>() { 1, 3 };
            Hs = Hs ?? new List<int>() { 2, 7 };
            while (recurse + 1 > Ws.Count)
            {
                int n = Ws.Count;
                Ws.Add(Ws[n - 1] + Hs[n - 1]);
                Hs.Add(4 * Hs[n - 1] - 2 * Hs[n - 2]);
            }
            int xl = Ws[0];
            int yl = Hs[0];
            if (recurse > 0)
            {
                xl = Ws[recurse] - Ws[recurse - 1];
                yl = Hs[recurse] - Hs[recurse - 1];
            }
            Node prevNode = null;
            Node firstNode = null;
            for (int i = 0; i < 8; i ++)
            {
                var xyf = (i + 1) / 2 % 2;
                var xn = i / 4;
                var yn = (i + 2) / 4 % 2;

                int xPos = (xyf == 1 ? yl : xl) * (xn == 1 ? -1 : 1) + x0;
                int yPos = (xyf == 1 ? xl : yl) * (yn == 1 ? -1 : 1) + y0;

                if (recurse != 0)
                {
                    createRecursiveOctagon(recurse - 1, xPos, yPos, Ws, Hs);
                }
                else
                {
                    if (!nodes.ContainsKey(new Tuple<int, int>(xPos, yPos)))
                    {
                        addNode(new Node(xPos, yPos));
                    }
                    Node node = nodes[new Tuple<int, int>(xPos, yPos)];
                    if (prevNode != null)
                    {
                        addConnection(node, prevNode, mutual: true);
                    }
                    prevNode = node;
                    if (i == 0)
                    {
                        firstNode = node;
                    }
                    if (i == 7)
                    {
                        addConnection(firstNode, node);
                    }
                }
            }
        }
        #endregion

        #region Save/Load Layout file
        public void outputToFile(String name, String fileLocation = null)
        {
            fileLocation = fileLocation ?? "C:\\Users\\nijon.REDMOND\\Desktop\\NodeConfigs";
            Directory.CreateDirectory(fileLocation);
            String filePath = fileLocation + "\\" + name + ".txt";

            StreamWriter writer = new StreamWriter(filePath, append: false);
            writer.WriteLine("Nodes!");
            writer.Close();

            writer = new StreamWriter(filePath, append: true);

            writer.WriteLine(nodes.Count);

            List<Connection> connections = new List<Connection>();
            foreach (Node node in nodes.Values)
            {
                writer.WriteLine(node.getName);
                writer.WriteLine($"{node.getId} {node.getX} {node.getY}");
                foreach (Node neighbor in node.getAdjNodes())
                {
                    connections.Add(new Connection(node, neighbor));
                }
            }
            writer.WriteLine(connections.Count);
            foreach (Connection con in connections)
            {
                writer.WriteLine($"{con.getSource.getId} {con.getDestination.getId}");
            }
            writer.Close();
        }

        public void inputFromFile(String name, String fileLocation = null)
        {
            fileLocation = fileLocation ?? "C:\\Users\\nijon.REDMOND\\Desktop\\NodeConfigs";
            Directory.CreateDirectory(fileLocation);
            String filePath = fileLocation + "\\" + name + ".txt";

            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            int numNodes = int.Parse(reader.ReadLine());
            for (int i = 0; i < numNodes; i++)
            {
                string nodeName = reader.ReadLine();
                string parameterLine = reader.ReadLine();
                string[] pars = parameterLine.Split(' ');
                int id = int.Parse(pars[0]);
                int x = int.Parse(pars[1]);
                int y = int.Parse(pars[2]);
                Node node = new Node(x, y, nodeName, id);

                if (nodes.ContainsKey(new Tuple<int, int>(x, y)))
                    throw new Exception("Current implementation does not allow multiple nodes at same coordinates");

                addNode(node);
            }
            int numConnections = int.Parse(reader.ReadLine());
            for (int i = 0; i < numConnections; i++)
            {
                string parameterLine = reader.ReadLine();
                string[] pars = parameterLine.Split(' ');
                int sourceId = int.Parse(pars[0]);
                int destId = int.Parse(pars[1]);
                addConnection(sourceId, destId);
            }
        }
        #endregion
    }
}
