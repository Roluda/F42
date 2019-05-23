using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    private int _maxCompletion = 5;
    private int _completion;
    public int Completion
    {
        get
        {
            return _completion;
        }
        set
        {
            _completion = Mathf.Clamp(value, 0, _maxCompletion);
        }
    }
    public int MaxCompletion
    {
        get
        {
            return _completion;
        }
        set
        {
            _completion = value;
        }
    }


    public static object Deserialize(byte[] data)
    {
        var result = new Gun();
        result.Completion = data[0];
        result.MaxCompletion = data[1];
        return result;
    }

    public static byte[] Serialize(object customType)
    {
        var c = (Gun)customType;
        return new byte[] { (byte)c.Completion,(byte)c.MaxCompletion};
    }
}
