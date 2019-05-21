using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class GeneralUiManager : MonoBehaviour
{
    [SerializeField]
    GameObject isRebelIndicator;
    [SerializeField]
    TMP_Text workLoadCount;

    public void QuitToMenu()
    {
        GameManager.Instance.LeaveRoom();
    }

    void Start()
    {
        isRebelIndicator.SetActive(PlayerController.Instance.isRebel);
    }

    //HÄSSLICH AF --> Attriubute+Property+Event implementieren
    void Update()
    {
        workLoadCount.text = PlayerController.Instance.workLoad.Count.ToString();
    }
}
