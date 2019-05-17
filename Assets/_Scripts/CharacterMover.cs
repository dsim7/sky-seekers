using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMover : MonoBehaviour
{
    public const float TIME_IN_MELEE = 0.3f;
    
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float meleeDistance;
    
    public Character Character { get; set; }

    Timer inMeleeTimer;
    Vector3 currentMeleePosition;
    Vector3 currentDestination;
    bool inMelee;

    bool moving;
    public bool Moving => moving;

    Transform anchor;
    public Transform Anchor
    {
        get { return anchor; }

        set
        {
            anchor = value;
            if (!inMelee && Vector3.Distance(transform.position, anchor.position) > 0.01)
            {
                currentDestination = anchor.position;
            }
        }
    }

    void Start()
    {
        inMeleeTimer = Timer.GetInstance(TIME_IN_MELEE, ReturnFromMelee, false);
    }

    void Update()
    {
        MoveToDestination();
    }

    public void MoveToMelee(CharacterActor target)
    {
        currentMeleePosition = Vector3.MoveTowards(target.transform.position, anchor.position, meleeDistance);
        currentDestination = currentMeleePosition;
        inMelee = true;
    }

    public void DoneInMelee()
    {
        inMeleeTimer.Reset();
        inMeleeTimer.Start();
    }

    void ReturnFromMelee()
    {
        currentDestination = anchor.position;
        inMelee = false;
    }

    void MoveToDestination()
    {
        if (Vector3.Distance(transform.position, currentDestination) > 0.01)
        {
            if (!moving)
            {
                moving = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
        }
        else if (moving)
        {
            moving = false;
        }
    }

}
