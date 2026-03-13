
using System;

public interface IHighlightable
{
    public event EventHandler<Location> OnHighlightEnabled; 
    public event EventHandler<Location> OnHighlightDisabled; 
    public bool Highlighted { get; }

    public void SetHighlighted(bool value);

}
