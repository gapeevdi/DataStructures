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
}