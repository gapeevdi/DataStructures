using System;
using System.Collections.Generic;

namespace Structures
{
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

        public void MoveKeysToNode(BTreeNode<T> destinationNode, int sourceStartIndex)
        {
            for (var i = sourceStartIndex; i < Keys.Count; i++)
            {
                destinationNode.AddValue(Keys[i]);
            }

            Keys.RemoveRange(sourceStartIndex, Keys.Count - sourceStartIndex);
        }

        public void RemoveKey(int position)
        {
            if (position > 0 && position < Keys.Count)
            {
                Keys.RemoveAt(position);    
            }
        }

        public void MoveChildrenToNode(BTreeNode<T> destinationNode, int sourceStartIndex)
        {
            if (IsLeaf == false)
            {
                for (var i = sourceStartIndex; i < Children.Count; i++)
                {
                    destinationNode.Children.Add(Children[i]);
                }

                Children.RemoveRange(sourceStartIndex, Children.Count - sourceStartIndex);
            }
        }

        internal int GetChildIndex(BTreeNode<T> child) => Children.IndexOf(child);

        private int GetMaxAmountOfNodes() => 2 * _degree.Value - 1;

        private bool HasFreeSpace() => Keys.Count <= GetMaxAmountOfNodes();
    }
}