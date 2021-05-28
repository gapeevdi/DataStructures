using System.Collections.Generic;
using Structures;

namespace Tests.AVLTreeTests
{
    public static class AVLTestUtil
    {
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
        
        public static void FillTreeWithValue(AVLTree<int> tree, int value, int count)
        {
            for (var i = 0; i < count; i++)
            {
                tree.Add(value);
            }
        }
        
        public static IReadOnlyList<int> FillTreeInAlternateOrder(AVLTree<int> tree, int startRange, int endRange, int step = 1)
        {
            var lowValue = startRange;
            var topValue = endRange;
            var count = 0;
            var addedValues = new List<int>();

            while (topValue > lowValue)
            {
                tree.Add(lowValue);
                tree.Add(topValue);
                addedValues.Add(lowValue);
                addedValues.Add(topValue);
                
                count += 2;

                lowValue += step;
                topValue -= step;
            }

            return addedValues;
        }
        
        public static IReadOnlyList<int> FillTreeInLinearOrder(AVLTree<int> tree, int startWith, int count = 1, int step = 1)
        {
            var valuesToAdd = GenerateArray(startWith, count, step);
            foreach (var value in valuesToAdd)
            {
                tree.Add(value);
            }

            return valuesToAdd;
        }
    }
}