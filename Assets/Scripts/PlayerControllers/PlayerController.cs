using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPunCallbacks
{
    public static bool isRebel = true;
    public static int position = 1;
    public static int assemblyPlayers = 3;
    protected const string addGunRpcName = "AddGunToWorkload";
    protected const string addElectricityRpcName = "AddElectricity";
    protected const string addGasRpcName = "AddGas";
    protected const string addPiecesRpcName = "AddPieces";


    void Start()
    {
        if (PhotonNetwork.IsConnected) {
            Debug.Log("PlayerConroller is connected");
            isRebel = (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsRebel"];
            Debug.Log("Position: " + (int)PhotonNetwork.LocalPlayer.CustomProperties["Position"]);
            position = (int)PhotonNetwork.LocalPlayer.CustomProperties["Position"];
            assemblyPlayers = PhotonNetwork.PlayerList.Length - 1;
        }
        CustomSetup();
    }

    public virtual void CustomSetup()
    {

    }
}
