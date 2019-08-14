﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssemblyPlayer : PlayerController, IPunObservable
{
    public static AssemblyPlayer Instance = null;

    [SerializeField]
    ResourceVector assembleCosts = new ResourceVector(1, 1, 1);
    public Gun currentGun = null;

    public bool blockedInput;

    public delegate void StartAssembleAction();
    public static event StartAssembleAction OnAssembleStartet;
    public delegate void FinishAssembleAction();
    public static event FinishAssembleAction OnAssembleFinished;
    public delegate void ConstructGunAction();
    public static event ConstructGunAction OnGunConstructed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// call this method to get to upmost Gun from the remaining workload
    /// </summary>
    public void StartAssembly()
    {
        if (currentGun == null)
        {
            currentGun = WeaponWorkload.Instance.TakeTopGun();
            OnAssembleStartet?.Invoke();
        }
        else
        {
            Debug.Log("Error: Gun under construction or workload is empty");
        }
    }

    public void ConstructGun()
    {
        if (currentGun == null)
        {
            Debug.Log("NoGun");
            return;
        }
        if (currentGun.Completion < position)
        {
            currentGun.Completion++;
            OnGunConstructed?.Invoke();
        }
    }

    /// <summary>
    /// call this method when a player attached his part to the current gun.
    /// </summary>
    public void FinishAssembly()
    {
        if (currentGun != null)
        {
            if (PhotonNetwork.IsConnected)
            {
                if (currentGun.Completion == position)
                {
                    foreach (Player player in PhotonNetwork.PlayerList)
                    {
                        if ((int)player.CustomProperties["Position"] - position == 1) //find next player in Line
                        {
                            Debug.Log("sent Gun to " + player.NickName);
                            WeaponWorkload.Instance.photonView.RPC("AddGunToWorkload", player, (object)currentGun);
                            currentGun = null;
                            OnAssembleFinished?.Invoke();
                        }
                    }
                }
                else
                {
                    currentGun = null;
                }
            }
            else
            {
                if (currentGun.Completion == position)
                {
                    currentGun = null;
                    OnAssembleFinished?.Invoke();
                }
                else
                {
                    currentGun = null;
                }
            }
        }
    }



    public void TrashResources()
    {
        Debug.Log("trashing Resources");
        ResourceVector newCosts = new ResourceVector(
            Random.Range(1, ResourceManager.Instance.Electricity / 10),
            Random.Range(1, ResourceManager.Instance.Gas / 10),
            Random.Range(1, ResourceManager.Instance.Pieces / 10)
        );
        ResourceManager.Instance.RemoveResources(newCosts);
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
