using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyList
{
    FodderEnemy,
    RockThrowEnemy,
    NumEnemies,
}


public class WaveSpawner : MonoBehaviour
{
    public static float TIME_BEFORE_FIRST_WAVE = 1f;
    public static float TIME_BETWEEN_WAVES = 5f;
    public static int MINIMUM_POINTS_LEFT = 20;

    public List<GameObject> Enemies = new List<GameObject>();
    private List<GameObject> m_EnemiesLeft = new List<GameObject>();
    private List<GameObject> m_AllowedEnemies = new List<GameObject>();

    //public GameObject FodderEnemy;
    //public GameObject RangedEnemy;

    private float m_TimeBetweenSpawn = 0;
    private float m_CurrentTime = 0;

    private bool m_CurrentlySpawning = false;
    private bool m_IsStopped = false;

    public void OverrideCurrentWave() { m_CurrentlySpawning = false; }

    public void StopSpawning()
    {
        m_IsStopped = true;
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Command.KillEnemy(enemy);
        }
        foreach(GameObject boss in GameObject.FindGameObjectsWithTag("Boss"))
        {
            if (boss != null)
            {
                Command.KillBoss(boss);
            }
        }
    }

    public void StartSpawning()
    {
        m_IsStopped = false;
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsStopped)
        {
            if (m_EnemiesLeft.Count > 0)
            {
                m_CurrentTime -= Time.deltaTime;
                if (m_CurrentTime <= 0)
                {
                    SpawnEnemy();
                    m_CurrentTime = m_TimeBetweenSpawn;
                }
            }
            else
            {
                m_CurrentlySpawning = false;
            }
        }
    }

    public void SpawnBoss(GameObject boss)
    {
        GameObject BossSpawn = GameObject.FindGameObjectWithTag("BossSpawn");
        Instantiate(boss, BossSpawn.transform.position, BossSpawn.transform.rotation);
        Enemies.Add(boss);
    }

    public int NumberEnemiesSpawned()
    {
        int totalEnemies = Enemies.Count;
        int numLeft = m_EnemiesLeft.Count;
        return totalEnemies - numLeft;
    }

    void SpawnEnemy()
    {
        // find a random spawn
        GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        int spawnNumber = Random.Range(0, enemySpawns.Length);

        // spawn in our enemy
        Instantiate(m_EnemiesLeft[m_EnemiesLeft.Count - 1], enemySpawns[spawnNumber].transform.position, enemySpawns[spawnNumber].transform.rotation);

        m_EnemiesLeft.RemoveAt(m_EnemiesLeft.Count - 1);
    }

    public delegate int del(int a);

    // pass in an array of enemy weightings as ints - see enum for order
    // pass in a list of enemylist values - default argument as numEnemies
    // need a variable time scale
    public void CreateWave(int points, int totalLengthInSeconds, del desiredPoints, int[] enemyWeighting, List<EnemyList> guaranteedEnemies = null)
    {
        
    }


    // Creates a wave based on the amount of points allowed, the difficulty of the enemies, and the total amount of time in seconds it takes to spawn all enemies
    public void CreateWave(int points, KeyValuePair<GameObject, int>[] enemies, int totalLengthInSeconds)
    {
        // this variable is used when finding what enemy we want to create
        int randMax = 0;

        // safety check, make sure that if we are already spawning in a wave you can't create a new one
        if (m_CurrentlySpawning)
        {
            return;
        }
        // Clear the lists of enemies we have
        m_AllowedEnemies.Clear();
        Enemies.Clear();

        for (int i = 0; i < enemies.Length; i++)
        {
            m_AllowedEnemies.Add(enemies[i].Key);
            randMax += enemies[i].Value;
        }

        // Find the enemy in this wave with the least amount of points
        int minPoints = m_AllowedEnemies[0].GetComponent<CharacterStats>().ScorePointValue;
        for (int i = 1; i < m_AllowedEnemies.Count; i++)
        {
            int enemyPoints = m_AllowedEnemies[0].GetComponent<CharacterStats>().ScorePointValue;
            if (enemyPoints < minPoints)
            {
                minPoints = enemyPoints;
            }
        }

        // if we still have points left we will still spawn enemies
        while (points >= minPoints)
        {
            // find the allowed enemy we will spawn based on the enemy weightings
            int enemy = Random.Range(0, randMax);
            int weighting = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                weighting += enemies[i].Value;
                if (enemy < weighting)
                {
                    enemy = i;
                    break;
                }
            }

            // make sure we have enough points left to spawn the enemy we want to spawn
            // if not find a new enemy
            while (m_AllowedEnemies[enemy].GetComponent<CharacterStats>().ScorePointValue > points)
            {
                enemy = Random.Range(0, m_AllowedEnemies.Count);
            }

            // add the enemy to the list of enemies
            Enemies.Add(m_AllowedEnemies[enemy]);

            // subtract the enemies point value from the total points allowed
            points -= m_AllowedEnemies[enemy].GetComponent<CharacterStats>().ScorePointValue;           
        }

        // EnemiesLeft is used when actually spawning the wave
        m_EnemiesLeft = new List<GameObject>(Enemies);

        // find the time between enemy spawns
        m_TimeBetweenSpawn = (float)(totalLengthInSeconds) / (float)Enemies.Count;
        m_CurrentTime = m_TimeBetweenSpawn;

        m_CurrentlySpawning = true;
    }
}

