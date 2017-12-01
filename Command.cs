using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Command {

    //Pending implementation of character object
    //Character* m_Character;
    public static void MoveTowards(GameObject player, Vector3 targetPosition)
    {
        // FOR AI
    }

    public static void Move(GameObject player, Vector2 direction)
    {
        float maxSpeed = CharacterStats.DEFAULT_MOVEMENT_SPEED * player.GetComponent<CharacterStats>().MovementSpeed;

        if (direction.y < -Mathf.Epsilon)
            maxSpeed *= 0.5f;

        // Changed this to new GDD formula
        Vector3 currentPosition = player.GetComponent<Rigidbody>().position; // need to move the rigidbody, not the transform
        float currentMovementSpeed = player.GetComponent<CharacterStats>().CurrentMovementSpeed;
        currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, maxSpeed, CharacterStats.ACCELERATION_SPEED);
        player.GetComponent<Rigidbody>().MovePosition(currentPosition + (direction.x * player.transform.right + direction.y * player.transform.forward) * currentMovementSpeed * Time.deltaTime);
    }

    public static void RotateTowards(GameObject player, Quaternion targetRotation)
    {
        // FOR AI
    }

    public static void RotateConstSpeedHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.transform.eulerAngles;

        rot.y += horizontalRotation * player.GetComponent<CharacterStats>().AngularSpeed * Time.deltaTime;

        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot.x, rot.y, rot.z));
    }

    public static void RotatePlayerCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<CharacterStats>().Camera.GetComponent<Third_Person_Camera>();

        float rot = cam.VerticalRotation;

        rot += verticalRotation * player.GetComponent<CharacterStats>().AngularSpeed * Time.deltaTime;
        rot = Mathf.Clamp(rot, -1, 1);

        cam.VerticalRotation = rot;
    }

    public static void RotateHorizontal(GameObject player, float horizontalRotation)
    {
        Vector3 rot = player.GetComponent<Rigidbody>().rotation.eulerAngles;

        rot.y -= horizontalRotation;

        // Have to rotate the rigidbody not the transform
        player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(rot.x, rot.y, rot.z));
    }

    public static void RotateCameraVertical(GameObject player, float verticalRotation)
    {
        Third_Person_Camera cam = player.GetComponent<CharacterStats>().Camera.GetComponent<Third_Person_Camera>();

        float rot = cam.VerticalRotation;

        rot -= verticalRotation;
        rot = Mathf.Clamp(rot, -1, 1);

        cam.VerticalRotation = rot;
    }

    public static void Jump(GameObject player)
    {
        player.GetComponent<Rigidbody>().AddForce(new Vector3(0, CharacterStats.JUMP_IMPULSE, 0), ForceMode.Impulse);
    }

    public static void UseAbility1(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[1].Use();
    }

    public static void UseAbility2(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[2].Use();
    }

    public static void UseAbility3(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[3].Use();
    }

    public static void UseAbility4(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[4].Use();
    }

    public static void UseAttackAbility(GameObject player)
    {
        player.GetComponent<CharacterStats>().Abilities[0].Use();
    }

    public static void KillEnemy(GameObject enemy)
    {
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        enemyStats.Game.GetComponent<WaveSpawner>().Enemies.Remove(enemy);
        enemyStats.Game.GetComponent<Game>().SetPointsLeft(enemyStats.Game.GetComponent<Game>().GetPointsLeft() - (int)enemyStats.ScorePointValue);

        enemy.GetComponent<EnemyAI>().Target.GetComponent<CharacterStats>().RemoveFromFollowers(enemyStats);

        Object.Destroy(enemy);
    }

    public static void DownPlayer(GameObject player)
    {
        // set isDowned to true
        CharacterStats playerStats = player.GetComponent<CharacterStats>();
        playerStats.isDowned = true;

        // untag the player so enemies know not to try and find this player
        player.tag = "DownedPlayer";
        // tell all of the player followers to find new targets
        foreach (CharacterStats follower in playerStats.Followers)
        {
            follower.gameObject.GetComponent<EnemyAI>().FindTarget();
        }

        // Change layer for layer masking
        player.layer = LayerMask.NameToLayer("DownedPlayer");

        // create the downed player hitbox
        Vector3 pos = player.transform.position;
        Quaternion rot = player.transform.rotation;
        Object.Instantiate(Resources.Load("DownedPlayerHitbox"), pos, rot);

        // change my colour for now so I know I have died
        Transform mesh = player.transform.Find("Mesh_Player");
        mesh.GetComponent<Renderer>().material.color = Color.yellow;
    }
}
