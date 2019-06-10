using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class SupplyList : MonoBehaviourPunCallbacks, IPunObservable
{
    public static SupplyList Instance = null;
    public ListEntry[] listEntries;

    [SerializeField]
    RectTransform ui_element = null;
    [SerializeField]
    RectTransform showPos = null;
    [SerializeField]
    RectTransform hidePos = null;
    [SerializeField]
    float showSpeed = 200;
    [SerializeField]
    float smoothTime = 0.1f;
    [SerializeField]
    RectTransform entryParent = null;
    [SerializeField]
    TMP_Text countdownText = null;
    [SerializeField]
    float spaceBetweenEntries = 50;
    [SerializeField]
    ListEntry entryPrefab = null;

    Vector3 currentVelocity;
    bool isShowing;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("ClearList", RpcTarget.All);
            }
        }
        else
        {
            ClearList();
        }
    }

    void Update()
    {

    }

    [PunRPC]
    void ChangeState(int entry, ListEntryState state)
    {
        if (entry < listEntries.Length && entry>=0)
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
            listEntries = new ListEntry[PhotonNetwork.PlayerList.Length-1]; //One Entry for each player except overseer
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

    public void ShowList()
    {
        isShowing = !isShowing;
        if (isShowing)
        {
            Debug.Log("ShowList");
            StopAllCoroutines();
            StartCoroutine(MoveTransform(ui_element, showPos));
        }
        else
        {
            Debug.Log("HideList");
            StopAllCoroutines();
            StartCoroutine(MoveTransform(ui_element, hidePos));
        }
    }

    public void UpdateCountdown(float time)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(time);
        countdownText.text = "resupply in: " + timeSpan.Seconds;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    IEnumerator MoveTransform(RectTransform rect, RectTransform target)
    {
        Vector3 direction = (target.localPosition - rect.localPosition).normalized;
        while ((rect.localPosition - target.localPosition).sqrMagnitude >= 1f)
        {
            yield return null;
            rect.transform.position = Vector3.SmoothDamp(rect.position, target.position, ref currentVelocity, smoothTime, showSpeed);
        }
    }
}
