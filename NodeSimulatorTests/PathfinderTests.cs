using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeSimulator;
using System.Collections.Generic;

namespace NodeSimulatorTests
{
    [TestClass]
    public class PathfinderTests
    {
        [TestMethod]
        public void Pathfinder_Dijkstras_SimplePremade()
        {
            NodeLayout layout = SimplePremadeLayout();
            Node start = layout.idLookup[1];
            Node end = layout.idLookup[2];
            List<(Node, double)> path = Pathfinder.DijkstraPath(layout, start, end);
            Assert.AreEqual(8, path.Count);
            Assert.AreEqual(28, path[0].Item2);
        }

        [TestMethod]
        public void Pathfinder_Exhaustive_SimplePremade()
        {
            NodeLayout layout = SimplePremadeLayout();
            Node start = layout.idLookup[1];
            Node end = layout.idLookup[2];
            List<(Node, double)> path = Pathfinder.ExhaustivePath(layout, start, end);
            Assert.AreEqual(8, path.Count);
            Assert.AreEqual(28, path[0].Item2);
        }

        private NodeLayout SimplePremadeLayout()
        {
            NodeLayout layout = new NodeLayout();
            Node n00 = new Node(0, 0); layout.addNode(n00);
            Node n01 = new Node(0, 1); n01.AddNeighbor(n00, mutual: true, 100); layout.addNode(n01);
            Node n02 = new Node(0, 2); n02.AddNeighbor(n01, mutual: true, 10); layout.addNode(n02);
            Node n10 = new Node(1, 0); n10.AddNeighbor(n00, mutual: true, 1); layout.addNode(n10);
            Node n11 = new Node(1, 1); n11.AddNeighbor(n10, mutual: true, 3); n11.AddNeighbor(n01, mutual: true, 30); layout.addNode(n11);
            Node n12 = new Node(1, 2); n12.AddNeighbor(n11, mutual: true, 7); n12.AddNeighbor(n02, mutual: true, 8); layout.addNode(n12);
            Node n20 = new Node(2, 0); n20.AddNeighbor(n10, mutual: true, 5); layout.addNode(n20);
            Node n21 = new Node(2, 1); n21.AddNeighbor(n20, mutual: true, 7); n21.AddNeighbor(n11, mutual: true, 2); layout.addNode(n21);
            Node n22 = new Node(2, 2); n22.AddNeighbor(n21, mutual: true, 3); n22.AddNeighbor(n12, mutual: true, 1); layout.addNode(n22);
            return layout;
        }
    }
}
