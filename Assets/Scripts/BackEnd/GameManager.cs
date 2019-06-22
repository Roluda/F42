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
            DestroyImmediate(gameObject);
        }
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Transfer")
        {
            if (PhotonNetwork.IsMasterClient) {
                Player[] playersInRoom = PhotonNetwork.PlayerList;
                bool[] rebs = NewRebelSetup(playersInRoom.Length - 1, 2); //Spielerzahl festlegen //Später: Position zufällig
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
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (target == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Position"))
        {
            int position = (int)changedProps["Position"];
            bool isRebel = (bool)changedProps["IsRebel"];
            int players = PhotonNetwork.PlayerList.Length;
            if (position == players)
            {
                TransitManager.Instance.DisplayOverseer();
                StartCoroutine(LoadGameSceneDelayed("Overseer", 14));
            }
            else
            {
                TransitManager.Instance.DisplayRole(isRebel);
                StartCoroutine(LoadGameSceneDelayed("Player" + position, 14));
            }
        }
    }

    IEnumerator LoadGameSceneDelayed(string sceneName, float delay)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    [PunRPC]
    public void KillWorker()
    {
        if (PlayerController.isRebel)
        {
            AssemblyAgent.Agent.Instance.Active = true;
        }
        else
        {
            photonView.RPC("RebelWin", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RebelWin()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("RebelWin");
        }
    }

    [PunRPC]
    public void LoyalWin()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("LoyalWin");
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
