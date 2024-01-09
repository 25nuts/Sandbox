using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingArmor : Block
{
    public GameObject HealEffect;
    public ParticleSystem HealEffectSystem;

    public void Heal()
    {
        if (HP < MaxHP && vehicle.Energy > GetExtraVariable("Energy") * Time.deltaTime && !Shocked)
        {
            HP += GetExtraVariable("Healing") * Time.deltaTime;
            HP = Mathf.Min(HP, MaxHP);
            vehicle.Energy -= GetExtraVariable("Energy") * Time.deltaTime;

            if (!HealEffectSystem.isPlaying)
                HealEffectSystem.Play();
        }
        else
            HealEffectSystem.Stop();
    }

    public override void Start()
    {
        base.Start();

        HealEffect = GameObject.Instantiate(EffectObjects.instance.HealEffect, transform.position, Attachment.transform.rotation);
        HealEffect.transform.parent = Attachment.transform;
        HealEffectSystem = HealEffect.GetComponent<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        Heal();
    }
}
