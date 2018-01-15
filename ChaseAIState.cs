using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAIState : AIState
{  
    public ChaseAIState(EnemyAI _Owner)
        : base(_Owner)
    {
        Type = StateType.Chase;

    }

    public override void Update()
    {
        UnityEngine.AI.NavMeshHit hit;

        CapsuleCollider col = Target.GetComponent<CapsuleCollider>();
        float height = col.height;
        if (UnityEngine.AI.NavMesh.SamplePosition(Target.transform.position + height * 0.5f * Vector3.up, out hit, 2 * height, 0))
        {
            Agent.SetDestination(hit.position);
        }

        //Agent.SetDestination(Target.transform.position);

        //if (Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
        //{
        //    if (UnityEngine.AI.NavMesh.SamplePosition(Target.transform.position, out hit, 5, 0))
        //        Agent.SetDestination(hit.position);
        //    else
        //        Agent.ResetPath();
        //}
        //if (UnityEngine.AI.NavMesh.SamplePosition(Target.transform.position, out hit, 5, UnityEngine.AI.NavMesh.AllAreas))
        //{
        //    Agent.SetDestination(hit.position);
        //    //Debug.Log(Agent.pathStatus);
        //}

        if (Agent.velocity.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(Agent.velocity.normalized); // look into why updateRotation isnt working

        // check if I have been injured
        Owner.CheckHealth();
    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        //Debug.Log("In Chase");
        Agent.isStopped = false;
    }
}
