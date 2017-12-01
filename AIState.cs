using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum STATE_TYPE
{
    CHASE,
    ATTACK,
    KITE,
    DEFAULT,
}


public abstract class AIState
{
    public AIState(EnemyAI owner)
    {
        Owner = owner;
        transform = owner.transform;
        Target = owner.Target;
        Agent = owner.Agent;
    }

    public abstract void Update();
    public abstract void Deactivate();
    public abstract void Activate();

    public EnemyAI Owner;
    public STATE_TYPE Type;
    protected Transform transform;
    protected GameObject Target;
    protected NavMeshAgent Agent;
}
