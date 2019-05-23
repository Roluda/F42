using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum ResourceType { electricity, gas, pieces}
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerController Instance;
    [Tooltip("whether the player is a rebel, change in edior only affects offline debug")]
    public bool isRebel;
    [Tooltip("The position of the Player in the Assembly Line, change in editor only affects offline debug")]
    public int position;
    [Tooltip("the amount of players in the room, change in editor only affects offline debug")]
    [SerializeField]
    int assemblyPlayers;
    [SerializeField]
    int initialGuns = 0;
    [SerializeField]
    int resourceCap = 100;

    public delegate void ResourceAction(ResourceType rt, int value);
    public static event ResourceAction OnResourceChange;
    public delegate void StartAssembleAction();
    public static event StartAssembleAction OnAssembleStartet;
    public delegate void FinishAssembleAction();
    public static event FinishAssembleAction OnAssembleFinished;
    public delegate void GunReceivedAction(Gun gun);
    public static event GunReceivedAction OnGunReceived;

    public int Electricity
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
    public int Gas
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
    public int Pieces
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

    public List<Gun> workLoad = new List<Gun>();
    public Gun currentGun;

    protected int _electricity;
    protected int _gas;
    protected int _pieces;

    protected const string addGunRpcName = "AddGunToWorkload";
    protected const string addElectricityRpcName = "AddElectricity";
    protected const string addGasRpcName = "AddGas";
    protected const string addPiecesRpcName = "AddPieces";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) {
            isRebel = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsRebel"];
            position = (int)PhotonNetwork.LocalPlayer.CustomProperties["Position"];
            assemblyPlayers = PhotonNetwork.PlayerList.Length - 1;
        }
        for (int i = 0; i < initialGuns; i++)
        {
            Gun newGun = new Gun();
            newGun.MaxCompletion = assemblyPlayers;
            newGun.Completion = position - 1;
            workLoad.Add(newGun);
        }
        CustomSetup();
    }

    public virtual void CustomSetup()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    /// <summary>
    /// call this method to get to upmost Gun from the remaining workload
    /// </summary>
    public void StartAssembly()
    {
        if (currentGun == null&& workLoad.Count>0)
        {
            Debug.Log("Started Assembling");
            currentGun = workLoad[workLoad.Count-1];
            workLoad.Remove(currentGun);
            OnAssembleStartet?.Invoke();
        }
        else
        {
            Debug.Log("Error: Gun under construction or workload is empty");
        }
    }

    /// <summary>
    /// call this method when a player attached his part to the current gun
    /// </summary>
    public void FinishAssembly()
    {
        if (currentGun != null)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if ((int)player.CustomProperties["Position"] - position == 1)
                {
                    Debug.Log("sent Gun to " + player.NickName);
                    photonView.RPC(addGunRpcName, player, (object)currentGun);
                    currentGun = null;
                    OnAssembleFinished?.Invoke();
                }
            }
        }
        else
        {
            Debug.Log("Start Assembling First");
        }
    }

    [PunRPC]
    public void AddElectricity(int value)
    {
        Electricity += value;
    }
    [PunRPC]
    public void AddGas(int value)
    {
        Gas += value;
    }
    [PunRPC]
    public void AddPieces(int value)
    {
        Pieces += value;
    }

    [PunRPC]
    public void AddGunToWorkload(Gun gun)
    {
        workLoad.Add(gun);
        OnGunReceived?.Invoke(gun);
        Debug.Log("Added Gun to Workload");
    }
}
