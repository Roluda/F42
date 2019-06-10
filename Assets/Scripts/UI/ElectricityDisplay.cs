using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectricityDisplay : ResourceDisplay
{
    protected override void OnValueChanged(ResourceType type, float newValue)
    {
        if (type == ResourceType.electricity)
        {
            CurrentValue = newValue;
        }
    }
}
