using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSimulator
{
    public class Node
    {
        private int id;
        private string name;
        private int X;
        private int Y;
        private Dictionary<Node, Connection> neighbors;

        public Node(int xin = 0, int yin = 0, string inName = null, int identity = 0)
        {
            init(xin, yin, inName, identity);
        }

        private void init(int xin, int yin, string inName, int ident)
        {
            X = xin;
            Y = yin;
            if (ident == 0)
            {
                ident = NodeLayout.nextIdentifier();
            }
            id = ident;
            name = inName ?? $"[{X} {Y}]";
            neighbors = new Dictionary<Node, Connection>();
        }

        public int getX => X;
        public int getY => Y;
        public string getName => name;
        public int getId => id;

        public List<Node> getAdjNodes()
        {
            List<Node> output = new List<Node>();
            output.AddRange(neighbors.Keys);
            return output;
        }

        public List<Connection> getOutgoingConnections()
        {
            List<Connection> output = new List<Connection>();
            output.AddRange(neighbors.Values);
            return output;
        }

        public void AddNeighbor(Node other, bool mutual = false, double dist = 1.0)
        {
            if (this == other)
                throw new Exception("Attempted to add node as neighbor to itself, not currently supported");

            if (neighbors.ContainsKey(other))
                return;

            Connection con = new Connection(this, other, dist);
            neighbors[other] = con;

            if (mutual)
            {
                if (other.neighbors.ContainsKey(this))
                    return;
                Connection mutCon = new Connection(other, this, dist);
                other.neighbors[this] = mutCon;
            }
        }

        public bool hasNeighbor(Node other)
        {
            return neighbors.ContainsKey(other);
        }

        public bool hasNeighbor(int id)
        {
            return neighbors.Keys.Any(node => node.getId == id);
        }

        public static bool operator ==(Node a, Node b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a is null || b is null)
                return false;

            return (a.X == b.X && a.Y == b.Y);
        }

        public override bool Equals(object obj)
        {
            return (this == (Node)obj);
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

        public override String ToString()
        {
            return $"[{X}, {Y} : {neighbors.Count}]";
        }
    }

    public class Connection
    {
        private Node A;
        private Node B;
        double length;

        public Connection(Node a, Node b, double dist = 1.0)
        {
            A = a;
            B = b;
            length = dist;
        }

        public Node getSource => A;
        public Node getDestination => B;
        public double getLength => length;
    }
}
