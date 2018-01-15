using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Command
{

    //Pending implementation of character object
    //Character* m_Character;
    public static void MoveTowards(GameObject player, Vector3 targetPosition)
    {
        // FOR AI
    }

    public static void Move(GameObject player, Vector2 direction)
    {
        Rigidbody body = player.GetComponent<Rigidbody>();
        CharacterStats stats = player.GetComponent<CharacterStats>();

        float maxSpeed = stats.MovementSpeed;

        if (direction.y < 0)
            maxSpeed *= 0.5f;

        Vector3 velocity = body.velocity;
        velocity.y = 0;
        float speed = velocity.magnitude;

        float desiredSpeed = speed + stats.Acceleration * Time.deltaTime;
        desiredSpeed = Mathf.Clamp(desiredSpeed, 0, maxSpeed);

        Vector3 direction3D = Vector3.Normalize(direction.x * body.transform.right + direction.y * body.transform.forward);
        Vector3 desiredVelocity = direction3D * desiredSpeed;

        Vector3 velocityDifference = desiredVelocity - velocity;
        velocityDifference = Vector3.ClampMagnitude(velocityDifference, stats.Acceleration * Time.deltaTime);

        body.AddForce(velocityDifference, ForceMode.VelocityChange);
    }

    public static void RotateTowards(GameObject player, Quaternion targetRotation)
    {
        // FOR AI
    }

    public static void RotateConstSpeedHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.transform.eulerAngles;

        rot.y += horizontalRotation * player.GetComponent<CharacterStats>().AngularSpeed * Time.deltaTime;

        player.transform.rotation = (Quaternion.Euler(rot.x, rot.y, rot.z));
    }

    public static void RotatePlayerCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<PlayerStats>().Camera.GetComponent<Third_Person_Camera>();

        float rot = cam.VerticalRotation;

        rot += verticalRotation * player.GetComponent<PlayerStats>().AngularSpeed * Time.deltaTime;
        rot = Mathf.Clamp(rot, -1, 1);

        cam.VerticalRotation = rot;
    }

    public static void RotateHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.GetComponent<Rigidbody>().rotation.eulerAngles;

        rot.y -= horizontalRotation * Time.deltaTime;

        // Have to rotate the rigidbody not the transform
        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot.x, rot.y, rot.z));
    }

    public static void RotateCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<PlayerStats>().Camera.GetComponent<Third_Person_Camera>();

        float rot = cam.VerticalRotation;

        rot -= verticalRotation * Time.deltaTime;
        rot = Mathf.Clamp(rot, -1, 1);

        cam.VerticalRotation = rot;
    }

    public static void Jump(GameObject player)
    {
        //Debug.Log(player.GetComponent<CharacterStats>().Grounded.ToString());
        PlayerStats stats = player.GetComponent<PlayerStats>();

        if (stats.Grounded)
        {
            Rigidbody body = player.GetComponent<Rigidbody>();
            PhysicMaterial material = player.GetComponent<CapsuleCollider>().material;

            if (body.mass < 1000)
            {
                body.mass = 1000;
            }
            material.frictionCombine = PhysicMaterialCombine.Minimum;
            material.staticFriction = 0.0f;
            material.dynamicFriction = 0.0f;
            body.AddForce(new Vector3(0, CharacterStats.JUMP_IMPULSE, 0), ForceMode.Impulse);
            stats.Grounded = false;
        }

    }

    public static void UseAbility1(GameObject player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats.isControllable)
        {
            stats.Abilities[1].Use();
        }
    }

    public static void UseAbility2(GameObject player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats.isControllable)
        {
            stats.Abilities[2].Use();
        }
    }

    public static void UseAbility3(GameObject player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats.isControllable)
        {
            stats.Abilities[3].Use();
        }
    }

    public static void UseAbility4(GameObject player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats.isControllable)
        {
            stats.Abilities[4].Use();
        }
    }

    public static void UseAttackAbility(GameObject player)
    {
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats.isControllable)
        {
            stats.Abilities[0].Use();
        }
    }

    public static void UseRevive(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.Revive.Use();
    }

    public static void Pause(Canvas pauseUI)
    {
        pauseUI.gameObject.SetActive(true);

        foreach (GameObject player in GameManager.playerManager.Players.Values)
        {
            player.GetComponent<PlayerStats>().isControllable = false;
        }

    }

    public static void UnPause(Canvas pauseUI)
    {
        Time.timeScale = 1;
        pauseUI.gameObject.SetActive(false);

        foreach (GameObject player in GameManager.playerManager.Players.Values)
        {
            player.GetComponent<PlayerStats>().isControllable = true;
        }
    }

    public static void KillEnemy(GameObject enemy)
    {
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();

        enemyStats.Game.GetComponent<WaveSpawner>().Enemies.Remove(enemy);
        enemyStats.Game.GetComponent<Game>().SetPointsLeft(enemyStats.Game.GetComponent<Game>().GetPointsLeft() - (int)enemyStats.ScorePointValue);
        enemy.GetComponent<EnemyAI>().Target.GetComponent<PlayerStats>().RemoveFromFollowers(enemyStats);

        Object.Destroy(enemy);
    }

    public static void KillBoss(GameObject boss)
    {
        CharacterStats bossStats = boss.GetComponent<CharacterStats>();
        if (bossStats == null)
        {
            return;
        }
        Game game = bossStats.Game.GetComponent<Game>();

        bossStats.Game.GetComponent<WaveSpawner>().Enemies.Remove(boss);
        game.SetPointsLeft(bossStats.Game.GetComponent<Game>().GetPointsLeft() - (int)bossStats.ScorePointValue);
        game.IsBossDead = true;
        game.DeadTimer += Time.time;
        boss.GetComponent<EnemyAI>().Target.GetComponent<PlayerStats>().RemoveFromFollowers(bossStats);

        Object.Destroy(boss);
    }

    public static void DownPlayer(GameObject player)
    {
        // set isDowned to true
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.isDowned = true;

        playerStats.AddToDowns(1);

        player.GetComponentInChildren<PlayerUI>().RevivePrompt.SetActive(false);

        playerStats.ScorePointsEarned = (int)(playerStats.ScorePointsEarned * 0.5f);

        // untag the player so enemies know not to try and find this player
        player.tag = "DownedPlayer";
        // Change layer for layer masking
        player.layer = LayerMask.NameToLayer("DownedPlayer");

        // create the downed player hitbox
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        playerStats.SetDownedHitbox((GameObject)Object.Instantiate(Resources.Load("DownedPlayerHitbox"), pos, rot));
        playerStats.GetDownedHitbox().GetComponent<DownedPlayerHitbox>().Initialize(playerStats);

        // change my colour for now so I know I have died
        Transform mesh = player.transform.Find("Mesh_Player");
        mesh.GetComponent<Renderer>().material.color = new Color(0.08f, 0.11f, 0.12f, 1);

        // make myself kinematic
        player.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        playerStats.RemoveAllFollowers();
    }

    public static void RevivePlayer(GameObject player, PlayerStats Revivor)
    {
        // set isDowned to false
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.isDowned = false;

        // tag the player as player
        player.tag = "Player";
        // Change layer for layer masking
        player.layer = LayerMask.NameToLayer("AlivePlayer");

        // change my colour back to normal
        Transform mesh = player.transform.Find("Mesh_Player");
        mesh.GetComponent<Renderer>().material = playerStats.PlayerMaterial;

        // Give me my health and shield back
        playerStats.Health = playerStats.MaxHealth;
        playerStats.Shield = playerStats.MaxShield;

        // make myself unkinematic
        player.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        Revivor.AddToRevives(1);
    }
}
