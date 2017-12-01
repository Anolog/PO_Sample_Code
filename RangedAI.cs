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
        if (State.Type == STATE_TYPE.ATTACK && ((AttackAIState)State).WaitingForFirstAttack)
            return;

        if ((attackTimer <= Time.time || Vector3.Distance(transform.position, Target.transform.position) >= 6) && State.Type == STATE_TYPE.KITE)
        {
            SwitchState(new AttackAIState(this));
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance / 2)
        {
            if (State.Type == STATE_TYPE.KITE)
                return;

            SwitchState(new KiteAIState(this));
            return;
        }
        if ((Vector3.Distance(transform.position, Target.transform.position) <= AttackDistance) && State.Type != STATE_TYPE.ATTACK)
        {
            SwitchState(new AttackAIState(this));
            return;
        }
        if ((Vector3.Distance(transform.position, Target.transform.position) > AttackDistance || CheckHealth()) && State.Type != STATE_TYPE.CHASE)
        {
            SwitchState(new ChaseAIState(this));
            return;
        }
    }

    //protected override void ChaseUpdateFunc()
    //{
    //    base.ChaseUpdateFunc();
    //    if (Vector3.Distance(transform.position, Target.transform.position) <= 10)
    //    {
    //        Agent.isStopped = true;
    //        UpdateFunc = AttackUpdateFunc;
    //    }

    //    if (isKnockback)
    //    {
    //        UpdateFunc = KnockbackUpdateFunc;
    //    }

    //}

    //protected override void AttackUpdateFunc()
    //{ 
    //    LookAtTarget();
    //    base.AttackUpdateFunc();

    //    if (Vector3.Distance(transform.position, Target.transform.position) < 5)
    //    {
    //        Agent.isStopped = false;
    //        UpdateFunc = KiteUpdateFunc;
    //    }

    //    if (Vector3.Distance(transform.position, Target.transform.position) > 10)
    //    {
    //        Agent.isStopped = false;
    //        UpdateFunc = ChaseUpdateFunc;
    //    }

    //    if (isKnockback)
    //    {
    //        UpdateFunc = KnockbackUpdateFunc;
    //    }
    //}

    //protected void KiteUpdateFunc()
    //{
    //    AttackTimer -= Time.deltaTime;

    //    LookAtTarget();

    //    Ray rayFromTarget = new Ray();
    //    rayFromTarget.origin = Target.transform.position;
    //    Vector3 rayDirection = new Vector3(-transform.forward.x, 0, -transform.forward.z);

    //    rayFromTarget.direction = rayDirection;

    //    Agent.destination = rayFromTarget.GetPoint(6);

    //    if (AttackTimer <= 0 || Vector3.Distance(transform.position, Target.transform.position) <= 6)
    //    {
    //        Agent.isStopped = true;
    //        UpdateFunc = AttackUpdateFunc;
    //    }

    //    if (isKnockback)
    //    {
    //        UpdateFunc = KnockbackUpdateFunc;
    //    }
    //}
}
