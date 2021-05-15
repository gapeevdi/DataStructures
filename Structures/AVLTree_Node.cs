using System;

namespace Structures
{
    public partial class AVLTree<T>
    {
        private class Node<TValue>
        {
            public Node(TValue value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                Value = value;
            }
            
            public TValue Value { get; set; }
            public Node<TValue> Parent { get; set; }
            public Node<TValue> Left { get; set; }
            public Node<TValue> Right { get; set; }
            public bool IsRoot => Parent == null;
            public bool IsLeaf => Left == null && Right == null;

            public void ReplaceChild(Node<TValue> nodeToReplace, Node<TValue> newNode)
            {
                if (ReferenceEquals(Left, nodeToReplace))
                {
                    Left = newNode;
                }
                else if (ReferenceEquals(Right, nodeToReplace))
                {
                    Right = newNode;
                }
            }

            public void RemoveChild(Node<T> nodeToRemove)
            {
                if (ReferenceEquals(Left, nodeToRemove))
                {
                    Left = null;
                }
                else if (ReferenceEquals(Right, nodeToRemove))
                {
                    Right = null;
                }
            }

            public int Height()
            {
                if (IsLeaf)
                {
                    return 0;
                }
                var leftSubTreeHeight = Left?.Height() ?? 0;
                var rightSubTreeHeight = Right?.Height() ?? 0;
                return Math.Max(leftSubTreeHeight, rightSubTreeHeight) + 1;
            }

            public int HeightDifference(out Node<TValue> higherSubTree)
            {
                var leftSubTreeHeight = Left?.Height() ?? -1;
                var rightSubTreeHeight = Right?.Height() ?? -1;
                var difference = Math.Abs(leftSubTreeHeight - rightSubTreeHeight);
                
                higherSubTree = leftSubTreeHeight > rightSubTreeHeight ? Left : Right;

                return difference;
            }
        }
    }
}