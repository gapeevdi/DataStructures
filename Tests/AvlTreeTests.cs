using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Structures;
using Tests.AVLTreeTests.Extensions;

namespace Tests.AVLTreeTests
{
    [TestFixture]
    public class AVLTreeTests
    {

        [TestCase(10, 100)]
        [TestCase(-10, 100)]
        [TestCase(0, 100)]
        public void Add_TheSameValueBeingInsertedManyTimes_TreeHeightLog2Count(int value, int count)
        {
            var tree = new AVLTree<int>();
            AVLTestUtil.FillTreeWithValue(tree, value, count);
            
            Assert.AreEqual(count, tree.Count);
            AssertExtensions.AVL.IsHeightValid(count, tree.Height);
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
            var addedValues = AVLTestUtil.FillTreeInLinearOrder(tree, startWith, count, step);
            
            Assert.AreEqual(count, tree.Count);
            AssertExtensions.AVL.IsHeightValid(addedValues.Count, tree.Height);
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
            var addedValues = AVLTestUtil.FillTreeInLinearOrder(tree, startWith, count, step);
            
            Assert.AreEqual(count, tree.Count);
            AssertExtensions.AVL.IsHeightValid(addedValues.Count, tree.Height);
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
            var addedValues = AVLTestUtil.FillTreeInAlternateOrder(tree, start, end, step);
            
            Assert.AreEqual(addedValues.Count, tree.Count);
            AssertExtensions.AVL.IsHeightValid(addedValues.Count, tree.Height);
        }


        [TestCase(-1, 2, -10, 1)]
        [TestCase(-1, 3, -10, 2)]
        [TestCase(-5, 10, -10, 4)]
        [TestCase(-10, 20, -10, 10)]
        [TestCase(-50, 100, -10, 30)]
        [TestCase(-500, 1000, -10, 300)]
        [TestCase(-5000, 10000, -10, 2000)]
        [TestCase(-50000, 100000, -10, 25000)]
        [TestCase(-500000, 1000000, -10, 400000)]
        [TestCase(-1, 2, 10, 1)]
        [TestCase(-1, 3, 10, 2)]
        [TestCase(-5, 10, 10, 4)]
        [TestCase(-10, 20, 10, 10)]
        [TestCase(-50, 100, 10, 30)]
        [TestCase(-500, 1000, 10, 300)]
        [TestCase(-5000, 10000, 10, 2000)]
        [TestCase(-50000, 100000, 10, 25000)]
        [TestCase(-500000, 1000000, 10, 400000)]
        public void Remove_ParticularAmountOfNodeRemoved_TreeHeightLog2Count(int startWith, int count, int step, int numberOfElementsToRemove)
        {
            var tree = new AVLTree<int>();
            var addedValues = AVLTestUtil.FillTreeInLinearOrder(tree, startWith, count, step);

            var rnd = new Random();
            var removedIndexes = new List<int>();

            while (removedIndexes.Count < numberOfElementsToRemove)
            {
                var index = rnd.Next(0, addedValues.Count - 1);
                tree.Remove(addedValues[index]);
                removedIndexes.Add(index);
            }
            
            Assert.AreEqual(addedValues.Count - removedIndexes.Distinct().Count(), tree.Count);
            AssertExtensions.AVL.IsHeightValid(addedValues.Count - removedIndexes.Count, tree.Height);
        }

        
        [TestCase(-500, 1000, 10)]
        public void Remove_NotExistingNode_TreeHeightRemainsTheSame(int startWith, int count, int step)
        {
            var tree = new AVLTree<int>();
            AVLTestUtil.FillTreeInLinearOrder(tree, startWith, count, step);
            var notExistingValue = 10000000;
            
            tree.Remove(notExistingValue);
            
            Assert.AreEqual(count, tree.Count);
        }
    }
}