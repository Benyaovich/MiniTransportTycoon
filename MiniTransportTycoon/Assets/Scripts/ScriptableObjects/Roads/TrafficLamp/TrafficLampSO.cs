using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TrafficLampSO", menuName = "Cell/Roads/TrafficLamp/TrafficLampSO")]
public class TrafficLampSO : CellObjectTypeSO
{
    public override Cell Create(Location location)
    {
        return new TrafficLamp(location);
    }

    public override Type CellType => typeof(TrafficLamp);
}
