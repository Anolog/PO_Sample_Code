public class RangedEnemyAI : EnemyAI
{
    float AttackTimer = 3;
    override protected void Start()
    {
        base.Start();
        // Find our target
        Players = GameObject.FindGameObjectsWithTag("Player");
        FindTarget();
        Agent.destination = Target.transform.position;
        // Ranged enemies will start out chasing their target
        UpdateFunc = ChaseUpdateFunc;
        // Set our enemies attack ability
        Stats.Abilities[0] = new RockThrowAbility(Stats);
        Health = Stats.Health;
    }
    void ChaseUpdateFunc()
    {
        Agent.destination = Target.transform.position;
        transform.LookAt(Target.transform);
        if (Vector3.Distance(transform.position, Target.transform.position) <= 10)
        {
            Agent.isStopped = true;
            UpdateFunc = AttackUpdateFunc;
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
        transform.LookAt(Target.transform);
        // use attack here
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            Stats.Abilities[0].Use();
            attackTimer = 0;
            // don't check if I should leave this update func if I am attacking
            return;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) < 5)
        {
            Agent.isStopped = false;
            UpdateFunc = KiteUpdateFunc;
        }
        if (Vector3.Distance(transform.position, Target.transform.position) > 10)
        {
            Agent.isStopped = false;
            UpdateFunc = ChaseUpdateFunc;
        }
        if (isKnockback)
        {
            UpdateFunc = KnockbackUpdateFunc;
            StartingKnockbackPos = transform.position;
        }
    }
    void KiteUpdateFunc()
    {
        AttackTimer -= Time.deltaTime;
        transform.LookAt(Target.transform);
        Ray rayFromTarget = new Ray();
        rayFromTarget.origin = Target.transform.position;
        Vector3 rayDirection = new Vector3(-transform.forward.x, 0, -transform.forward.z);
        rayFromTarget.direction = rayDirection;
        Agent.destination = rayFromTarget.GetPoint(6);
        if (AttackTimer <= 0 || Vector3.Distance(transform.position, Target.transform.position) <= 6)
        {
            Agent.isStopped = true;
            UpdateFunc = AttackUpdateFunc;
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
    void FindNewTarget()
    {
    }
}