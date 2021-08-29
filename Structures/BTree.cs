using System;

namespace Structures
{
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
        
        private BTreeNode<T> AllocateNode()
        {
            return new BTreeNode<T>(_degree);
        }

        private void AddImplementation(BTreeNode<T> node, T value)
        {
            var newValueIndex = node.KeysCount - 1;
            if (node.IsLeaf)
            {
                while (newValueIndex >= 0 && value.CompareTo(node.Keys[newValueIndex]) <= 0)
                {
                    //DO NOTHING BUT DECREMENTING newValueIndex
                    newValueIndex--;
                }

                node.InsertKey(value, newValueIndex + 1);
                return;
            }

            while (newValueIndex >= 0 && value.CompareTo(node.Keys[newValueIndex]) <= 0)
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

        private T SplitChildNode(BTreeNode<T> parent, BTreeNode<T> childToSplit)
        {
            var newNode = AllocateNode();

            var position = parent.GetChildIndex(childToSplit);
            childToSplit.MoveKeysToNode(newNode, _degree.Value);
            childToSplit.MoveChildrenToNode(newNode, _degree.Value);

            var middleValue = childToSplit.Keys[_degree.Value - 1];
            
            parent.InsertKey(middleValue, position);
            childToSplit.RemoveKey(_degree.Value - 1);
            
            parent.InsertChild(newNode, position + 1);

            return middleValue;
        }
    }
}