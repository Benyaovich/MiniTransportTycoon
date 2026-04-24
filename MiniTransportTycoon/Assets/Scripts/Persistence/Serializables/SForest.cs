using System;

[Serializable]
public class SForest : SCell
{
    public int numOfTrees;
    public SForest(Forest forest) : base(forest)
    {
        numOfTrees = forest.NumOfTrees;
    }

    public SForest()
    {
        
    }
}
