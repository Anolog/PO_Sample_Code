using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBombHitbox : Hitbox
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && Attacker.gameObject.tag == "Player" ||
            other.gameObject.tag == "Boss" && Attacker.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<CharacterStats>() != null)
            {
                RaycastHit hit;
                Vector3 dir = other.gameObject.transform.position - gameObject.transform.position;
                Ray ray = new Ray(gameObject.transform.position, dir);
                if (Physics.Raycast(ray, out hit, 2))
                {
                    if (hit.transform == other.gameObject.transform)
                    {
                        other.gameObject.GetComponent<CharacterStats>().TakeDamage(Attacker, Type, AbilityDamage);
                    }
                }
            }
        }
    }
}
