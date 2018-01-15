using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBombProjectile : Hitbox
{

    private GameObject Hitbox;
    public float ExplosionDamage = 10;
    public float DistancetoExplode = 3;
    public float ExplosionLifetime = 0.5f;
    public float TimeLeftUntilExplosion = 3.0f;

    public override void Initialize(CharacterStats attacker, Ability.AbilityType type, int abilityDamage, float lifeTime)
    {
        base.Initialize(attacker, type, abilityDamage, lifeTime);
        ExplosionDamage += attacker.Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterStats>() != Attacker.gameObject.GetComponent<CharacterStats>())
        {
            Hitbox = (GameObject)Instantiate(Resources.Load("DamageHitboxes/EnergyBombHitbox"), gameObject.transform.position, gameObject.transform.rotation);
            Hitbox.GetComponent<Hitbox>().Initialize(Attacker, Type, (int)ExplosionDamage, ExplosionLifetime);
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        return;
    }

}
