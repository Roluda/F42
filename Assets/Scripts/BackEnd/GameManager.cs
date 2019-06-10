using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instance;
    bool isLeaving = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {         
        if (PhotonNetwork.IsMasterClient)
        {
            Player[] playersInRoom = PhotonNetwork.PlayerList; 
            bool[] rebs = NewRebelSetup(playersInRoom.Length -1, 2); //Spielerzahl festlegen //Später: Position zufällig
            for (int i = 0; i < playersInRoom.Length; i++)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                props.Add("Position", i + 1);
                if (i < playersInRoom.Length - 1)
                {
                    props.Add("IsRebel", rebs[i]);
                }
                else
                {
                    props.Add("IsRebel", false);
                }
                PhotonNetwork.PlayerList[i].SetCustomProperties(props); //Synchron auf allen clients
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (target == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Position"))
        {
            int position = (int)changedProps["Position"];
            int players = PhotonNetwork.PlayerList.Length;
            PhotonNetwork.AutomaticallySyncScene = false;
            if (position == players)
            {
                SceneManager.LoadScene("Overseer");
            }
            else
            {
                SceneManager.LoadScene("Player" + position);
            }
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        SceneManager.LoadScene(0);
        Instance = null;
        PhotonNetwork.Destroy(gameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Waiting? Return to Lobby --> Everyone Leave Room!
        if (!isLeaving)
        {
            isLeaving = true;
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// returns á random positioning of the rebels from left to right
    /// </summary>
    /// <param name="maxPlayers"></param>
    /// <param name="rebelCount"></param>
    /// <returns></returns>
    bool[] NewRebelSetup(int maxPlayers, int rebelCount)
    {
        bool[] rebels = new bool[maxPlayers];
        if (maxPlayers < rebelCount)
        {
            throw new System.Exception("rebelCount has to be lower than maxPlayers");
        }
        while (rebelCount > 0)
        {
            int r = Random.Range(0, maxPlayers);
            if (!rebels[r])
            {
                rebels[r] = true;
                rebelCount--;
            }
        }
        return rebels;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
