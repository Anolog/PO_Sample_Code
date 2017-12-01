using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Use this for the NavMeshAgent

public class MeleeEnemyAI : EnemyAI {

    protected override void Start()
    {
        base.Start();
        // Set our enemies attack ability
        Stats.Abilities[0] = new BasicMeleeAbility(Stats);
        AttackDistance = 1.5f;
    }

    protected override void CheckStateChange()
    {
        if ((Vector3.Distance(transform.position, Target.transform.position) <= AttackDistance) && State.Type != STATE_TYPE.ATTACK)
        {
            SwitchState(new AttackAIState(this));
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) > AttackDistance || CheckHealth() && State.Type != STATE_TYPE.CHASE)
        {
            SwitchState(new ChaseAIState(this));
            return;
        }
    }

    //protected override void ChaseUpdateFunc()
    //{
    //    base.ChaseUpdateFunc();
    //    if (Vector3.Distance(transform.position, Target.transform.position) < 1.5f)
    //    {
    //        Agent.isStopped = true;
    //        UpdateFunc = AttackUpdateFunc;
    //    }

    //    // if my saved health is less than the health in my stats I have been attacked and may need to find a new target
    //    if (Stats.Health < Health)
    //    {
    //        Health = Stats.Health;
    //        if (LatestHitType == Ability.AbilityType.Melee)
    //        {
    //            FindNewTarget(LatestAttacker);
    //        }
    //        else
    //        {
    //            float distanceToCurrent = Vector3.Distance(transform.position, Target.transform.position);
    //            float distanceToAttacker = Vector3.Distance(transform.position, LatestAttacker.transform.position);
    //            if (distanceToCurrent > distanceToAttacker)
    //            {
    //                FindNewTarget(LatestAttacker);
    //            }
    //        }
    //    }

    //    if (isKnockback)
    //    {
    //        UpdateFunc = KnockbackUpdateFunc;
    //    }
    //}

    //protected override void AttackUpdateFunc()
    //{
    //    base.AttackUpdateFunc();

    //    if (Vector3.Distance(transform.position, Target.transform.position) > 1.5f)
    //    {
    //        Agent.isStopped = false;
    //        UpdateFunc = ChaseUpdateFunc;
    //    }
    //    // if my saved health is less than the health in my stats I have been attacked and may need to find a new target
    //    else if (Health < Stats.Health)
    //    {
    //        Health = Stats.Health;
    //        if (LatestHitType == Ability.AbilityType.Melee)
    //        {
    //            FindNewTarget(LatestAttacker);
    //        }
    //        else
    //        {
    //            float distanceToCurrent = Vector3.Distance(transform.position, Target.transform.position);
    //            float distanceToAttacker = Vector3.Distance(transform.position, LatestAttacker.transform.position);
    //            if (distanceToCurrent > distanceToAttacker)
    //            {
    //                FindNewTarget(LatestAttacker);
    //            }
    //        }
    //        UpdateFunc = ChaseUpdateFunc;
    //    }

    //    if (isKnockback)
    //    {
    //        UpdateFunc = KnockbackUpdateFunc;
    //    }
    //}
}
