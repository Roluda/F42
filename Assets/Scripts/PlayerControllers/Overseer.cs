using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Overseer : PlayerController
{
    [SerializeField]
    SupplyList theList = null;

    public int electricityGain = 10;
    public int gasGain = 10;
    public int piecesGain = 10;

    public override void CustomSetup()
    {
        
    }

    public void SendSupplyToPlayers()
    { 
        foreach(ListEntry entry in theList.listEntries)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if ((int)player.CustomProperties["Position"] == entry.position)
                {
                    switch (entry.State)
                    {
                        case ListEntryState.electricity:
                            photonView.RPC(addElectricityRpcName, player, electricityGain);
                            break;
                        case ListEntryState.gas:
                            photonView.RPC(addGasRpcName, player, gasGain);
                            break;
                        case ListEntryState.pieces:
                            photonView.RPC(addPiecesRpcName, player, piecesGain);
                            break;
                    }
                }
            }
        }
    }
}
