using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAIState : AIState {

    public bool WaitingForFirstAttack;

    public AttackAIState(EnemyAI _Owner)
        : base(_Owner)
    {
        Type = STATE_TYPE.ATTACK;
    }

    public override void Update()
    {
        Owner.LookAtTarget();

        if (Owner.attackTimer <= Time.time)
        {
            Owner.Stats.Abilities[0].Use();
            Owner.attackTimer = Time.time + Owner.timeBetweenAttacks;
            WaitingForFirstAttack = false;
        }
    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        Agent.isStopped = true;
        Owner.attackTimer = Time.time + Owner.timeBeforeFirstAttack;
        WaitingForFirstAttack = true;
    }
}
