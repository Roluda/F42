using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPopup : MonoBehaviour
{
    public static ConfirmPopup Instance;

    [SerializeField]
    GameObject panel;

    public delegate void Action();
    public Action actionToConfirm = null;


    // Start is called before the first frame update
    void Start()
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

    public void ApplyForAction(Action act)
    {
        actionToConfirm = act;
        Show();
    }

    public void Confirm()
    {
        actionToConfirm?.Invoke();
        Hide();
    }
    public void Decline()
    {
        Hide();
    }
    public void Show()
    {
        panel.SetActive(true);
    }
    public void Hide()
    {
        panel.SetActive(false);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
