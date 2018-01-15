using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyMineHitbox : Hitbox
{
/*
    private float lifeTime = 0.5f;

    protected void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
            DestroyImmediate(gameObject);
    }
    */
    protected override void OnTriggerEnter(Collider other)
    {
        //Remember to change this for when it is done testing as the player
        if (other.gameObject.tag == "Player" && Attacker.gameObject.tag == "Boss")
        {
            if (other.gameObject.GetComponent<CharacterStats>() != null)
            {
                RaycastHit hit;
                Vector3 dir = other.gameObject.transform.position - gameObject.transform.position;
                Ray ray = new Ray(gameObject.transform.position, dir);
                if(Physics.Raycast(ray, out hit, 2))
                {
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(Attacker, Type, AbilityDamage);
                }
            }
        }  
    }
}
