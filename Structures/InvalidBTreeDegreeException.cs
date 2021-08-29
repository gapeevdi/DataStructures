using System;

namespace Structures
{
    public class InvalidBTreeDegreeException : ArgumentException
    {
        public InvalidBTreeDegreeException() : base("B-tree degree can't be less than 2")
        {
            
        }
    }
}