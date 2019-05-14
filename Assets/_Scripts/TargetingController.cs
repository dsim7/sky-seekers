using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetingController : MonoBehaviour
{
    [SerializeField]
    TargetingRaycaster controls;
    [SerializeField]
    Texture2D targetingTexture;

    List<Character> selectedChars;
    int numOfTargetsSelecting;
    Predicate<Character> targetChecker;
    UnityAction<List<Character>> onComplete;
    UnityAction onCancel;

    bool targeting;
    bool Targeting
    {
        get { return targeting; }

        set
        {
            targeting = value;
            if (value)
            {
                Cursor.SetCursor(targetingTexture, targetingTextureCenter, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
    Vector2 targetingTextureCenter;

    void Start()
    {
        targetingTextureCenter = new Vector2(targetingTexture.width / 2, targetingTexture.height / 2);

        selectedChars = new List<Character>();
        controls.RegisterListener(SelectTarget);
    }

    void Update()
    {
        if (targeting && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelTargeting();
        }
    }

    void SelectTarget(CharacterActor selected)
    {
        if (targeting = true && targetChecker != null && targetChecker(selected.Character))
        {
            selectedChars.Add(selected.Character);
            if (selectedChars.Count >= numOfTargetsSelecting)
            {
                onComplete.Invoke(selectedChars);

                selectedChars.Clear();
                onComplete = null;
                onCancel = null;
                targetChecker = null;
                numOfTargetsSelecting = 0;
                Targeting = false;
            }
        }
    }

    public void ChooseNewTargets(int numberOfTargets, Predicate<Character> checker, 
        UnityAction<List<Character>> onTargetingComplete, UnityAction onTargetingCancel)
    {
        selectedChars.Clear();
        numOfTargetsSelecting = numberOfTargets;
        targetChecker = checker;
        onComplete = onTargetingComplete;
        onCancel = onTargetingCancel;
        Targeting = true;
    }

    public void CancelTargeting()
    {
        if (onCancel != null)
        {
            onCancel.Invoke();
        }

        selectedChars.Clear();
        onComplete = null;
        onCancel = null;
        targetChecker = null;
        numOfTargetsSelecting = 0;
        Targeting = false;
    }
}
