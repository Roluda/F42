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
    TMP_Text workLoadCount = null;

    public void QuitToMenu()
    {
        GameManager.Instance.LeaveRoom();
    }

    void Awake()
    {
        PlayerController.OnAssembleStartet += UpdateWorkloadText;
        PlayerController.OnAssembleFinished += UpdateWorkloadText;
        PlayerController.OnGunReceived += UpdateWorkloadText;
    }

    void Start()
    {
        workLoadCount.text = PlayerController.Instance.workLoad.Count.ToString();
    }

    void UpdateWorkloadText()
    {
        workLoadCount.text = PlayerController.Instance.workLoad.Count.ToString();
    }

    void UpdateWorkloadText(Gun gun)
    {
        workLoadCount.text = PlayerController.Instance.workLoad.Count.ToString();
    }
}
