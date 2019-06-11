using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class WinScreenManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_Text winners = null;
    [SerializeField]
    bool rebelsWon = true;

    // Start is called before the first frame update
    void Start()
    {
        winners.text = WinnerText();
        GameManager.Instance.Invoke("LeaveRoom", 10);
    }

    string WinnerText()
    {
        List<string> winners = (rebelsWon) ? RebelWinners() : LoyalWinners();
        string text = winners[0];
        for (int i= 1; i<winners.Count-1; i++)
        {
            text += ", " + winners[i];
        }
        text += " and " + winners[winners.Count - 1];
        return text;
    }

    List<string> RebelWinners()
    {
        List<string> names = new List<string>();
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if ((bool)player.CustomProperties["IsRebel"])
            {
                names.Add(player.NickName);
            }
        }
        return names;
    }

    List<string> LoyalWinners()
    {
        List<string> names = new List<string>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!(bool)player.CustomProperties["IsRebel"])
            {
                names.Add(player.NickName);
            }
        }
        return names;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
