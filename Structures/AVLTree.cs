using System;

namespace Structures
{
    public partial class AVLTree<T>
    where T : IComparable<T>
    {

        private Node<T> _root;

        public int Height => _root?.Height() ?? 0;
        public int Count { get; private set; }
        public bool Contains(T value) => Find(value) != null;
        
        public void Add(T value)
        {
            var newNode = new Node<T>(value);
           
            if (_root == null)
            {
                _root = newNode.BalanceSubTree();
            }
            else
            {
                var subTreeRoot = PutInPlaceNode(newNode);    
                _root = subTreeRoot.BalanceSubTree();
            }
            
            Count++;
        }

        public void Remove(T value)
        {
            var nodeToRemove = Find(value);
            var subTreeToBalance = RemoveImp(nodeToRemove);
            _root = subTreeToBalance?.BalanceSubTree();
            
            Count--;
        }
        
        
        private Node<T> RemoveImp(Node<T> nodeToRemove)
        {
            var parent = nodeToRemove?.Parent;
            Node<T> subTreeToBalance = null; 
            if (nodeToRemove != null)
            {
                
                // case 1 - leaf
                if (nodeToRemove.IsLeaf)
                {
                    subTreeToBalance = parent;
                    parent?.RemoveChild(nodeToRemove);
                }
                
                // case 2 - right child exists and left is null
                else if (nodeToRemove.Right != null && nodeToRemove.Left == null)
                {
                    subTreeToBalance = nodeToRemove.Right; 
                    parent?.ReplaceChild(nodeToRemove, subTreeToBalance);
                    subTreeToBalance.Parent = parent;
                }

                // case 3 - left child exists so looking for the max value from the subtree
                // in this case we need to replace a value of nodeToRemove node with the max value
                // from the left subtree
                else
                {
                    subTreeToBalance = FindMaxNode(nodeToRemove.Left);
                    nodeToRemove.Value = subTreeToBalance.Value;
                    subTreeToBalance.Parent?.ReplaceChild(subTreeToBalance, subTreeToBalance.Left);
                    if (subTreeToBalance.Left != null)
                    {
                        subTreeToBalance.Left.Parent = subTreeToBalance.Parent;
                    } 
                    subTreeToBalance = nodeToRemove;
                }
                
                if (nodeToRemove.IsRoot)
                {
                    _root = subTreeToBalance;
                }
            }
            
            return subTreeToBalance;
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

    }
}