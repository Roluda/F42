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
    [SerializeField]
    KillPlayerButton killButtonPrefab = null;
    [SerializeField]
    RectTransform killButtonsParent = null;
    [SerializeField]
    float spaceBetweenButtons = 60;

    KillPlayerButton[] killButtons;

    public override void CustomSetup()
    {
        WeaponWorkload.OnGunReceived += OnGunReceived;
        goalDisplay.text = WeaponWorkload.Instance.workLoad.Count + "/" + productionGoal + " guns assembled";
        if (PhotonNetwork.IsConnected)
        {
            killButtons = new KillPlayerButton[PhotonNetwork.PlayerList.Length - 1];
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                int i = (int)player.CustomProperties["Position"] - 1;
                if (i < killButtons.Length)
                {
                    killButtons[i] = Instantiate(killButtonPrefab, killButtonsParent);
                    killButtons[i].targetPosition = i + 1;
                    killButtons[i].Name = player.NickName;
                    killButtons[i].GetComponent<RectTransform>().localPosition = new Vector3(0, -(i + 1) * spaceBetweenButtons, 0);
                }
            }
        }
        else
        {
            killButtons = new KillPlayerButton[5];
            for (int i = 0; i < 5; i++)
            {
                killButtons[i] = Instantiate(killButtonPrefab, killButtonsParent);
                killButtons[i].targetPosition = i + 1;
                killButtons[i].Name = "Debug" + i;
                killButtons[i].GetComponent<RectTransform>().localPosition = new Vector3(0, -(i + 1) * spaceBetweenButtons, 0);
            }
        }
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
        remainingDisplay.text = string.Format("{0:D2}:{1:D2} remaining", ts.Minutes, ts.Seconds);
    }

    public void OnGunReceived(Gun g) {
        goalDisplay.text = WeaponWorkload.Instance.workLoad.Count + "/" + productionGoal + " guns assembled";
    }
}
