using NUnit.Framework;
using Structures;

namespace Tests.AVLTreeTests
{
    [TestFixture]
    public class BTreeTests
    {
        [TestCase(0, 1000000)]
        public void Add(int start, int end)
        {
            var bTree = new BTree<int>(new BTreeDegree(6));

            for (var i = start; i < end; i++)
            {
                bTree.Add(i);
            }
            
            
        }
    }
}