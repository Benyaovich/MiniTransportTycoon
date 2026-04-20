using System;
using Persistence;

[Serializable]
public class SerializableModelGridObject
{
    public SerializableCell model;
    public string modelType;
        
    public SerializableModelGridObject(ModelGridObject modelGridObject)
    {
        if (modelGridObject.Model is Forest forest)
        {
            model = new SerializableForest(forest);
        }
        else
        {
            model = new SerializableCell(modelGridObject.Model);
        }
        modelType = modelGridObject.Model!.GetType().ToString();
    }
    
    public SerializableModelGridObject(){}
}
