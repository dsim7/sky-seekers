using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterMover : MonoBehaviour
{
    public const float TIME_IN_MELEE = 0.3f;
    public const float MOVE_SPEED = 35f;
    public const float MELEE_DISTANCE = 2f;
    
    public Character Owner { get; set; }

    Timer inMeleeTimer;
    Vector3 currentMeleePosition;
    Vector3 currentDestination;
    bool inMelee;

    public bool Moving { get; private set; }

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
        currentMeleePosition = Vector3.MoveTowards(target.transform.position, anchor.position, MELEE_DISTANCE);
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
            if (!Moving)
            {
                Moving = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, MOVE_SPEED * Time.deltaTime);
        }
        else if (Moving)
        {
            Moving = false;
        }
    }

}
