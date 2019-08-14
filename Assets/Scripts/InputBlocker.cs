using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputBlocker : MonoBehaviour
{
    [SerializeField]
    float blockTime;
    [SerializeField]
    GameObject blockingImage;
    [SerializeField]
    TMP_Text countdown;
    bool isBlocked;
    

    // Start is called before the first frame update
    void Start()
    {
        blockingImage.SetActive(false);
        countdown.text = "";
        ShakeDetector.OnShake += StartBlock;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBlock()
    {
        if (!isBlocked) {
            StopAllCoroutines();
            StartCoroutine(BlockInput(blockTime));
        }
    }

    IEnumerator BlockInput(float time)
    {
        isBlocked = true;
        AssemblyPlayer.Instance.blockedInput = true;
        blockingImage.SetActive(true);
        float t = 0;
        while (t < time)
        {
            countdown.text = System.Math.Round(blockTime - t, 0).ToString();
            t += Time.deltaTime;
            if(blockTime-t < 1)
            {
                isBlocked = false;
            }
            yield return null;

        }
        countdown.text = "";
        blockingImage.SetActive(false);
        AssemblyPlayer.Instance.blockedInput = false;
    }

    void OnDisable()
    {
        ShakeDetector.OnShake -= StartBlock;
    }
}
