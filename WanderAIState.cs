using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderAIState : AIState
{
    private GameObject WanderLocation = null;
    private GameObject PreviousWanderLocation = null;
    float closeEnough = 0.5f;

    public WanderAIState(EnemyAI _Owner)
        : base(_Owner)
    {
        Type = StateType.Wander;
    }

    public override void Update()
    {
        Agent.destination = WanderLocation.transform.position;
        if (Agent.velocity.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(Agent.velocity.normalized); // look into why updateRotation isnt working

        if (Vector3.Distance(WanderLocation.transform.position, Owner.gameObject.transform.position) <= closeEnough)
        {
            Owner.SwitchState(new WaitAIState(Owner, 0.25f));
        }

    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        //Debug.Log("In Wander");
        Agent.isStopped = false;
        WanderLocation = Owner.WanderLocations[Random.Range(0, Owner.WanderLocations.Length - 1)];
        if (PreviousWanderLocation != null)
        {
            while (WanderLocation == PreviousWanderLocation)
            {
                WanderLocation = Owner.WanderLocations[Random.Range(0, Owner.WanderLocations.Length - 1)];
            }
        }
        PreviousWanderLocation = WanderLocation;
    }
}
