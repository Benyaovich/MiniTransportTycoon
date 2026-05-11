
using System;
using UnityEngine;

public interface IHighlightable
{
    public event EventHandler<Location> OnHighlightEnabled; 
    public event EventHandler<Location> OnHighlightDisabled; 
    public bool Highlighted { get; }
    public Color HighlightColor { get; }

    public void SetHighlighted(bool value);
    public void SetHighlightColor(Color color);

}
