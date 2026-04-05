using System;
using UnityEngine;

public class TrafficLamp : IAdvancable
{
    private Timer _switchtimerUD;
    private Timer _switchtimerLR;
    private bool _isOn;
    private bool? _UDLight; // fentrol lefele haladas lampaja
    private bool? _LRLight; // balrol jobbra haladas lampaja
    public bool? UDLight => _UDLight;
    public bool? LRLight => _LRLight;
    public bool IsLightOn => _isOn;

    public TrafficLamp()
    {
        _isOn = false;
    }

    public void SetTrafficLight(float UDLightLength = 3, float LRLightLenth = 3, bool isUDonFirst = true)
    {
        _switchtimerUD = new Timer(UDLightLength);
        _switchtimerLR = new Timer(LRLightLenth);

        _switchtimerUD.OnTimerElapsed += LightSwitch;
        _switchtimerLR.OnTimerElapsed += LightSwitch;
        
        _UDLight = isUDonFirst;
        _LRLight = !isUDonFirst;
        
        _isOn = true;
    }

    public void LightSwitch(object sender, EventArgs e)
    {
        _UDLight = !_UDLight;
        _LRLight = !_LRLight;
    }

    public void Tick(float delta)
    {
        if (_switchtimerUD is not null && _switchtimerLR is not null)
        {
            if (_UDLight == true)
            {
                _switchtimerUD.Tick(delta);
            }
            else
            {
                _switchtimerLR.Tick(delta);
            }
        }
    }
}
