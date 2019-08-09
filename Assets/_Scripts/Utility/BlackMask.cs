using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BlackMask : MonoBehaviour
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] Image img;
    [SerializeField] [Range(0,1f)] float alphaTarget = 0;
    [SerializeField] UnityEvent thenEvent = new UnityEvent();

    public float fadeRate = 0.5f;

    void Update()
    {
        if (cg.alpha == alphaTarget)
        {
            thenEvent.Invoke();
            thenEvent.RemoveAllListeners();
        }
        else
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, alphaTarget, fadeRate * Time.deltaTime);
        }
    }

    public void InstantFadeTo(float amount)
    {
        alphaTarget = amount;
        cg.alpha = amount;

        if (amount != 0)
        {
            Enable();
            img.raycastTarget = true;
        }
        else
        {
            img.raycastTarget = false;
        }
    }

    public void FadeOut()
    {
        FadeOut(null);
    }

    public void FadeOut(UnityAction then)
    {
        Enable();
        alphaTarget = 1;
        img.raycastTarget = true;

        if (then != null)
        {
            thenEvent.AddListener(then);
        }
    }

    public void FadeOutPartial(float amount)
    {
        FadeOutPartial(amount, null);
    }

    public void FadeOutPartial(float amount, UnityAction then)
    {
        Enable();
        alphaTarget = amount;
        img.raycastTarget = true;

        if (then != null)
        {
            thenEvent.AddListener(then);
        }
    }

    public void FadeIn()
    {
        FadeIn(null);
    }

    public void FadeIn(UnityAction then)
    {
        Enable();
        alphaTarget = 0;
        img.raycastTarget = false;

        if (then != null)
        {
            thenEvent.AddListener(then);
        }
        thenEvent.AddListener(Disable);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    void Enable()
    {
        gameObject.SetActive(true);
    }

}
