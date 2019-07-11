using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    [SerializeField]
    float shakeMargin = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaked())
        {
            Debug.Log("SHAKE BABY BABY SHAKE ");
        }
    }

    bool isShaked()
    {
        float shakeX = Mathf.Abs(Input.acceleration.x);
        float shakeY = Mathf.Abs(Input.acceleration.y);
        return shakeX > shakeMargin || shakeY > shakeMargin;
    }
}
