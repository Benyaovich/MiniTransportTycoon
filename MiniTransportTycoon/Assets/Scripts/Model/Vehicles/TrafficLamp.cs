using System;
using Model.Enumerations;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLamp : IAdvancable
{
    private Timer _switchtimerUD;
    private Timer _switchtimerLR;
    private bool _isOn;
    private bool _UDLightActive; // fentrol lefele haladas lampaja
    public bool UDLightActive => _UDLightActive;
    public bool IsLightOn => _isOn;

    public List<Direction> PassingDirection
    {
        get
        {
            if (!_isOn) return new List<Direction>(){Direction.Up, Direction.Down, Direction.Left, Direction.Right};
                
            if (_UDLightActive)    
            {
                return new List<Direction>(){Direction.Up, Direction.Down};
            }
            else
            {
                return new List<Direction>(){Direction.Left, Direction.Right};
            }
        }
    }

    public TrafficLamp()
    {
        _isOn = false;
        _UDLightActive = false;
    }

    public void SetTrafficLight(float UDLightLength = 3, float LRLightLenth = 3, bool isUDonFirst = true)
    {
        if (_switchtimerLR is not null || _switchtimerUD is not null)
        {
            _switchtimerLR.OnTimerElapsed -= LightSwitch;
            _switchtimerUD.OnTimerElapsed -= LightSwitch;
        }
        
        _switchtimerUD = new Timer(UDLightLength);
        _switchtimerLR = new Timer(LRLightLenth);

        _switchtimerUD.OnTimerElapsed += LightSwitch;
        _switchtimerLR.OnTimerElapsed += LightSwitch;
        
        _UDLightActive = isUDonFirst;
        
        _isOn = true;
    }

    public void TurnOffTrafficLight()
    {
        _isOn = false;
    }

    public void TurnOnTrafficLight()
    {
        if(_switchtimerUD is null ||  _switchtimerLR is null) throw new InvalidOperationException("The TrafficLamp hasn't been set yet.");
        
        _isOn = true;
    }

    public void LightSwitch(object sender, EventArgs e)
    {
        if(!_isOn) throw new InvalidOperationException("The TrafficLamp hasn't been set yet.");
        
        _UDLightActive = !_UDLightActive;
    }

    public void Tick(float delta)
    {
        if (_isOn)
        {
            if (_UDLightActive)
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
