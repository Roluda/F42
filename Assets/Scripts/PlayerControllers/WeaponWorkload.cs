using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponWorkload : MonoBehaviourPunCallbacks, IPunObservable
{
    public static WeaponWorkload Instance = null;
    public List<Gun> workLoad = new List<Gun>();

    public delegate void GunReceivedAction(Gun gun);
    public static event GunReceivedAction OnGunReceived;

    [SerializeField]
    int initialGuns = 0;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialGuns; i++)
        {
            Gun newGun = new Gun();
            newGun.MaxCompletion = PlayerController.assemblyPlayers;
            newGun.Completion = PlayerController.position - 1;
            workLoad.Add(newGun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// returns and removes the topmost Gun in the list
    /// </summary>
    /// <returns></returns>
    public Gun TakeTopGun()
    {
        if (workLoad.Count > 0)
        {
            Gun topGun = workLoad[workLoad.Count - 1];
            workLoad.Remove(topGun);
            return topGun;
        }
        else
        {
            return null;
        }
    }

    [PunRPC]
    public void AddGunToWorkload(Gun gun)
    {
        workLoad.Add(gun);
        OnGunReceived?.Invoke(gun);
        Debug.Log("Added Gun to Workload");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
