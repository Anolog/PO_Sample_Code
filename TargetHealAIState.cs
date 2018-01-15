using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHealAIState : AIState {

    private GameObject m_HealTarget;

    public TargetHealAIState(EnemyAI _Owner, GameObject healTarget)
        : base(_Owner)
    {
        Type = StateType.Heal;
        m_HealTarget = healTarget;
    }

    public override void Update()
    {
        if (m_HealTarget == null)
        {
            Owner.SwitchState(new WaitAIState(Owner, 0.25f));
            return;
        }

        Vector3 direction = m_HealTarget.transform.position - Owner.transform.position;
        direction.Normalize();
        direction.y = 0;
        Owner.transform.rotation = Quaternion.LookRotation(direction);

        if (m_HealTarget.GetComponent<CharacterStats>().Health >= m_HealTarget.GetComponent<CharacterStats>().MaxHealth)
        {
            Owner.SwitchState(new WaitAIState(Owner, 0.25f));
            m_HealTarget = null;
            return;
        }

        float distance = Vector3.Distance(m_HealTarget.transform.position, Owner.transform.position);
        if (distance >= 10)
        {
            Owner.SwitchState(new WaitAIState(Owner, 0.25f));
            m_HealTarget = null;
            return;
        }
    }

    public override void Deactivate()
    {

    }

    public override void Activate()
    {
        //Debug.Log("In Target Heal");
        Agent.isStopped = true;

        Owner.Stats.Abilities[0].Use(m_HealTarget);
    }
}
