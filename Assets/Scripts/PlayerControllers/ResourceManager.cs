using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum ResourceType { electricity, gas, pieces }
public class ResourceManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static ResourceManager Instance = null;

    public delegate void ResourceAction(ResourceType rt, float value);
    public static event ResourceAction OnResourceChange;
    [SerializeField]
    public float resourceCap = 100;
    [SerializeField]
    ResourceVector initialResources = new ResourceVector(20, 20, 20);
    [SerializeField]
    ResourceVector resourceGainOnSupply = new ResourceVector(20, 20, 20);
    [SerializeField]
    float resupplyInterval = 30;
    public float timeTillSupply;

    protected float _electricity;
    protected float _gas;
    protected float _pieces;

    public float Electricity
    {
        get
        {
            return _electricity;
        }
        set
        {
            value = Mathf.Clamp(value, 0, resourceCap);
            if (value != _electricity)
            {
                OnResourceChange?.Invoke(ResourceType.electricity, value);
                _electricity = value;
            }
        }
    }
    public float Gas
    {
        get
        {
            return _gas;
        }
        set
        {
            value = Mathf.Clamp(value, 0, resourceCap);
            if (value != _gas)
            {
                OnResourceChange?.Invoke(ResourceType.gas, value);
                _gas = value;
            }
        }
    }
    public float Pieces
    {
        get
        {
            return _pieces;
        }
        set
        {
            value = Mathf.Clamp(value, 0, resourceCap);
            if (value != _pieces)
            {
                OnResourceChange?.Invoke(ResourceType.pieces, value);
                _pieces = value;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    void Start()
    {
        AddResources(initialResources);
        timeTillSupply = resupplyInterval;
    }

    void Update()
    {
        timeTillSupply -= Time.deltaTime;
        if (timeTillSupply <= 0)
        {
            timeTillSupply = resupplyInterval;
            if (PhotonNetwork.IsMasterClient)
            {
                SendSupplyToPlayers();
            }
        }
        SupplyList.Instance.UpdateCountdown(timeTillSupply);
    }

    public bool CheckSufficieny(ResourceVector value)
    {
        bool isSufficient = value.electricity <= Instance.Electricity && value.gas <= Instance.Gas && value.pieces <= Instance.Pieces;
        Debug.Log("sufficient Resources = " + isSufficient);
        return isSufficient;
    }

    [PunRPC]
    public void RemoveResources(ResourceVector value)
    {
        Instance.Electricity -= value.electricity;
        Instance.Gas -= value.gas;
        Instance.Pieces -= value.pieces;
    }

    public void SendSupplyToPlayers()
    {
        foreach (ListEntry entry in SupplyList.Instance.listEntries)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if ((int)player.CustomProperties["Position"] == entry.position)
                {
                    switch (entry.State)
                    {
                        case ListEntryState.electricity:
                            photonView.RPC("AddElectricity", player, resourceGainOnSupply.electricity);
                            break;
                        case ListEntryState.gas:
                            photonView.RPC("AddGas", player, resourceGainOnSupply.gas);
                            break;
                        case ListEntryState.pieces:
                            photonView.RPC("AddPieces", player, resourceGainOnSupply.pieces);
                            break;
                    }
                }
            }
        }
    }

    [PunRPC]
    public void AddResources(ResourceVector value)
    {
        Instance.Electricity += value.electricity;
        Instance.Gas += value.gas;
        Instance.Pieces += value.pieces;
    }

    [PunRPC]
    public void AddElectricity(float value)
    {
        Electricity += value;
    }
    [PunRPC]
    public void AddGas(float value)
    {
        Gas += value;
    }
    [PunRPC]
    public void AddPieces(float value)
    {
        Pieces += value;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
