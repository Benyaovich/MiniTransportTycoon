using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TraficLampTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TraficLampSetLights()
    {
        TrafficLamp trafficLamp = new TrafficLamp();
        
        Assert.IsFalse(trafficLamp.IsLightOn);
        
        trafficLamp.SetTrafficLight();
        
        Assert.IsTrue(trafficLamp.IsLightOn);
        Assert.IsTrue(trafficLamp.UDLightActive);
        
        trafficLamp.Tick(3.1f);
        
        Assert.IsFalse(trafficLamp.UDLightActive);
        
        trafficLamp.Tick(3.1f);
        
        Assert.IsTrue(trafficLamp.UDLightActive);
    }

    [Test]
    public void TraficLampSetTrafficLightsCustomTime()
    {
        TrafficLamp trafficLamp = new TrafficLamp();
        trafficLamp.SetTrafficLight(1, 3, false);
        
        Assert.IsFalse(trafficLamp.UDLightActive);
        
        //ket tick-ben valtas
        trafficLamp.Tick(1.501f);
        Assert.IsFalse(trafficLamp.UDLightActive);
        
        trafficLamp.Tick(1.501f);
        Assert.IsTrue(trafficLamp.UDLightActive);
    }

    [Test]
    public void RoadWithTrafficLightsSetGreen()
    {
        
    }
}
