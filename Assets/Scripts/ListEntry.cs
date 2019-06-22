using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public enum ListEntryState {empty=0, electricity=1, gas=2,pieces=3}

public class ListEntry : MonoBehaviour
{
    public delegate void EntryChangeAction(ListEntry entry);
    public static event EntryChangeAction OnEntryChange;

    public ListEntryToggle[] toggles;
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
            OnEntryChange?.Invoke(this);
        }
    }

    void Start()
    {
        if (toggles.Length == 3)
        {
            toggles[0].OnClick += SetStateToElectricity;
            toggles[1].OnClick += SetStateToGas;
            toggles[2].OnClick += SetStateToPieces;
        }
    }

    public void SetStateToElectricity()
    {
        if (PhotonNetwork.IsConnected)
        {
            SupplyList.Instance.photonView.RPC("ChangeState", RpcTarget.MasterClient, position, (int)ListEntryState.electricity);
        }
    }

    public void SetStateToGas()
    {
        if (PhotonNetwork.IsConnected)
        {
            SupplyList.Instance.photonView.RPC("ChangeState", RpcTarget.MasterClient, position, (int)ListEntryState.gas);
        }
    }

    public void SetStateToPieces()
    {
        if (PhotonNetwork.IsConnected)
        {
            SupplyList.Instance.photonView.RPC("ChangeState", RpcTarget.MasterClient, position, (int)ListEntryState.pieces);
        }
    }
}
