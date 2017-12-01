using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAIState : AIState {

    public ChaseAIState(EnemyAI _Owner)
        : base(_Owner)
    {
        Type = STATE_TYPE.CHASE;
    }

    public override void Update()
    {
        Agent.destination = Target.transform.position;
        transform.rotation = Quaternion.LookRotation(Agent.velocity.normalized);

        // check if I have been injured
        Owner.CheckHealth();
    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        Agent.isStopped = false;
    }
}
