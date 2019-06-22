using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class KillPlayerButton : MonoBehaviour
{
    public int targetPosition;
    [SerializeField]
    TMP_Text playerName = null;
    public string Name
    {
        get
        {
            return playerName.text;
        }
        set
        {
            playerName.text = value;
        }
    }

    public void ApplyForKill()
    {
        ConfirmPopup.Instance.ApplyForAction(KillTargetPlayer);
    }

    public void KillTargetPlayer()
    {
        Debug.Log("Trying to Kill Player on Position: " + targetPosition);
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if((int)player.CustomProperties["Position"] == targetPosition)
            {
                GameManager.Instance.photonView.RPC("KillWorker", player);
            }
        }
    }
}
