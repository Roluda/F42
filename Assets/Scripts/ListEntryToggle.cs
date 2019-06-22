using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListEntryToggle : Toggle
{
    public delegate void ClickActionHandler();
    public event ClickActionHandler OnClick;


    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (!isOn)
        {
            Debug.Log("ClickOnToggle");
            OnClick?.Invoke();
        }
    }
}
