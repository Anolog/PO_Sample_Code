using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Use this for the NavMeshAgent

public class RangedEnemyAI : EnemyAI
{
    override protected void Start()
    {
        base.Start();
        // Set our enemies attack ability
        Stats.Abilities[0] = new RockThrowAbility(Stats);
        AttackDistance = 10f;

        Agent.stoppingDistance = 10;
    }

    protected override void CheckStateChange()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance / 2 && State.Type == StateType.Attack)
        {
            SwitchState(new KiteAIState(this));
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackDistance && State.Type != StateType.Kite)
        {
            SwitchState(new AttackAIState(this));
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) > AttackDistance || CheckHealth())
        {
            SwitchState(new ChaseAIState(this));
            return;
        }
    }
}
