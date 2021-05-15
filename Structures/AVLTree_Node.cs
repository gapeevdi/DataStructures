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
            
            
            public Node<TValue> BalanceSubTree()
            {
                var nextNodeToConsider = this;
                var newSubTreeRoot = this;
                while (nextNodeToConsider != null)
                {
                    if (nextNodeToConsider.HeightDifference(out var higherSubTree) == 2)
                    {
                        if (higherSubTree == nextNodeToConsider.Left)
                        {
                            higherSubTree.HeightDifference(out var higher);
                         
                            if (higher == higherSubTree.Left)
                            {
                                nextNodeToConsider = nextNodeToConsider.RightRotation(); // higherSubTree
                            }
                            else if (higher == higherSubTree.Right)
                            {
                                nextNodeToConsider = higherSubTree.LeftRightRotation();
                            }
                        }
                        else if(higherSubTree == nextNodeToConsider.Right)
                        {
                            higherSubTree.HeightDifference(out var higher);
                            if (higher == higherSubTree.Right)
                            {
                                nextNodeToConsider = nextNodeToConsider.LeftRotation();
                            }
                            else if (higher == higherSubTree.Left)
                            {
                                nextNodeToConsider = higherSubTree.RightLeftRotation();
                            }
                        }
                    }

                    newSubTreeRoot = nextNodeToConsider;
                    nextNodeToConsider = nextNodeToConsider.Parent;
                }

                return newSubTreeRoot;
            }
            
            public Node<TValue> LeftRotation()
            {
            
                var newRoot = this.Right;
                var leftSubTree = newRoot.Left;

                newRoot.Parent = this.Parent;
                newRoot.Parent?.ReplaceChild(this, newRoot);

                newRoot.Left = this;
                this.Parent = newRoot;

                this.Right = leftSubTree;

                if (leftSubTree != null)
                {
                    leftSubTree.Parent = this;
                }

                return newRoot;
            }
            
            public Node<TValue> RightRotation()
            {
                var newRoot = this.Left;
                var rightSubTree = newRoot.Right;
            
                newRoot.Parent = this.Parent;
                newRoot.Parent?.ReplaceChild(this, newRoot);

                newRoot.Right = this;
                this.Parent = newRoot;

                this.Left = rightSubTree;
                if (rightSubTree != null)
                {
                    rightSubTree.Parent = this;
                }

                return newRoot;
            }
            
            public Node<TValue> LeftRightRotation()
            {
                var newSubTreeRoot = LeftRotation();
                return newSubTreeRoot.Parent.RightRotation();
            }

            public Node<TValue> RightLeftRotation()
            {
                var newSubTreeRoot = this.RightRotation();
                return newSubTreeRoot.Parent.LeftRotation();
            }
        }
    }
}