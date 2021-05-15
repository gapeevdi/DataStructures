using System;
using System.Collections.Generic;

namespace Structures
{
    public class AVLTree<T>
    where T : IComparable<T>
    {

        private Node<T> _root;
        
        private class Node<T>
        where T : IComparable<T>
        {
            public Node(T value)
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                Value = value;
            }
            
            public T Value { get; set; }
            
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
           
            if (_root == null)
            {
                _root = newNode;
                BalanceTree(_root);
            }
            else
            {
                var subTreeRoot = PutInPlaceNode(newNode);    
                BalanceTree(subTreeRoot);
            }
            
            Count++;
        }

        public void Remove(T value)
        {
            var nodeToRemove = Find(value);
            var substituteNode = RemoveImp(nodeToRemove);
            
            if (substituteNode != null)
            {
                BalanceTree(substituteNode);
            }

            Count--;
        }
        
        
        private Node<T> RemoveImp(Node<T> nodeToRemove)
        {
            var parent = nodeToRemove?.Parent;
            Node<T> newSubTreeRoot = null; 
            if (nodeToRemove != null)
            {
                
                // case 1 - leaf
                if (nodeToRemove.Left == null && nodeToRemove.Right == null)
                {
                    newSubTreeRoot = parent;
                    parent?.RemoveChild(nodeToRemove);
                }
                
                // case 2 - right child exists and left is null
                else if (nodeToRemove.Right != null && nodeToRemove.Left == null)
                {
                    newSubTreeRoot = nodeToRemove.Right; 
                    parent?.ReplaceChild(nodeToRemove, newSubTreeRoot);
                    newSubTreeRoot.Parent = parent;
                }

                // case 3 - left child exists so looking for the max value from the subtree
                // in this case we need to replace a value of nodeToRemove node with the max value
                // from the left subtree
                else
                {
                    newSubTreeRoot = FindMaxNode(nodeToRemove.Left);
                    nodeToRemove.Value = newSubTreeRoot.Value;
                    newSubTreeRoot.Parent?.ReplaceChild(newSubTreeRoot, newSubTreeRoot.Left);
                    if (newSubTreeRoot.Left != null)
                    {
                        newSubTreeRoot.Left.Parent = newSubTreeRoot.Parent;
                    } 
                    newSubTreeRoot = nodeToRemove;
                }
                
                if (nodeToRemove.IsRoot)
                {
                    _root = newSubTreeRoot;
                }
            }
            
            return newSubTreeRoot;
        }

        private Node<T> FindMaxNode(Node<T> subTreeRoot)
        {
            var maxNode = subTreeRoot;
            while (maxNode.Right != null)
            {
                maxNode = maxNode.Right;
            }

            return maxNode;
        }


        public bool Contains(T value) => Find(value) != null;
        
        private Node<T> Find(T value)
        {
            var currentNode = _root;
            Node<T> foundNode = null;

            while (currentNode != null && foundNode == null)
            {
                if (value.CompareTo(currentNode.Value) == 0)
                {
                    foundNode = currentNode;
                }
                else if (value.CompareTo(currentNode.Value) < 0)
                {
                    currentNode = currentNode.Left;
                }
                else
                {
                    currentNode = currentNode.Right;
                }
            }

            return foundNode;
        }

        private Node<T> PutInPlaceNode(Node<T> newNode)
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

            return newNodeParent;
        }

        private void BalanceTree(Node<T> node)
        {
            //_root.Parent is null
            var subTreeRoot = node;
            while (subTreeRoot != null)
            {
                if (subTreeRoot.HeightDifference(out var higherSubTree) == 2)
                {
                    if (higherSubTree == subTreeRoot.Left)
                    {
                         higherSubTree.HeightDifference(out var higher);
                         
                         if (higher == higherSubTree.Left)
                         {
                             subTreeRoot = RightRotation(subTreeRoot); // higherSubTree
                         }
                         else if (higher == higherSubTree.Right)
                         {
                             subTreeRoot = LeftRightRotation(higherSubTree);
                         }
                    }
                    else if(higherSubTree == subTreeRoot.Right)
                    {
                        higherSubTree.HeightDifference(out var higher);
                        if (higher == higherSubTree.Right)
                        {
                            subTreeRoot = LeftRotation(subTreeRoot);
                        }
                        else if (higher == higherSubTree.Left)
                        {
                            subTreeRoot = RightLeftRotation(higherSubTree);
                        }
                    }
                }

                _root = subTreeRoot;
                subTreeRoot = subTreeRoot.Parent;
            }
        }

        private Node<T> LeftRotation(Node<T> node)
        {
            
            var newRoot = node.Right;
            var leftSubTree = newRoot.Left;

            newRoot.Parent = node.Parent;
            newRoot.Parent?.ReplaceChild(node, newRoot);

            newRoot.Left = node;
            node.Parent = newRoot;

            node.Right = leftSubTree;

            if (leftSubTree != null)
            {
                leftSubTree.Parent = node;
            }

            return newRoot;
        }

        private Node<T> RightRotation(Node<T> node)
        {
            var newRoot = node.Left;
            var rightSubTree = newRoot.Right;
            
            newRoot.Parent = node.Parent;
            newRoot.Parent?.ReplaceChild(node, newRoot);

            newRoot.Right = node;
            node.Parent = newRoot;

            node.Left = rightSubTree;
            if (rightSubTree != null)
            {
                rightSubTree.Parent = node;
            }

            return newRoot;
        }

        private Node<T> LeftRightRotation(Node<T> node)
        {
            var newSubTreeRoot = LeftRotation(node);
            return RightRotation(newSubTreeRoot.Parent);
        }

        private Node<T> RightLeftRotation(Node<T> node)
        {
            var newSubTreeRoot = RightRotation(node);
            return LeftRotation(newSubTreeRoot.Parent);
        }
    }
}