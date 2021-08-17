using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodeSimulator;
using System;
using System.Collections.Generic;

namespace NodeSimulatorTests
{
    [TestClass]
    public class PriorityQueueTests
    {
        public static Random random = new Random();

        [TestMethod]
        public void PriorityQueue_Constructor()
        {
            PriorityQueue<int> queue = new PriorityQueue<int>();
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void PriorityQueue_AddRemove_Single()
        {
            PriorityQueue<int> queue = new PriorityQueue<int>();
            int num = random.Next();
            queue.Enqueue(num, 0.5);
            Assert.AreEqual(1, queue.Count);
            int output = queue.Dequeue();
            Assert.AreEqual(0, queue.Count);
            Assert.AreEqual(num, output);
        }

        [TestMethod]
        public void PriorityQueue_AddRemove_Multiple()
        {
            random = new Random(0);
            PriorityQueue<int> queue = new PriorityQueue<int>();
            int num = 20;
            List<double> priorities = randomList(num);
            List<double> sortedPriorities = new List<double>(priorities);
            sortedPriorities.Sort();
            for (int i = 0; i < num; i ++)
            {
                queue.Enqueue(i, priorities[i]);
                Assert.AreEqual(i + 1, queue.Count);
            }
            Assert.AreEqual(num, queue.Count);
            List<int> orderOut = new List<int>();
            for (int i = 0; i < num; i ++)
            {
                orderOut.Add(queue.Dequeue());
                Assert.AreEqual(sortedPriorities[i], priorities[orderOut[i]],$"Failed on {i}");
            }
            Assert.AreEqual(0, queue.Count);
        }

        private static List<double> randomList(int count)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < count; i ++)
            {
                output.Add(random.NextDouble());
            }
            return output;
        }
    }
}
