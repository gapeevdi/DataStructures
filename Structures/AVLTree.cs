using System;

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
            public int Height { get; }
        }

        public void Add(T value)
        {
            var newNode = new Node<T>(value);
            if (_root == null)
            {
                _root = newNode;
                return;
            }

            PutInPlaceNode(newNode);
            //BalanceTree(newNode);
        }

        private void PutInPlaceNode(Node<T> newNode)
        {
            var newNodeParent = _root;
            var rootElementFound = false;
            while (rootElementFound == false)
            {
                if (newNode.Value.CompareTo(newNodeParent.Value) < 0)
                {
                    if (_root.Left != null)
                    {
                        newNodeParent = _root.Left;
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
                    if (_root.Right != null)
                    {
                        newNodeParent = _root.Right;
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
    }
}