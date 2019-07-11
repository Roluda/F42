using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitManager : MonoBehaviour
{
    public static TransitManager Instance;
    [SerializeField]
    Image roleImage = null;
    [SerializeField]
    Image rebelImage = null;
    [SerializeField]
    Image loyalImage = null;
    [SerializeField]
    TMP_Text instructions = null;

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
        instructions.text = "";
    }

    public void DebugDispaly()
    {
        DisplayRole(Random.value>0.5);
    }

    public void DisplayOverseer()
    {
        StopAllCoroutines();
        StartCoroutine(OverseerIntroProcedure());
    }

    public void DisplayRole(bool isRebel)
    {
        if (isRebel)
        {
            roleImage = rebelImage;
        }
        else
        {
            roleImage = loyalImage;
        }
        StopAllCoroutines();
        StartCoroutine(AssemblyIntroProcedure());
    }

    IEnumerator OverseerIntroProcedure()
    {
        StartCoroutine(TypeText(instructions, "you are the overseer", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "motivate your fellow employees", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "find and eliminate the rebels", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "avoid killing loyal workers", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "...", 2));
        yield return new WaitForSeconds(3);
    }

    IEnumerator AssemblyIntroProcedure()
    {
        StartCoroutine(TypeText(instructions, "the game is about to start", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "the following information is top secret", 2));
        yield return new WaitForSeconds(3);
        StartCoroutine(TypeText(instructions, "your role is:", 2));
        yield return new WaitForSeconds(3);
        instructions.text = "";
        StartCoroutine(FadeIn(roleImage, .5f));
        yield return new WaitForSeconds(2);
        StartCoroutine(FadeOut(roleImage, .5f));
    }

    IEnumerator TypeText(TMP_Text target, string message, float duration)
    {
        float interval = duration / message.Length;
        target.text = "";
        for(int i=0;i<message.Length; i++)
        {
            yield return new WaitForSeconds(interval);
            target.text = target.text + message[i];
        }
    }

    IEnumerator FadeIn(Image target, float duration)
    {
        while(target.color.a < 1)
        {
            Color newColor = target.color;
            newColor.a += Time.deltaTime * duration;
            target.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeOut(Image target, float duration)
    {
        while (target.color.a > 0)
        {
            Color newColor = target.color;
            newColor.a -= Time.deltaTime * duration;
            target.color = newColor;
            yield return null;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
