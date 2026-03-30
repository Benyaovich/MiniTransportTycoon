using System;
using JetBrains.Annotations;

[Serializable]
public class SerializableModelGridObject
{
    public SerializableLocation location;
    [CanBeNull] public SerializableCell model;
    public string modelType;
        
    public SerializableModelGridObject(ModelGridObject modelGridObject)
    {
        location = new SerializableLocation(modelGridObject.Location);
        model = new SerializableCell(modelGridObject.Model);
        modelType = modelGridObject.Model.GetType().ToString();
    }
}
