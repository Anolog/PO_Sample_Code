using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportClone : Hitbox
{

    public override void Initialize(CharacterStats attacker, Ability.AbilityType type, int abilityDamage, float lifetime )
    {
        base.Initialize(attacker, type, abilityDamage, lifetime);

    }
}
