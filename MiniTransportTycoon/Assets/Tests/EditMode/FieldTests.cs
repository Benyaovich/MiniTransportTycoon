using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FieldTest
{
    [Test]
    public void FieldTestConstructor()
    {
        Field f1 = new Field(new Location(1, 2));
    }
    
    [Test]
    public void InheritanceWorks()
    {
        List<Location> poslist = new List<Location>();
        Cell c2 = new Field(new Location(0, 0));
        poslist = c2.GetGridPositionList();
        
        Assert.AreEqual(1, poslist.Count);
        
        Assert.AreEqual(0, poslist[0].X);
        Assert.AreEqual(0, poslist[0].Y);
    }
}
