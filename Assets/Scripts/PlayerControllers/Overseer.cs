using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class Overseer : PlayerController
{
    [SerializeField]
    float goalTime = 300;
    [SerializeField]
    int productionGoal = 100;
    [SerializeField]
    TMP_Text goalDisplay = null;
    [SerializeField]
    TMP_Text remainingDisplay = null;

    public override void CustomSetup()
    {
        WeaponWorkload.OnGunReceived += OnGunReceived;
    }

    void Update()
    {

        if (goalTime > 0)
        {
            goalTime -= Time.deltaTime;
        }
        else
        {
            if (WeaponWorkload.Instance.workLoad.Count >= productionGoal)
            {
                GameManager.Instance.photonView.RPC("LoyalWin", RpcTarget.All);
            }
            else
            {
                GameManager.Instance.photonView.RPC("RebelWin", RpcTarget.All);
            }
        }
        System.TimeSpan ts = System.TimeSpan.FromSeconds(goalTime);
        remainingDisplay.text = "time remaining: " + ts.Seconds;
    }

    public void OnGunReceived(Gun g) {
        goalDisplay.text = WeaponWorkload.Instance.workLoad.Count + "/" + productionGoal + " guns assembled";
    }
}
