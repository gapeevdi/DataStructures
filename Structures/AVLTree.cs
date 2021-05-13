using System;
using System.Xml.Schema;

namespace Structures
{
    public class AVLTree<T>
    where T : IComparable<T>
    {

        private Node<T> _root;
        
        private class Node<T>
        {
            public Node(T value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                Value = value;
            }
            
            public T Value { get; }
            
            public Node<T> Parent { get; set; }
            
            public Node<T> Left { get; set; }
            public Node<T> Right { get; set; }
            public bool IsRoot => Parent == null;

            public void ReplaceChild(Node<T> nodeToReplace, Node<T> newNode)
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

            public int Height()
            {
                if (Left == null && Right == null)
                {
                    return 0;
                }
                var leftSubTreeHeight = Left?.Height() ?? 0;
                var rightSubTreeHeight = Right?.Height() ?? 0;
                return Math.Max(leftSubTreeHeight, rightSubTreeHeight) + 1;
            }

            public int HeightDifference(out Node<T> higherSubTree)
            {
                var leftSubTreeHeight = Left?.Height() ?? -1;
                var rightSubTreeHeight = Right?.Height() ?? -1;
                var difference = Math.Abs(leftSubTreeHeight - rightSubTreeHeight);
                
                higherSubTree = leftSubTreeHeight > rightSubTreeHeight ? Left : Right;

                return difference;
            }
        }

        public int Height => _root?.Height() ?? 0;
        public int Count { get; private set; }
        
        public void Add(T value)
        {
            var newNode = new Node<T>(value);
            Count++;
            if (_root == null)
            {
                _root = newNode;
                return;
            }

            PutInPlaceNode(newNode);
            BalanceTree(newNode);
        }

        private void PutInPlaceNode(Node<T> newNode)
        {
            var newNodeParent = _root;
            var rootElementFound = false;
            while (rootElementFound == false)
            {
                if (newNode.Value.CompareTo(newNodeParent.Value) < 0)
                {
                    if (newNodeParent.Left != null)
                    {
                        newNodeParent = newNodeParent.Left;
                    }
                    else
                    {
                        newNodeParent.Left = newNode;
                        newNode.Parent = newNodeParent;
                        rootElementFound = true;
                    }
                }
                else
                {
                    if (newNodeParent.Right != null)
                    {
                        newNodeParent = newNodeParent.Right;
                    }
                    else
                    {
                        newNodeParent.Right = newNode;
                        newNode.Parent = newNodeParent;
                        rootElementFound = true;
                    }
                }
            }
        }

        private void BalanceTree(Node<T> newNode)
        {
            var subTreeRoot = newNode.Parent;
            while (subTreeRoot != null)
            {
                if (subTreeRoot.HeightDifference(out var higherSubTree) == 2)
                {
                    if (higherSubTree == subTreeRoot.Left)
                    {
                         higherSubTree.HeightDifference(out var higher);
                         
                         if (higher == higherSubTree.Left)
                         {
                             subTreeRoot = RightRotation(higherSubTree);
                         }
                         else if (higher == higherSubTree.Right)
                         {
                             subTreeRoot = LeftRightRotation(higherSubTree.Right);
                         }
                    }
                    else if(higherSubTree == subTreeRoot.Right)
                    {
                        higherSubTree.HeightDifference(out var higher);
                        if (higher == higherSubTree.Right)
                        {
                            subTreeRoot = LeftRotation(higherSubTree);
                        }
                        else if (higher == higherSubTree.Left)
                        {
                            subTreeRoot = RightLeftRotation(higherSubTree.Left);
                        }
                    }
                }

                _root = subTreeRoot;
                subTreeRoot = subTreeRoot.Parent;
            }
        }

        private Node<T> LeftRotation(Node<T> node)
        {
            var subTreeParent = node.Parent;
            var nodeLeftSubTree = node.Left;
            var parentParent = subTreeParent.Parent;
            
            node.Parent = subTreeParent.Parent;
            node.Left = subTreeParent;
            subTreeParent.Parent = node;
            subTreeParent.Right = nodeLeftSubTree;

            parentParent?.ReplaceChild(subTreeParent, node);
            
            if (nodeLeftSubTree != null)
            {
                nodeLeftSubTree.Parent = subTreeParent;    
            }

            return node;
        }

        private Node<T> RightRotation(Node<T> node)
        {
            var subTreeParent = node.Parent;
            var nodeRightSubTree = node.Right;
            var parentParent = subTreeParent.Parent;
            
            node.Parent = subTreeParent.Parent;
            subTreeParent.Parent = node;
            
            node.Right = subTreeParent;
            subTreeParent.Left = nodeRightSubTree;

            parentParent?.ReplaceChild(subTreeParent, node);
            
            if (nodeRightSubTree != null)
            {
                nodeRightSubTree.Parent = subTreeParent;    
            }

            return node;
        }

        private Node<T> LeftRightRotation(Node<T> node)
        {
            var newSubTreeRoot = LeftRotation(node);
            return RightRotation(newSubTreeRoot);
        }

        private Node<T> RightLeftRotation(Node<T> node)
        {
            var newSubTreeRoot = RightRotation(node);
            return LeftRotation(newSubTreeRoot);
        }
    }
}