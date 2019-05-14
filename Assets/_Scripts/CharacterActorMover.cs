using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActorMover : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float timeInMelee;
    [SerializeField]
    float meleeDistance;

    float inMeleeTimer;
    Vector3 currentMeleePosition;
    Vector3 currentDestination;
    bool inMelee;

    Transform anchor;
    public Transform Anchor
    {
        get { return anchor; }

        set
        {
            anchor = value;
            if (!inMelee)
            {
                currentDestination = anchor.position;
            }
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, currentDestination) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
        }
        if (inMelee)
        {
            HelperMethods.UpdateTimer(ref inMeleeTimer, timeInMelee, ReturnFromMelee);
        }
    }

    public void MoveToMelee(CharacterActor target)
    {
        currentMeleePosition = Vector3.MoveTowards(target.transform.position, anchor.position, meleeDistance);
        currentDestination = currentMeleePosition;
        inMeleeTimer = 0;
        inMelee = true;
    }

    void ReturnFromMelee()
    {
        currentDestination = anchor.position;
        inMelee = false;
    }
}
