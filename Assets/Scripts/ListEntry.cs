using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ListEntryState {empty=0, electricity=1, gas=2,pieces=3}

public class ListEntry : MonoBehaviour
{
    public Toggle[] toggles;
    [SerializeField]
    ToggleGroup toggleGroup = null;
    [SerializeField]
    TMP_Text NameText = null;
    private ListEntryState _state;
    private bool _isLocked;
    public int position;

    public bool IsLocked
    {
        get
        {
            return _isLocked;
        }
        set
        {
            foreach (Toggle tog in toggles)
            {
                tog.gameObject.SetActive(!value);
            }
            _isLocked = value;
        }
    }

    public string Name
    {
        get
        {
            return NameText.text;
        }
        set
        {
            NameText.text = value;
        }
    }

    public ListEntryState State
    {
        get
        {
            return _state;
        }
        set
        {
            if(value != ListEntryState.empty)
            {
                toggleGroup.allowSwitchOff = false;
                //Changing the state to non emty also sets the toggles
                toggles[(int)value - 1].isOn = true;
            }
            else if(value ==ListEntryState.empty)
            {
                toggleGroup.allowSwitchOff = true;
                toggleGroup.SetAllTogglesOff();
            }
            Debug.Log("Set State of " + Name + " to " + value);
            _state = value;
        }
    }

    public void SetStateToElectricity(bool value)
    {
        if (value)
        {
            State = ListEntryState.electricity;
        }
    }

    public void SetStateToGas(bool value)
    {
        if (value)
        {
            State = ListEntryState.gas;
        }
    }

    public void SetStateToPieces(bool value)
    {
        if (value)
        {
            State = ListEntryState.pieces;
        }
    }
}
