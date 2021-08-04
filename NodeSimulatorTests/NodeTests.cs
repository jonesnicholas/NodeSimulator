using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeSimulator;
using System;

namespace NodeSimulatorTests
{
    [TestClass]
    public class NodeTests
    {
        public static Random random = new Random();

        # region Creation tests
        [TestMethod]
        public void Node_Creation_Default()
        {
            Node defNode = new Node();
            Assert.AreEqual(0, defNode.getX);
            Assert.AreEqual(0, defNode.getY);
            Assert.AreEqual(0, defNode.getAdjNodes().Count);
        }

        [TestMethod]
        public void Node_Creation_Location()
        {
            int testX = random.Next(int.MinValue, int.MaxValue);
            int testY = random.Next(int.MinValue, int.MaxValue);
            Node defNode = new Node(testX, testY);
            Assert.AreEqual(testX, defNode.getX);
            Assert.AreEqual(testY, defNode.getY);
            Assert.AreEqual(0, defNode.getAdjNodes().Count);
        }

        [TestMethod]
        public void Node_Creation_LocationName()
        {
            int testX = random.Next(int.MinValue, int.MaxValue);
            int testY = random.Next(int.MinValue, int.MaxValue);
            string testName = "FigureOutRandomStrings";
            Node defNode = new Node(testX, testY, testName);
            Assert.AreEqual(testX, defNode.getX);
            Assert.AreEqual(testY, defNode.getY);
            Assert.AreEqual(testName, defNode.getName);
            Assert.AreEqual(0, defNode.getAdjNodes().Count);
        }

        [TestMethod]
        public void Node_Creation_LocationNameId()
        {
            int testX = random.Next(int.MinValue, int.MaxValue);
            int testY = random.Next(int.MinValue, int.MaxValue);
            string testName = "FigureOutRandomStrings";
            int testId = random.Next(int.MinValue, int.MaxValue);
            Node defNode = new Node(testX, testY, testName, testId);
            Assert.AreEqual(testX, defNode.getX);
            Assert.AreEqual(testY, defNode.getY);
            Assert.AreEqual(testName, defNode.getName);
            Assert.AreEqual(testId, defNode.getId);
            Assert.AreEqual(0, defNode.getAdjNodes().Count);
        }
        #endregion

        #region Add Neighbor tests
        [TestMethod]
        public void Node_AddNeighbor_SingleDirected()
        {
            Node aNode = new Node(0, 0);
            Node bNode = new Node(1, 0);
            aNode.AddNeighbor(bNode);

            Assert.AreEqual(1, aNode.getAdjNodes().Count);
            Assert.IsTrue(aNode.getAdjNodes().Contains(bNode));
            Assert.AreEqual(0, bNode.getAdjNodes().Count);
        }

        [TestMethod]
        public void Node_AddNeighbor_SingleMutual()
        {
            Node aNode = new Node(0, 0);
            Node bNode = new Node(1, 0);
            aNode.AddNeighbor(bNode, mutual:true);

            Assert.AreEqual(1, aNode.getAdjNodes().Count);
            Assert.IsTrue(aNode.getAdjNodes().Contains(bNode));
            Assert.AreEqual(1, bNode.getAdjNodes().Count);
            Assert.IsTrue(bNode.getAdjNodes().Contains(aNode));
        }

        [TestMethod]
        public void Node_AddNeighbor_SelfNeighbor()
        {
            try
            {
                Node node = new Node();
                node.AddNeighbor(node);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Attempted to add node as neighbor to itself, not currently supported");
            }
        }

        [TestMethod]
        public void Node_AddNeighbor_DuplicateNeighbor()
        {
            Node aNode = new Node(0, 0);
            Node bNode = new Node(1, 0);
            Node cNode = new Node(0, 1);

            aNode.AddNeighbor(bNode);
            aNode.AddNeighbor(bNode);

            cNode.AddNeighbor(aNode);
            aNode.AddNeighbor(cNode, mutual: true);

            Assert.AreEqual(2, aNode.getAdjNodes().Count);
            Assert.IsTrue(aNode.hasNeighbor(bNode));
            Assert.IsTrue(aNode.hasNeighbor(cNode));
            Assert.AreEqual(0, bNode.getAdjNodes().Count);
            Assert.AreEqual(1, cNode.getAdjNodes().Count);
            Assert.IsTrue(cNode.hasNeighbor(aNode));
        }

        #endregion

        [TestMethod]
        public void Node_ToString()
        {
            Node testNode = new Node(1, 2, "TestNode", 3);
            for (int i = 0; i < 3; i++)
            {
                Node adjNode = new Node(0, i + 1);
                testNode.AddNeighbor(adjNode);
            }
            string prediction = $"[1, 2 : 3]";
            Assert.AreEqual(prediction, testNode.ToString());
        }

        [TestMethod]
        public void Node_Operator_EqualsNull()
        {
            Node a = new Node();
            Node b = null;
            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
        }

        [TestMethod]
        public void Node_Operator_EqualsAndNotEquals()
        {
            Node a = new Node(1, 1);
            Node b = new Node(1, 1);
            Node c = new Node();

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
            Assert.IsFalse(a != b);
            Assert.IsFalse(b != a);
            Assert.IsFalse(a == c);
            Assert.IsFalse(c == a);
            Assert.IsTrue(a != c);
            Assert.IsTrue(c != a);
        }
    }
}
