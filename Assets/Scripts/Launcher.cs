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
    private byte maximumPlayersPerRoom = 6;

    [SerializeField]
    GameObject connectionPanel;
    [SerializeField]
    TMP_Text connectionText;

    private bool isConnecting;
    private string gameVersion = "1";

    public static string roomID = "def";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        ExitGames.Client.Photon.PhotonPeer.RegisterType(typeof(Gun), 255, Gun.Serialize, Gun.Deserialize);
    }

    void Start()
    {
        connectionPanel.SetActive(true);
        connectionText.text = "";
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
        connectionText.text = "room " + roomID + ": " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + " connected";
        Debug.Log("OnJoinedRoom was called by Launcher");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        connectionText.text = "room " + roomID +": "+ PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers +" connected";
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Transfer");
            }
        }
        Debug.Log("PlayerConnected");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        connectionText.text = "room " + roomID + ": " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + " connected";
        connectionText.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + " connected";
    }
}
