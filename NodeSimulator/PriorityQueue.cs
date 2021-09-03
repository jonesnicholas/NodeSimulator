using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeSimulator
{
    public class PriorityQueue<TElement>
    {
        QueueNode<TElement>[] heap;
        int count;
        int countLimit;

        public int Count => count;

        public PriorityQueue()
        {
            countLimit = 16;
            heap = new QueueNode<TElement>[countLimit];
            count = 0;
        }

        public void Enqueue(TElement element, double priority)
        {
            if (count + 1 >= countLimit)
                ScaleUpArray();

            heap[count] = new QueueNode<TElement>(element, priority);
            fixHeapUp(count);
            count++;
        }

        public (TElement, double) Dequeue()
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Priority Queue contains no elements");
            }
            TElement outElement = heap[0].Element;
            double outPri = heap[0].Priority;
            swapIndex(0, Count - 1);
            heap[Count - 1] = null;
            count--;
            fixHeapDown(0);
            if (count <= countLimit / 4)
            {
                ScaleDownArray();
            }
            return (outElement, outPri);
        }

        public void fixHeapUp(int index)
        {
            if (index == 0)
                return;
            int pIndex = (index - 1) / 2;
            if (heap[index] < heap[pIndex])
            {
                swapIndex(index, pIndex);
                fixHeapUp(pIndex);
            }
        }

        public void fixHeapDown(int index)
        {
            if (index >= count )
                return;
            int lchild = 2 * index + 1;
            int rchild = 2 * index + 2;
            if (lchild < count && heap[lchild] < heap[index])
            {
                swapIndex(lchild, index);
                fixHeapDown(lchild);
            }
            if (rchild < count && heap[rchild] < heap[index])
            {
                swapIndex(rchild, index);
                fixHeapDown(rchild);
            }
        }

        public void swapIndex(int a, int b)
        {
            QueueNode<TElement> swap = heap[a];
            heap[a] = heap[b];
            heap[b] = swap;
        }

        private void ScaleUpArray()
        {
            QueueNode<TElement>[] newHeap = new QueueNode<TElement>[countLimit * 2];
            for (int i = 0; i < count; i ++)
            {
                newHeap[i] = heap[i];
            }
            heap = newHeap;
            countLimit *= 2;
        }

        private void ScaleDownArray()
        {
            QueueNode<TElement>[] newHeap = new QueueNode<TElement>[countLimit / 2];
            for (int i = 0; i < count; i++)
            {
                newHeap[i] = heap[i];
            }
            heap = newHeap;
            countLimit /= 2;
        }

        private class QueueNode<TE>
        {
            public TE Element;
            public double Priority;
            public QueueNode(TE element, double priority)
            {
                Element = element;
                Priority = priority;
            }

            #region Operator overloads
            public static bool operator ==(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority == b.Priority;
            }

            public static bool operator !=(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority != b.Priority;
            }

            public static bool operator <=(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority <= b.Priority;
            }

            public static bool operator >=(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority >= b.Priority;
            }

            public static bool operator <(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority < b.Priority;
            }

            public static bool operator >(QueueNode<TE> a, QueueNode<TE> b)
            {
                return a.Priority > b.Priority;
            }
            #endregion
        }
    }
}
