using System;
using System.Collections.Generic;

namespace Structures
{

    public struct BTreeDegree
    {
        public int Value { get; }
        
        public BTreeDegree(int degreeValue)
        {
            if (degreeValue < 2)
            {
                throw new InvalidBTreeDegreeException();
            }

            Value = degreeValue;
        }
    }

    public class InvalidBTreeDegreeException : InvalidOperationException
    {
        public InvalidBTreeDegreeException() : base("B-tree degree can't be less than 2")
        {
            
        }
    }
    
    public class BTree<T>
    where T : IComparable<T>
    {
        private readonly BTreeDegree _degree;
        private BTreeNode<T> _root;
        
        public BTree(BTreeDegree degree)
        {
            _degree = degree;
            _root = AllocateNode();
        }

        private BTreeNode<T> AllocateNode()
        {
            return new BTreeNode<T>(_degree);
        }

        public void Add(T value)
        {
            if (_root.IsFull)
            {
                var oldRoot = _root;
                var newRoot = AllocateNode();
                _root = newRoot;
                _root.AddChild(oldRoot);

                SplitChildNode(_root, oldRoot);
            }

            AddImplementation(_root, value);
        }

        private void AddImplementation(BTreeNode<T> node, T value)
        {
            var newValueIndex = node.KeysCount - 1;
            if (node.IsLeaf)
            {
                while (value.CompareTo(node.Keys[newValueIndex]) <= 0 && newValueIndex >= 0)
                {
                    //DO NOTHING BUT DECREMENTING newValueIndex
                    newValueIndex--;
                }
                
                node.PushChildrenOneStepForward(newValueIndex + 1);
                node.InsertKey(value, newValueIndex);
                return;
            }

            while (value.CompareTo(node.Keys[newValueIndex]) <= 0 && newValueIndex >= 0)
            {
                while (value.CompareTo(node.Keys[newValueIndex]) <= 0 && newValueIndex >= 0)
                {
                    //DO NOTHING BUT DECREMENTING newValueIndex
                    newValueIndex--;
                }

                newValueIndex++;
                var childNode = node.Children[newValueIndex];
                if (childNode.IsFull)
                {
                    var middleValue = SplitChildNode(node, childNode);
                    if (value.CompareTo(middleValue) > 0)
                    {
                        newValueIndex++;
                    }
                }
                AddImplementation(node.Children[newValueIndex], value);
                
            }
        }

        private T SplitChildNode(BTreeNode<T> parent, BTreeNode<T> childToSplit)
        {
            var newNode = AllocateNode();

            childToSplit.MoveKeysToNode(newNode, _degree.Value + 1);
            childToSplit.MoveChildrenToNode(newNode, _degree.Value + 1);

            var middleValue = parent.Keys[_degree.Value];
            
           //parent.PushKeysOneStepForward(parent.GetChildIndex(childToSplit) + 1);
            parent.InsertKey(childToSplit.Keys[_degree.Value], parent.GetChildIndex(childToSplit) + 1);
            
            //parent.PushChildrenOneStepForward(parent.GetChildIndex(childToSplit) + 1);
            parent.InsertChild(newNode, parent.GetChildIndex(childToSplit) + 1);

            return middleValue;
        }
    }


    public sealed class BTreeNode<T>
    where T : IComparable<T>
    {
        private readonly BTreeDegree _degree;
        internal List<T> Keys { get; }
        internal int KeysCount => Keys.Count;
        internal List<BTreeNode<T>> Children { get; }
        internal int ChildrenCount => Children.Count;
        internal bool IsLeaf => Children.Count == 0;
        internal bool IsFull => Keys.Count == GetMaxAmountOfNodes();
        
        public BTreeNode(BTreeDegree degree)
        {
            _degree = degree;
            Keys = new List<T>();
            Children = new List<BTreeNode<T>>();
        }

        public void AddValue(T value)
        {
            if (HasFreeSpace())
            {
                Keys.Add(value);
            }
        }

        public void AddChild(BTreeNode<T> child)
        {
            Children.Add(child);
        }

        public void InsertKey(T value, int position)
        {
            Keys.Insert(position, value);
        }
        
        public void InsertChild(BTreeNode<T> child, int position)
        {
            Children.Insert(position, child);
        }

        public void PushKeysOneStepForward(int startFromIndex)
        {
            for (var i = Keys.Count - 1; i >= startFromIndex; i--)
            {
                Keys[i + 1] = Keys[i];
            }
        }

        public void PushChildrenOneStepForward(int startFromIndex)
        {
            for (var i = Children.Count - 1; i >= startFromIndex ; i++)
            {
                Children[i + 1] = Children[i];
            }
        }

        public void MoveKeysToNode(BTreeNode<T> destinationNode, int sourceStartIndex)
        {
            for (var i = sourceStartIndex; i < Keys.Count; i++)
            {
                destinationNode.AddValue(Keys[i]);
            }

            Keys.RemoveRange(_degree.Value + 1, int.MaxValue);
        }

        public void MoveChildrenToNode(BTreeNode<T> destinationNode, int sourceStartIndex)
        {
            if (IsLeaf)
            {
                for (var i = sourceStartIndex; i < Children.Count; i++)
                {
                    destinationNode.Children.Add(Children[i]);
                }    
            }
            
        }

        internal int GetChildIndex(BTreeNode<T> child) => Children.IndexOf(child);

        private int GetMaxAmountOfNodes() => 2 * _degree.Value - 1;

        private bool HasFreeSpace() => Keys.Count <= GetMaxAmountOfNodes();
    }
}