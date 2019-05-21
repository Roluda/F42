using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerController Instance;
    [HideInInspector]
    public bool isRebel;
    [HideInInspector]
    public int position;
    [SerializeField]
    int initialGuns;
    public int electricity;
    public int gas;
    public int pieces;
    public List<Gun> workLoad = new List<Gun>();
    public Gun currentGun;

    protected const string addGunRpcName = "AddGunToWorkload";

    public virtual void Setup()
    {

    }

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
        isRebel = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsRebel"];
        position = (int)PhotonNetwork.LocalPlayer.CustomProperties["Position"];
        for (int i = 0; i < initialGuns; i++)
        {
            Gun newGun = new Gun(5);
            workLoad.Add(newGun);
        }
        Setup();
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
                }
                else
                {
                    Debug.Log("No next Player was found in order to add workload");
                }
            }
        }
        else
        {
            Debug.Log("Start Assembling First");
        }
    }

    [PunRPC]
    public void AddGunToWorkload(Gun gun)
    {
        workLoad.Add(gun);
        Debug.Log("Added Gun to Workload");
    }
}
