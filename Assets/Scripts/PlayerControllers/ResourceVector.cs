using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ResourceVector
{
    public ResourceVector(float e, float g, float p)
    {
        electricity = e;
        gas = g;
        pieces = p;
    }
    public float electricity;
    public float gas;
    public float pieces;

    public static object Deserialize(byte[] data)
    {
        ResourceVector result = new ResourceVector(data[0],data[1],data[2]);
        return result;
    }

    public static byte[] Serialize(object vector)
    {
        var c = (ResourceVector)vector;
        return new byte[] { (byte)c.electricity, (byte)c.gas, (byte)c.pieces };
    }
}
