using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1AI : EnemyAI
{

    //private float m_MostHealth;
    private bool m_NeedNewTarget;
    public float TeleportDistance = 10;
    private Dictionary<PlayerStats, int> m_PlayerDamages = new Dictionary<PlayerStats, int>();
    private Ability.AbilityType m_CurrentTargetAbilityType;
    public Ability.AbilityType GetCurrentTargetAbilityType() { return m_CurrentTargetAbilityType; }

    private float m_GroundShockCoolDownTime;
    private float m_EnergyMineCoolDownTime;
    private float m_KnockbackCoolDownTime;

    private float m_GroundShockTimer;
    private float m_EnergyMineTimer;
    private float m_KnockbackTimer;

    private GameObject[] TeleportLocations;

    private float m_TimeForRecentDamage = 5;
    private float m_RecentDamageTimer;
    private float m_RecentDamage = 0;
    private float m_AmountOfDamageAllowed = 60;

    protected override void Start()
    {
        // setup out stats and navigation
        Agent = GetComponent<NavMeshAgent>();
        Stats = GetComponent<CharacterStats>();
        Agent.speed = Stats.MovementSpeed;
        Agent.acceleration = Stats.Acceleration;
        Agent.angularSpeed = Stats.AngularSpeed;
        timeBetweenAttacks = Stats.AttSpd;
        Body = GetComponent<Rigidbody>();
        Stats.MaxHealth = (int)Stats.Health;
        AttackDistance = 1f;

        // setup our abilities
        Stats.Abilities[0] = new BossMeleeAbility(Stats);

        Stats.Abilities[1] = new EnergyMineAbility(Stats);
        m_EnergyMineCoolDownTime = Stats.Abilities[1].GetCoolDown() * 2;
        m_EnergyMineTimer = m_EnergyMineCoolDownTime + Time.time;

        Stats.Abilities[2] = new GroundShockAbility(Stats);
        m_GroundShockCoolDownTime = Stats.Abilities[2].GetCoolDown() * 2;
        m_GroundShockTimer = m_GroundShockCoolDownTime + Time.time;

        Stats.Abilities[3] = new TeleportAbility(Stats);

        Stats.Abilities[4] = new BasicKnockbackAbility(Stats);
        m_KnockbackCoolDownTime = Stats.Abilities[4].GetCoolDown() * 2;
        m_KnockbackTimer = m_KnockbackCoolDownTime + Time.time;

        // Find our target
        Players = GameManager.playerManager.PlayerList();
        FindInitialTarget();
        Agent.destination = Target.transform.position;
        SwitchState(new BossChaseAIState(this));

        TeleportLocations = GameObject.FindGameObjectsWithTag("BossTeleportLocation");
    }

    protected override void Update()
    {
        if (Target != null)
        {
            if (Target.gameObject.tag != "Player")
            {
                FindTarget();
                return;
            }
        }

        if (Stats.isControllable)
        {
            CheckStateChange();
            State.Update();
        }

        if (m_RecentDamageTimer >= Time.time)
        {
            m_RecentDamage = 0;
        }
    }

    public void AddDamage(PlayerStats attacker, int damage, Ability.AbilityType type)
    {
        //Debug.Log(Stats.Health);

        // reset the damage counter timer everytime I take damage
        m_RecentDamageTimer = m_TimeForRecentDamage + Time.time;
        m_RecentDamage += damage;

        // check if my recent damage is above the threshold
        if (m_RecentDamage >= m_AmountOfDamageAllowed)
        {
            // find a random teleport location and switch my state to teleport there
            SwitchState(new BossTeleportAIState(this, TeleportLocations[Random.Range(0, 4)]));
            m_RecentDamage = 0;
        }

        m_PlayerDamages[attacker] += damage;
        m_NeedNewTarget = CheckIfNeedNewTarget();

        if (m_NeedNewTarget)
        {
            m_CurrentTargetAbilityType = type;
        }

        float tenPercentOfMaxHealth = Stats.MaxHealth * 0.1f;
        float maxHealthMinusTenPercent = Stats.MaxHealth - tenPercentOfMaxHealth;
        float currentHealthMinusTenPercent = Stats.Health - tenPercentOfMaxHealth;

        float AbilityTimerCoolDownMultiplier = ((currentHealthMinusTenPercent / maxHealthMinusTenPercent) + 1) / 2f;
        m_GroundShockCoolDownTime *= AbilityTimerCoolDownMultiplier;
        m_EnergyMineCoolDownTime *= AbilityTimerCoolDownMultiplier;
        m_KnockbackCoolDownTime *= AbilityTimerCoolDownMultiplier;
    }

    private bool CheckIfNeedNewTarget()
    {
        int mostDamage = 0;
        PlayerStats tempTarget = null;
        foreach (PlayerStats player in m_PlayerDamages.Keys)
        {
            if (mostDamage < m_PlayerDamages[player])
            {
                mostDamage = m_PlayerDamages[player];
                tempTarget = player;
            }
        }

        if (tempTarget != null)
        {
            Target = tempTarget.gameObject;
            return true;
        }
        return false;
    }

    protected override void CheckStateChange()
    {
        if (m_NeedNewTarget)
        {
            SwitchState(new BossSwitchTargetAIState(this));
            m_NeedNewTarget = false;
            return;
        }


        if (m_GroundShockTimer <= Time.time)
        {
            RaycastHit hit;
            Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
            if (Physics.Raycast(ray, out hit, 4))
            {
                if (hit.rigidbody != null)
                {
                    if (hit.rigidbody.gameObject.tag == "Player")
                    {
                        SwitchState(new BossGroundShockAIState(this));
                        m_GroundShockTimer = m_GroundShockCoolDownTime + Time.time;
                    }
                }
            }
        }

        if (m_EnergyMineTimer <= Time.time)
        {
            SwitchState(new BossEnergyMineAIState(this));
            m_EnergyMineTimer = m_EnergyMineCoolDownTime + Time.time;
        }

        if (m_KnockbackTimer <= Time.time)
        {
            if (Vector3.Distance(Target.transform.position, gameObject.transform.position) <= AttackDistance)
            {
                SwitchState(new BossKnockbackAIState(this));
                m_KnockbackTimer = m_KnockbackCoolDownTime + Time.time;
            }
        }
    }

    protected virtual void FindInitialTarget()
    {
        GameObject tempTarget = null;
        float mostHealth = 0;
        int i = 0;
        foreach (GameObject player in Players)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            m_PlayerDamages.Add(stats, 0);
            if (player.GetComponent<CharacterStats>().Health > mostHealth)
            {
                mostHealth = player.GetComponent<CharacterStats>().Health;
                tempTarget = player;
            }
            i++;
        }

        //m_MostHealth = mostHealth;
        FindNewTarget(tempTarget);
    }
}
