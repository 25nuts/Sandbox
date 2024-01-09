using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningArmor : Block
{
    public override void DamageEffects(float trueDamage)
    {
        if (trueDamage > 0f && vehicle.Energy < vehicle.EnergyCapacity)
        {
            vehicle.Energy += GetExtraVariable("Recharge");
            vehicle.Energy = Mathf.Min(vehicle.Energy, vehicle.EnergyCapacity);
        }
    }

    public override void Damage(float damage, float defencePercent, bool removeFlash, Block Attacker)
    {
        float trueDamage = damage - (Defence * defencePercent);
        HP -= Mathf.Max(trueDamage, 0f);
        if (trueDamage > 0f && DamageFlash == 0 && !(removeFlash))
            DamageFlash = 1f;

        if (trueDamage > 0f && !Attacker.ShockImmunity)
        {
            Attacker.Shocked = true;
            Attacker.ShockedTimer = Mathf.Max(2f, Attacker.ShockedTimer);
        }

        DamageEffects(trueDamage);

        if (HP <= 0f)
        {
            BreakBlock();
        }
    }

    public override void Start()
    {
        base.Start();

        ShockImmunity = true;
    }
}
