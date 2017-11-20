public class MeleeEnemyAI : EnemyAI {
    protected override void Start()
    {
        base.Start();
        // Find our target
        Players = GameObject.FindGameObjectsWithTag("Player");
        FindTarget();
        Agent.destination = Target.transform.position;
        // Ranged enemies will start out chasing their target
        UpdateFunc = ChaseUpdateFunc;
        // Set our enemies attack ability
        Stats.Abilities[0] = new BasicMeleeAbility(Stats);
        Health = Stats.Health;
    }
    void ChaseUpdateFunc()
    {
        //Debug.Log("Chase");
        Agent.destination = Target.transform.position;
        Transform lookAtTransform = Target.transform;
        //lookAtTransform.position = new Vector3(Target.transform.position.x, Target.transform.position.y + 1, Target.transform.position.z);
        transform.LookAt(lookAtTransform);
        if (Vector3.Distance(transform.position, Target.transform.position) < 1.5f)
        {
            Agent.isStopped = true;
            UpdateFunc = AttackUpdateFunc;
        }
        // if my saved health is less than the health in my stats I have been attacked and may need to find a new target
        if (Stats.Health < Health)
        {
            Health = Stats.Health;
            if (LatestHitType == Ability.AbilityType.Melee)
            {
                FindNewTarget(LatestAttacker);
            }
            else
            {
                float distanceToCurrent = Vector3.Distance(transform.position, Target.transform.position);
                float distanceToAttacker = Vector3.Distance(transform.position, LatestAttacker.transform.position);
                if (distanceToCurrent > distanceToAttacker)
                {
                    FindNewTarget(LatestAttacker);
                }
            }
        }
        if (isKnockback)
        {
            UpdateFunc = KnockbackUpdateFunc;
            StartingKnockbackPos = transform.position;
        }
    }
    float timeBetweenAttacks = 2f,
          attackTimer = 0f;
    void AttackUpdateFunc()
    {
        //Debug.Log("Attack");
        // use attack here
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            Stats.Abilities[0].Use();
            attackTimer = 0;
            // don't check if I should leave this update func if I am attacking
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) > 1.5f)
        {
            Agent.isStopped = false;
            UpdateFunc = ChaseUpdateFunc;
        }
        // if my saved health is less than the health in my stats I have been attacked and may need to find a new target
        else if (Health < Stats.Health)
        {
            Health = Stats.Health;
            if (LatestHitType == Ability.AbilityType.Melee)
            {
                FindNewTarget(LatestAttacker);
            }
            else
            {
                float distanceToCurrent = Vector3.Distance(transform.position, Target.transform.position);
                float distanceToAttacker = Vector3.Distance(transform.position, LatestAttacker.transform.position);
                if (distanceToCurrent > distanceToAttacker)
                {
                    FindNewTarget(LatestAttacker);
                }
            }
            UpdateFunc = ChaseUpdateFunc;
        }
        if (isKnockback)
        {
            UpdateFunc = KnockbackUpdateFunc;
            StartingKnockbackPos = transform.position;
        }
    }
    void KnockbackUpdateFunc()
    {
        Vector3 velocity = new Vector3(-transform.forward.x, 0.5f, -transform.forward.z);
        Agent.velocity = 10 * velocity;
        if (Vector3.Distance(StartingKnockbackPos, transform.position) > 3)
        {
            Agent.velocity = Vector3.zero;
            Agent.isStopped = false;
            UpdateFunc = ChaseUpdateFunc;
            isKnockback = false;
        }
    }
    void FindNewTarget(GameObject target)
    {
        Target = target;
    }
}