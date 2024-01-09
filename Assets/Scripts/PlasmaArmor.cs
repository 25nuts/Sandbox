using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaArmor : Block
{
    public GameObject PlasmaShield;
    public float Shields;
    private float Cooldown;

    public override void Start()
    {
        base.Start();

        PlasmaShield = Instantiate(EffectObjects.instance.PlasmaShield, transform.position, transform.rotation);
        PlasmaShield.transform.parent = transform;
    }

    public override float TrueDamage(float damage, float defencePercent)
    {
        if (Shields > 0f)
        {
            Shields--;
            Instantiate(EffectObjects.instance.SmallShockwave, transform.position, Quaternion.identity);
            return 0f;
        }
        else
            return base.TrueDamage(damage, defencePercent);
    }

    public override void Update()
    {
        base.Update();

        if (Cooldown <= 0f)
        {
            if (Shields < 1f && vehicle.Plasma > 0f)
            {
                Shields += GetExtraVariable("Shields");

                vehicle.Plasma -= 1f;
                Cooldown += GetExtraVariable("Cooldown");
            }
        }
        else
        {
            Cooldown -= Time.deltaTime;
        }

        PlasmaShield.SetActive(Shields > 0f ? true : false);
    }
}
