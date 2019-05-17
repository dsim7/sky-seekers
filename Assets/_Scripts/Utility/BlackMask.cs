using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class BlackMask : MonoBehaviour
{
    CanvasGroup _cg;
    CanvasGroup cg { get { if (_cg == null) { _cg = GetComponent<CanvasGroup>(); } return _cg; } set { _cg = value; } }

    float alphaTarget = 0;
    UnityEvent thenEvent = new UnityEvent();

    public float fadeRate = 0.5f;

    void Awake()
    {
        cg.blocksRaycasts = true;
    }

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
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.blocksRaycasts = false;
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
        cg.blocksRaycasts = true;

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
        cg.blocksRaycasts = true;

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
        cg.blocksRaycasts = false;

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
