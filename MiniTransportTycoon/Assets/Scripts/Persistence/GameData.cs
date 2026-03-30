using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<SerializableModelGridObject> GridArray = new();

    public GameData(ModelGridObject[,] gridArray)
    {
        for (int y = 0; y < gridArray.GetLength(1); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                if (gridArray[x, y].Model != null)
                {
                    GridArray.Add(new SerializableModelGridObject(gridArray[x, y]));
                }
            }
        }
    }
}
