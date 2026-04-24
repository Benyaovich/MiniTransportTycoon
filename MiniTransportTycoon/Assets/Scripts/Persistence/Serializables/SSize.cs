using System;

[Serializable]
public class SSize
{
    public int width;
    public int height;
        
    public SSize(Size size)
    {
        width = size.Width;
        height = size.Height;
    }
    
    public SSize(){}
}
