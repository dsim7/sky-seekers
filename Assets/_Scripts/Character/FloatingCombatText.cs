using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatingCombatText : MonoBehaviour
{
    public TMPro.TextMeshPro textMesh;
    public Animator animator;
    public bool active { get; private set; } = false;

    public void Appear(Transform anchor, string text, Color color = default)
    {
        transform.SetParent(anchor);
        transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        textMesh.text = text;
        textMesh.color = color;
        animator.SetTrigger("Appear");
        active = true;
    }

    public void Hide()
    {
        active = false;
    }
}
