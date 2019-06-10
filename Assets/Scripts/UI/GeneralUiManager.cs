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
        AssemblyPlayer.OnAssembleStartet += UpdateWorkloadText;
        AssemblyPlayer.OnAssembleFinished += UpdateWorkloadText;
        WeaponWorkload.OnGunReceived += UpdateWorkloadText;
    }

    void Start()
    {
        workLoadCount.text = WeaponWorkload.Instance.workLoad.Count.ToString();
    }

    void UpdateWorkloadText()
    {
        workLoadCount.text = WeaponWorkload.Instance.workLoad.Count.ToString();
    }

    void UpdateWorkloadText(Gun gun)
    {
        workLoadCount.text = WeaponWorkload.Instance.workLoad.Count.ToString();
    }

    void OnDestroy()
    {
        AssemblyPlayer.OnAssembleStartet -= UpdateWorkloadText;
        AssemblyPlayer.OnAssembleFinished -= UpdateWorkloadText;
        WeaponWorkload.OnGunReceived -= UpdateWorkloadText;
    }
}
