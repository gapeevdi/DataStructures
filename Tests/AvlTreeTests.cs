using System;
using NUnit.Framework;
using Structures;

namespace Tests
{
    [TestFixture]
    public class AVLTreeTests
    {

        private static bool IsExpectedTreeHeight(int nodeCount, int actualHeight) =>
            actualHeight >= Math.Floor(Math.Log2(nodeCount))
            && actualHeight <= Math.Floor(1.45 * Math.Log2(nodeCount + 2));
        
        private static int[] GenerateArray(int startWith, int count = 1, int step = 1)
        {
            var array = new int[count];
            array[0] = startWith;
            
            for (var i = 1; i < count; i++)
            {
                array[i] = startWith + i * step;
            }

            return array;
        }

        private static int FillTreeInLinearOrder(AVLTree<int> tree, int startWith, int count = 1, int step = 1)
        {
            var valuesToAdd = GenerateArray(startWith, count, step);
            var addedValuesCount = 0;
            foreach (var value in valuesToAdd)
            {
                tree.Add(value);
                addedValuesCount++;
            }

            return addedValuesCount;
        }

        private static int FillTreeInAlternateOrder(AVLTree<int> tree, int startRange, int endRange, int step = 1)
        {
            var lowValue = startRange;
            var topValue = endRange;
            var count = 0;

            while (topValue > lowValue)
            {
                tree.Add(lowValue);
                tree.Add(topValue);
                count += 2;

                lowValue += step;
                topValue -= step;
            }

            return count;
        }
        
        [TestCase(0, 1, 10)]
        [TestCase(-1, 2, 10)]
        [TestCase(-1, 3, 10)]
        [TestCase(-5, 10, 10)]
        [TestCase(-10, 20, 10)]
        [TestCase(-50, 100, 10)]
        [TestCase(-500, 1000, 10)]
        [TestCase(-5000, 10000, 10)]
        [TestCase(-50000, 100000, 10)]
        [TestCase(-500000, 1000001, 10)]
        public void Add_EachItemGreaterThanPrevious_TreeHeightLog2Count(int startWith, int count, int step)
        {
            var tree = new AVLTree<int>();
            var addedValuesCount = FillTreeInLinearOrder(tree, startWith, count, step);
            
            Assert.AreEqual(count, tree.Count);
            Assert.IsTrue(IsExpectedTreeHeight(addedValuesCount, tree.Height));
        }

        [TestCase(0, 1, -10)]
        [TestCase(-1, 2, -10)]
        [TestCase(-1, 3, -10)]
        [TestCase(-5, 10, -10)]
        [TestCase(-10, 20, -10)]
        [TestCase(-50, 100, -10)]
        [TestCase(-500, 1000, -10)]
        [TestCase(-5000, 10000, -10)]
        [TestCase(-50000, 100000, -10)]
        [TestCase(-500000, 1000000, -10)]
        public void Add_EachItemLessThanPrevious_TreeHeightLog2Count(int startWith, int count, int step)
        {
            var tree = new AVLTree<int>();
            var addedValuesCount = FillTreeInLinearOrder(tree, startWith, count, step);
            
            Assert.AreEqual(count, tree.Count);
            Assert.IsTrue(IsExpectedTreeHeight(addedValuesCount, tree.Height));
        }

        [TestCase(-10, 10, 1)]
        [TestCase(-100, 100, 5)]
        [TestCase(-1000, 1000, 10)]
        [TestCase(-10000, 10000, 3)]
        [TestCase(-10000, 10000, 3)]
        [TestCase(-50000, 50000, 7)]
        [TestCase(-500000, 500000, 11)]
        public void Add_AlternatingOrder_TreeHeightLog2Count(int start, int end, int step)
        {
            var tree = new AVLTree<int>();
            var addedValuesCount = FillTreeInAlternateOrder(tree, start, end, step);
            
            Assert.AreEqual(addedValuesCount, tree.Count);
            Assert.IsTrue(IsExpectedTreeHeight(addedValuesCount, tree.Height));
        }

        [Test]
        public void AddTest_1()
        {
            var tree = new AVLTree<int>();
            
            tree.Add(10);
            tree.Add(20);
            tree.Add(30);
            tree.Add(40);
            tree.Add(350);
            tree.Add(50);

            var height = tree.Height;
            Assert.AreEqual(tree.Height, Math.Ceiling(Math.Log2(tree.Count)));
        }
        
        [Test]
        public void AddTest_2()
        {
            var tree = new AVLTree<int>();
            
            tree.Add(100);
            tree.Add(500);
            tree.Add(300);
            tree.Add(400);
            tree.Add(200);
            tree.Add(150);

            var height = tree.Height;
            Assert.AreEqual(tree.Height, Math.Ceiling(Math.Log2(tree.Count)));
        }

        [Test]
        public void Remove()
        {
            var tree = new AVLTree<int>();
            
            tree.Add(100);
            tree.Add(500);
            tree.Add(300);
            tree.Add(400);
            tree.Add(200);
            tree.Add(150);
            tree.Add(450);
            tree.Add(350);
            tree.Add(360);
            tree.Add(370);
            tree.Add(380);
            tree.Add(390);
            
            tree.Remove(390);
            tree.Remove(380);
            tree.Remove(370);
            tree.Remove(360);
            tree.Remove(350);
            tree.Remove(450);
            tree.Remove(150);
            tree.Remove(200);
            tree.Remove(400);
            tree.Remove(300);
            tree.Remove(500);
            tree.Remove(100);
            
            tree.Add(100);
            tree.Add(500);
            tree.Add(300);
            tree.Add(400);
            tree.Add(200);
            tree.Add(150);
            tree.Add(450);
            tree.Add(350);
            tree.Add(360);
            tree.Add(370);
            tree.Add(380);
            tree.Add(390);
            
            tree.Remove(390);
            tree.Remove(380);
            tree.Remove(370);
            tree.Remove(360);
            tree.Remove(350);
            tree.Remove(450);
            tree.Remove(150);
            tree.Remove(200);
            tree.Remove(400);
            tree.Remove(300);
            tree.Remove(500);
            tree.Remove(100);
        }
        
        [Test]
        public void Remove_1()
        {
            var tree = new AVLTree<int>();
            
            tree.Add(200);
            tree.Add(100);
            tree.Add(300);
            tree.Add(400);
            
            tree.Remove(100);
            
        }
    }
}