using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Use this for the NavMeshAgent

public class MeleeEnemyAI : EnemyAI {

    protected override void Start()
    {
        base.Start();
        // Set our enemies attack ability
        Stats.Abilities[0] = new EnemyMeleeAbility(Stats);
        AttackDistance = 2.1f;
        Agent.stoppingDistance = AttackDistance;
    }

    protected override void CheckStateChange()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) <= AttackDistance)
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
