using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class SupplyList : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    RectTransform entryParent = null;
    public ListEntry[] listEntries;
    [SerializeField]
    float spaceBetweenEntries = 50;
    [SerializeField]
    ListEntry entryPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("ClearList", RpcTarget.All);
        }
    }

    [PunRPC]
    void ChangeState(int entry, ListEntryState state)
    {
        if (entry < listEntries.Length)
        {
            listEntries[entry].State = state;
        }
        else
        {
            Debug.Log("Warning: Tried to Chnage not existing list Entry.");
        }
    }

    [PunRPC]
    void ClearList()
    {
        if (listEntries != null)
        {
            foreach (ListEntry entry in listEntries)
            {
                entry.State = ListEntryState.empty;
            }
        }
    }

    void Setup()
    {
        if (PhotonNetwork.IsConnected)
        {
            listEntries = new ListEntry[PhotonNetwork.PlayerList.Length-1];
            foreach (Player player in PhotonNetwork.PlayerList)
            {

                int i = (int)player.CustomProperties["Position"] - 1;
                if (i < listEntries.Length)
                {
                    listEntries[i] = Instantiate(entryPrefab, entryParent);
                    listEntries[i].position = i + 1;
                    listEntries[i].Name = player.NickName;
                    listEntries[i].State = ListEntryState.empty;
                    listEntries[i].GetComponent<RectTransform>().localPosition = new Vector3(0, -(i + 1) * spaceBetweenEntries, 0);
                }
            }
        }
        else
        {
            listEntries = new ListEntry[5];
            for(int i = 0; i < listEntries.Length; i++)
            {
                int j = i + 1;
                listEntries[i] = Instantiate(entryPrefab, entryParent);
                listEntries[i].position = j;
                listEntries[i].Name = "Debug "+j;
                listEntries[i].State = ListEntryState.empty;
                listEntries[i].GetComponent<RectTransform>().localPosition = new Vector3(0, -j * spaceBetweenEntries, 0);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
