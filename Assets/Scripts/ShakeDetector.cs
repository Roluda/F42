using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public static ShakeDetector Instance;
    [SerializeField]
    float shakeMargin = 0.05f;

    public delegate void ShakeAction();
    public static event ShakeAction OnShake;

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaked())
        {
            OnShake?.Invoke();
            Debug.Log("SHAKE BABY BABY SHAKE ");
        }
    }

    bool isShaked()
    {
        float shakeX = Mathf.Abs(Input.acceleration.x);
        float shakeY = Mathf.Abs(Input.acceleration.y);
        return shakeX > shakeMargin || shakeY > shakeMargin;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
