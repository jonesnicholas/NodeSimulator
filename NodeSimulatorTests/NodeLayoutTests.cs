using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeSimulator;
using System;
using System.Linq;

namespace NodeSimulatorTests
{
    [TestClass]
    public class NodeLayoutTests
    {
        public static Random random = new Random();

        [TestMethod]
        public void Simulation_Scenario_CreateSimpleGrid()
        {
            NodeLayout layout = new NodeLayout();
            int size = 4;
            layout.createSimpleNodeGrid(size);
            Assert.AreEqual(Math.Pow(size, 2), layout.nodes.Values.Count);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Node testNode = layout.nodes[new Tuple<int, int>(i, j)];
                    Node otherNode;
                    if (i != 0)
                    {
                        otherNode = layout.nodes[new Tuple<int, int>(i - 1, j)];
                        Assert.IsTrue(testNode.hasNeighbor(otherNode));
                    }
                    if (i != size - 1)
                    {
                        otherNode = layout.nodes[new Tuple<int, int>(i + 1, j)];
                        Assert.IsTrue(testNode.hasNeighbor(otherNode));
                    }
                    if (j != 0)
                    {
                        otherNode = layout.nodes[new Tuple<int, int>(i, j - 1)];
                        Assert.IsTrue(testNode.hasNeighbor(otherNode));
                    }
                    if (j != size - 1)
                    {
                        otherNode = layout.nodes[new Tuple<int, int>(i, j + 1)];
                        Assert.IsTrue(testNode.hasNeighbor(otherNode));
                    }
                }
            }
        }

        [TestMethod]
        public void Simulation_IO()
        {
            NodeLayout layout = new NodeLayout();
            layout.createSimpleNodeGrid(5);
            layout.outputToFile("SimpleGrid5");

            NodeLayout readLayout = new NodeLayout();
            readLayout.inputFromFile("SimpleGrid5");

            Assert.AreEqual(layout.nodes.Count, readLayout.nodes.Count);
            foreach(int id in layout.idLookup.Keys)
            {
                Node node = layout.idLookup[id];
                Node readNode = readLayout.idLookup[id];
                Assert.AreEqual(node, readNode);
                Assert.AreEqual(node.getAdjNodes().Count, readNode.getAdjNodes().Count);
                foreach(Node neighbor in node.getAdjNodes())
                {
                    Assert.IsTrue(readNode.hasNeighbor(neighbor.getId));
                }
            }
        }
    }
}
