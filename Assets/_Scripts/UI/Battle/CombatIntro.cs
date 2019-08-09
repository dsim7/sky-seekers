using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatIntro : MonoBehaviour
{
    [SerializeField] BlackMask blackMask;
    [SerializeField] GameObject characters;
    [SerializeField] GameObject ui;

    private void Start()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(.6f);
        blackMask.FadeIn();
        yield return new WaitForSeconds(1f);
        characters.SetActive(true);
        ui.GetComponent<Animator>().SetTrigger("In");
    }
}
