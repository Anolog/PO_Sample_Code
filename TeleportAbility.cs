using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAbility : Ability
{
    public static float RECOVERY_TIME = 0.15f;
    public static float CAST_TIME = 0.5f;
    public static float BASE_DAMAGE = 0.0f;
    public static float LIFE_TIME = 1.5f;
    private static float TIME_LEFT_TO_SWAP = LIFE_TIME * 0.5f;
    public static float COOL_DOWN_TIME = LIFE_TIME;

    private float m_TimeLeftToSwap = TIME_LEFT_TO_SWAP;
    public GameObject TeleporLocation;
    private bool m_CanTeleport = false;
    private bool m_HasSwapped = true;

    private Vector3 m_LocationToTeleportTo = Vector3.zero;

    private GameObject m_teleportClone;

    public TeleportAbility(CharacterStats character)
        : base(character)
    {
        m_Type = AbilityType.Ranged;
        m_CastTime = CAST_TIME;
        m_RecoveryTime = RECOVERY_TIME;
        m_CoolDownTime = COOL_DOWN_TIME;
        m_BaseDamage = BASE_DAMAGE;

        //No damage since it's just a TP.
        Damage = m_BaseDamage;

        //Lifetime is used for the clone
        m_Lifetime = LIFE_TIME;
    }

    public override void Use(GameObject teleportLocation)
    {
        TeleporLocation = teleportLocation;

        base.Use();
    }

    protected override void ActivateEffect()
    {
        //TeleporLocation = m_Character.gameObject.GetComponent<Boss1AI>().Target;

        RaycastHit hit;

        Vector3 dir = -TeleporLocation.transform.forward;
        Ray ray = new Ray(TeleporLocation.transform.position, dir);

        if (Physics.Raycast(ray, out hit, 1.5f, LayerMask.NameToLayer("AlivePlayer")))
        {
            m_CanTeleport = false;
        }

        else
        {
            m_CanTeleport = true;
        }

        if (m_CanTeleport == true)
        {

            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            //Create the clone
            m_teleportClone = (GameObject)Object.Instantiate(Resources.Load("Teleport/TeleportClonePrefab"), pos, rot);
            m_teleportClone.GetComponent<Hitbox>().Initialize(m_Character, m_Type, (int)Damage, m_Lifetime);

            m_LocationToTeleportTo = new Vector3(-0.5f * TeleporLocation.transform.forward.x, 0.5f, -0.5f * TeleporLocation.transform.forward.z);

            //Move clone to the teleport location
            m_teleportClone.transform.position = TeleporLocation.transform.position + m_LocationToTeleportTo;

            //Set back so it doesn't create new gameobject.
            //m_CanTeleport = false;
            m_HasSwapped = false;
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_CanTeleport)
        {
            m_TimeLeftToSwap -= Time.deltaTime;
        }

        if (m_TimeLeftToSwap <= 0.0f && m_HasSwapped == false)
        {

            //Grab a few values before swap
            Vector3 preSwapLoc = m_Character.transform.position;
            Quaternion preSwapRot = m_Character.transform.rotation;

            //Move the boss to the teleport location
            m_Character.transform.position = m_teleportClone.transform.position;
            m_Character.transform.rotation = m_teleportClone.transform.rotation;

            //Move the clone to the boss location before swap
            m_teleportClone.transform.position = preSwapLoc;
            m_teleportClone.transform.rotation = preSwapRot; 


            m_HasSwapped = true;
            m_TimeLeftToSwap = TIME_LEFT_TO_SWAP;

            m_CanTeleport = false;

            //Object.DestroyObject(m_teleportClone);
        }
    }

    public override void ResetCooldown()
    {
        m_CoolDownTime = COOL_DOWN_TIME;
    }
}
