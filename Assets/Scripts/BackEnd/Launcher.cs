using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per Room")]
    [SerializeField]
    private byte minimumPlayersPerRoom = 3;
    [SerializeField]
    private byte maximumPlayersPerRoom = 6;

    [SerializeField]
    GameObject connectionPanel = null;
    [SerializeField]
    GameObject startMatchButton = null;
    [SerializeField]
    TMP_Text connectionText = null;
    [SerializeField]
    GameObject disconnectionButton = null;

    private bool isConnecting;
    private string gameVersion = "0.2";

    public static string roomID = "def";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Gun), 255, Gun.Serialize, Gun.Deserialize);
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(ResourceVector), 254, ResourceVector.Serialize, ResourceVector.Deserialize);
    }

    void Start()
    {
        connectionPanel.SetActive(true);
        connectionText.text = "";
        disconnectionButton.SetActive(false);
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayersPerRoom && PhotonNetwork.CurrentRoom.PlayerCount <= maximumPlayersPerRoom)
            {
                startMatchButton.SetActive(true);
            }
        }
        else
        {
            startMatchButton.SetActive(false);
        }
    }

    public void Connect()
    {
        isConnecting = true;
        connectionPanel.SetActive(false);
        connectionText.text = "connecting...";
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomID);
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void StartMatch()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Transfer");
        }
    }

    public override void OnConnectedToMaster()
    {
        connectionText.text = "connected to master server";
        Debug.Log("OnConnectedToMaster called by Launcher");
        if (isConnecting)
        {
            PhotonNetwork.JoinRoom(roomID);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectionButton.SetActive(false);
        connectionPanel.SetActive(true);
        connectionText.text = "";
        Debug.LogWarningFormat("OnDisconnected called by Launcher with reason {0}", cause);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        connectionText.text = "creating room " + roomID;
        Debug.Log("OnJoinRoomFailed called by Launcher. No such room availiable, will create new room");
        PhotonNetwork.CreateRoom(roomID, new RoomOptions { MaxPlayers = maximumPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        disconnectionButton.SetActive(true);
        connectionText.text = "room " + roomID + ": " + PhotonNetwork.CurrentRoom.PlayerCount +" players connected";        
        Debug.Log("OnJoinedRoom was called by Launcher");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectionText.text = "room " + roomID +": "+ PhotonNetwork.CurrentRoom.PlayerCount + " players connected";
        if (PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayersPerRoom && PhotonNetwork.CurrentRoom.PlayerCount <= maximumPlayersPerRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startMatchButton.SetActive(true);
            }
        }
        else
        {
            startMatchButton.SetActive(false);
        }
        Debug.Log("PlayerConnected");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        connectionText.text = "room " + roomID + ": " + PhotonNetwork.CurrentRoom.PlayerCount + " players connected";
        connectionText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + " players connected";
        if (PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayersPerRoom && PhotonNetwork.CurrentRoom.PlayerCount <= maximumPlayersPerRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startMatchButton.SetActive(true);
            }
        }
        else
        {
            startMatchButton.SetActive(false);
        }
    }
}
