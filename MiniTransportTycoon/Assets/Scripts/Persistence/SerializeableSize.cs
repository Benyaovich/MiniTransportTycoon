using System;

[Serializable]
public class SerializeableSize
{
    public int width;
    public int height;
        
    public SerializeableSize(Size size)
    {
        width = size.Width;
        height = size.Height;
    }
    
    public SerializeableSize(){}
}
