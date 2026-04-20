using System;

namespace Persistence
{
    [Serializable]
    public class SerializableForest : SerializableCell
    {
        public int numOfTrees;
        public SerializableForest(Forest forest) : base(forest)
        {
            numOfTrees = forest.NumOfTrees;
        }
        
        public SerializableForest() {}
    }
}