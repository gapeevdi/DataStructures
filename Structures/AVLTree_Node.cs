using System;

namespace Structures
{
    public partial class AVLTree<T>
    {
        private class Node<TValue>
        {
            
            public int Height { get; private set; }

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

            public void ComputeHeight()
            {
                if (IsLeaf)
                {
                    Height = 0;
                }
                var leftSubTreeHeight = Left?.Height ?? 0;
                var rightSubTreeHeight = Right?.Height ?? 0;
                Height = Math.Max(leftSubTreeHeight, rightSubTreeHeight) + 1;
            }

            internal int HeightDifference(out Node<TValue> higherSubTree)
            {
                var leftSubTreeHeight = Left?.Height ?? -1;
                var rightSubTreeHeight = Right?.Height ?? -1;
                var difference = Math.Abs(leftSubTreeHeight - rightSubTreeHeight);
                
                higherSubTree = leftSubTreeHeight > rightSubTreeHeight ? Left : Right;

                return difference;
            }
            
            public Node<TValue> LeftRotation()
            {
            
                var newRoot = Right;
                var leftSubTree = newRoot.Left;

                newRoot.Parent = Parent;
                newRoot.Parent?.ReplaceChild(this, newRoot);

                newRoot.Left = this;
                Parent = newRoot;
                Right = leftSubTree;

                if (leftSubTree != null)
                {
                    leftSubTree.Parent = this;
                }

                newRoot.Left.ComputeHeight();
                newRoot.ComputeHeight();
                newRoot.Parent?.ComputeHeight();
                
                return newRoot;
            }
            
            public Node<TValue> RightRotation()
            {
                var newRoot = Left;
                var rightSubTree = newRoot.Right;
            
                newRoot.Parent = Parent;
                newRoot.Parent?.ReplaceChild(this, newRoot);

                newRoot.Right = this;
                Parent = newRoot;

                Left = rightSubTree;
                if (rightSubTree != null)
                {
                    rightSubTree.Parent = this;
                }

                newRoot.Right.ComputeHeight();
                newRoot.ComputeHeight();
                newRoot.Parent?.ComputeHeight();
                
                return newRoot;
            }
            
            public Node<TValue> LeftRightRotation()
            {
                var newSubTreeRoot = LeftRotation();
                return newSubTreeRoot.Parent.RightRotation();
            }

            public Node<TValue> RightLeftRotation()
            {
                var newSubTreeRoot = RightRotation();
                return newSubTreeRoot.Parent.LeftRotation();
            }
            
            public Node<TValue> FindMaxNode()
            {
                var maxNode = this;
                while (maxNode.Right != null)
                {
                    maxNode = maxNode.Right;
                }

                return maxNode;
            }
        }
    }
}