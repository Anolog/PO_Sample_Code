using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteAIState : AIState {

    public KiteAIState(EnemyAI _Owner)
        : base(_Owner)
    {
        Owner.attackTimer = Time.time + Owner.timeBetweenAttacks;
        Type = STATE_TYPE.KITE;
    }

    public override void Update()
    {
        Owner.LookAtTarget();

        Vector3 direction = new Vector3(Target.transform.forward.x, 0, Target.transform.forward.z);
        direction.Normalize();
        Agent.destination = 10 * direction;
    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        Agent.isStopped = false;
    }
}
