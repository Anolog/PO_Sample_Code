public class WaveSpawner : MonoBehaviour
{
    public static float TIME_BEFORE_FIRST_WAVE = 1f;
    public static float TIME_BETWEEN_WAVES = 5f;
    public static uint MINIMUM_POINTS_LEFT = 200;
    public List<GameObject> Enemies = new List<GameObject>();
    List<GameObject> EnemiesLeft = new List<GameObject>();
    List<GameObject> AllowedEnemies = new List<GameObject>();
    //public GameObject FodderEnemy;
    //public GameObject RangedEnemy;
    float TimeBetweenSpawn = 0;
    float CurrentTime = 0;
    bool CurrentlySpawning = false;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (EnemiesLeft.Count > 0)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                SpawnEnemy();
                CurrentTime = TimeBetweenSpawn;
            }
        }
        else
        {
            CurrentlySpawning = false;
        }
    }
    void SpawnEnemy()
    {
        // find a random spawn
        GameObject[] enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        int spawnNumber = Random.Range(0, enemySpawns.Length);
        // spawn in our enemy
        Instantiate(EnemiesLeft[EnemiesLeft.Count - 1], enemySpawns[spawnNumber].transform.position, enemySpawns[spawnNumber].transform.rotation);
        EnemiesLeft.RemoveAt(EnemiesLeft.Count - 1);
    }
    // Creates a wave based on the amount of points allowed, the difficulty of the enemies, and the total amount of time in seconds it takes to spawn all enemies
    public void CreateWave(uint points, KeyValuePair<GameObject, int>[] enemies, int length)
    {
        // this variable is used when finding what enemy we want to create
        int randMax = 0;
        // safety check, make sure that if we are already spawning in a wave you can't create a new one
        if (CurrentlySpawning)
        {
            return;
        }
        // Clear the lists of enemies we have
        AllowedEnemies.Clear();
        Enemies.Clear();
        for (int i = 0; i < enemies.Length; i++)
        {
            AllowedEnemies.Add(enemies[i].Key);
            randMax += enemies[i].Value;
        }
        // Find the enemy in this wave with the least amount of points
        uint minPoints = AllowedEnemies[0].GetComponent<CharacterStats>().ScorePointValue;
        for (int i = 1; i < AllowedEnemies.Count; i++)
        {
            uint enemyPoints = AllowedEnemies[0].GetComponent<CharacterStats>().ScorePointValue;
            if (enemyPoints < minPoints)
            {
                minPoints = enemyPoints;
            }
        }
        // if we still have points left we will still spawn enemies
        while (points > minPoints)
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
            while (AllowedEnemies[enemy].GetComponent<CharacterStats>().ScorePointValue > points)
            {
                enemy = Random.Range(0, AllowedEnemies.Count);
            }
            // add the enemy to the list of enemies
            Enemies.Add(AllowedEnemies[enemy]);
            // subtract the enemies point value from the total points allowed
            points -= AllowedEnemies[enemy].GetComponent<CharacterStats>().ScorePointValue;
        }
        // EnemiesLeft is used when actually spawning the wave
        EnemiesLeft = new List<GameObject>(Enemies);
        // find the time between enemy spawns
        TimeBetweenSpawn = (float)(length) / (float)Enemies.Count;
        CurrentTime = TimeBetweenSpawn;
        CurrentlySpawning = true;
    }
}