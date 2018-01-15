﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBombAbility : Ability {

    public static float RECOVERY_TIME = 0.15f;
    public static float CAST_TIME = 0.1f;
    public static float COOL_DOWN_TIME = 10f;
    public static float BASE_DAMAGE = 2;
    public static float LIFE_TIME = 5f;
    public static string ABILITY_NAME = "Energy Bomb";

    public GameObject Bomb;

    private static float MIN_DISTANCE = 0.5f;
    private static float MAX_DISTANCE = 20f;
    const float LAUNCH_ANGLE = 25.0f;

    public EnergyBombAbility(CharacterStats character)
        : base(character)
    {
        m_Type = AbilityType.Ranged;
        m_CastTime = CAST_TIME;
        m_RecoveryTime = RECOVERY_TIME;
        m_CoolDownTime = COOL_DOWN_TIME;
        m_BaseDamage = BASE_DAMAGE;
        AbilityName = ABILITY_NAME;

        Damage = m_BaseDamage + m_Character.Damage;

        m_Lifetime = LIFE_TIME;
    }

    protected override void ActivateEffect()
    {
        // determine a position over my shoulder to start the throw from
        Vector3 pos = m_Character.transform.position;
        pos.y += 1.6f;
        pos.x += m_Character.transform.forward.x * 0.5f;
        pos.z += m_Character.transform.forward.z * 0.5f;
        Quaternion rot = m_Character.transform.rotation;

        Bomb = (GameObject)Object.Instantiate(Resources.Load("EnergyBomb/EnergyBomb"), pos, rot);
        Bomb.GetComponent<EnergyBombProjectile>().Initialize(m_Character, m_Type, (int)Damage, m_Lifetime);

        // get vertical rotation from third person camera (-1 to 1 value)
        float verticalRotation = m_Character.gameObject.GetComponentInChildren<Third_Person_Camera>().VerticalRotation;
        // normalize this value (0 to 1 value)
        verticalRotation += 1f; // makes it from 0 to 2
        verticalRotation *= 0.5f; // makes it from 0 to 1
        // use this value to lerp between my min throw distance, and my max throw distance (also determine these)
        float horizontalDist = Mathf.Lerp(MIN_DISTANCE, MAX_DISTANCE, verticalRotation);
        float verticalDist = 0f;
        // copy rock thrower code to determine the force I need to throw at
        float g = -Physics.gravity.y;
        float xSqr = Mathf.Pow(horizontalDist, 2);
        float sin2Angle = Mathf.Sin(LAUNCH_ANGLE * 2 * Mathf.Deg2Rad);
        float cosSqrAngle = Mathf.Cos(LAUNCH_ANGLE * Mathf.Deg2Rad);

        float speed = Mathf.Sqrt((xSqr * g) / (horizontalDist * sin2Angle - 2 * verticalDist * cosSqrAngle));

        Vector2 direction2D = new Vector2(m_Character.transform.forward.x, m_Character.transform.forward.z);
        Vector3 xForce = Mathf.Cos(LAUNCH_ANGLE * Mathf.Deg2Rad) * speed * Bomb.GetComponent<Rigidbody>().mass * Vector3.Normalize(new Vector3(direction2D.x, 0, direction2D.y));
        Vector3 yForce = Mathf.Sin(LAUNCH_ANGLE * Mathf.Deg2Rad) * speed * Bomb.GetComponent<Rigidbody>().mass * Vector3.up;

        // use this:
        Bomb.GetComponent<Rigidbody>().AddForce(xForce + yForce, ForceMode.Impulse);
    }

    public override void ResetCooldown()
    {
        m_CoolDownTime = COOL_DOWN_TIME;
    }
}
