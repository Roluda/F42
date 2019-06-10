using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PiecesDisplay : ResourceDisplay
{
    protected override void OnValueChanged(ResourceType type, float newValue)
    {
        if (type == ResourceType.pieces)
        {
            CurrentValue = newValue;
        }
    }
}
