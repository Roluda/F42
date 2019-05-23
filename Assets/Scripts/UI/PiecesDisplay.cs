using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PiecesDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text display = null;

    void Awake()
    {
        PlayerController.OnResourceChange += OnValueChange;
    }
    void Start()
    {
        display.text = PlayerController.Instance.Pieces.ToString();
    }

    void OnValueChange(ResourceType type, int newValue)
    {
        if (type == ResourceType.pieces)
        {
            display.text = newValue.ToString();
        }
    }
}
