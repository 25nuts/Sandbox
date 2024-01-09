using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public VehicleController vehicle;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer Attachment;
    public Collider2D BlockCollider;

    public float MaxHP;
    public float Heat;

    public float DamageFlash;

    public string BlockName = "Empty";
    public Sprite BlockSprite;
    public bool Core;

    public float HP = 100f;
    public float Defence;
    public float Weight = 1f;

    public float HeatConductivity = 0.1f;
    public float MeltingPoint = 100f;

    public int Button;
    public bool ReverseP2;

    public bool Rotatable;
    public bool Directional;
    public Sprite ExtraSprite;
    public bool AttachmentEnabled;
    public float Angle;

    public List<string> ExtraVariableNames = new List<string>();
    public List<float> ExtraVariables = new List<float>();

    public bool Shocked;
    public float ShockedTimer;
    public ParticleSystem ShockedParticleEffect;

    [HideInInspector]
    public bool Turret = false;
    [HideInInspector]
    public bool ShockImmunity = false;

    private bool Felloff = false;

    public virtual void AddCollider()
    {
        BlockCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Attachment = transform.GetChild(0).GetComponent<SpriteRenderer>();

        AddCollider();
    }

    public virtual float GetExtraVariable(string variableName)
    {
        if (!ExtraVariableNames.Contains(variableName))
            return 0f;
        return ExtraVariables[ExtraVariableNames.IndexOf(variableName)];
    }


    public virtual void DeleteBlock()
    {
        Destroy(this.gameObject);
    }

    public virtual void BreakBlock()
    {
        Instantiate(EffectObjects.instance.Shockwave, transform.position, Quaternion.identity);

        if (vehicle.blocks.Contains(this))
            vehicle.blocks.Remove(this);
        if (vehicle.blockGrid.Contains(this))
            vehicle.blockGrid[vehicle.blockGrid.IndexOf(this)] = null;

        if (Core)
        {
            vehicle.DeleteVehicle();
            return;
        }

        vehicle.RemoveUnconnectedBlocks();
        DeleteBlock();
    }

    public virtual void Falloff(bool delete)
    {
        if (!Felloff)
        {
            GameObject brokenBlock = Instantiate(EffectObjects.instance.BrokenBlock, transform.position, transform.rotation);
            brokenBlock.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;

            GameObject brokenBlockAttachment = brokenBlock.transform.GetChild(0).gameObject;
            brokenBlockAttachment.GetComponent<SpriteRenderer>().sprite = Attachment.sprite;
            brokenBlockAttachment.transform.rotation = Attachment.transform.rotation;
            brokenBlockAttachment.GetComponent<SpriteRenderer>().flipY = Attachment.flipY;

            brokenBlock.GetComponent<BrokenBlock>().Heat = Heat;
            brokenBlock.GetComponent<BrokenBlock>().HeatConductivity = HeatConductivity;
            brokenBlock.GetComponent<BrokenBlock>().MeltingPoint = MeltingPoint;

            Felloff = true;
        }

        if (delete)
        {
            if (vehicle.blocks.Contains(this))
                vehicle.blocks.Remove(this);
            if (vehicle.blockGrid.Contains(this))
                vehicle.blockGrid[vehicle.blockGrid.IndexOf(this)] = null;

            DeleteBlock();
        }
    }

    public virtual float TrueDamage(float damage, float defencePercent)
    {
        return damage - (Defence * defencePercent);
    }

    public virtual void DamageEffects(float trueDamage)
    {

    }

    public virtual void Damage(float damage, float defencePercent)
    {
        float trueDamage = TrueDamage(damage, defencePercent);
        HP -= Mathf.Max(trueDamage, 0f);
        if (trueDamage > 0f && DamageFlash == 0)
            DamageFlash = 1f;
        DamageEffects(trueDamage);
        if (HP <= 0f)
        {
            BreakBlock();
        }
    }

    public virtual void Damage(float damage, float defencePercent, bool removeFlash)
    {
        float trueDamage = TrueDamage(damage, defencePercent);
        HP -= Mathf.Max(trueDamage, 0f);
        if (trueDamage > 0f && DamageFlash == 0 && !(removeFlash))
            DamageFlash = 1f;
        DamageEffects(trueDamage);
        if (HP <= 0f)
        {
            BreakBlock();
        }
    }

    public virtual void Damage(float damage, float defencePercent, bool removeFlash, Block Attacker)
    {
        float trueDamage = TrueDamage(damage, defencePercent);
        HP -= Mathf.Max(trueDamage, 0f);
        if (trueDamage > 0f && DamageFlash == 0 && !(removeFlash))
            DamageFlash = 1f;
        DamageEffects(trueDamage);
        if (HP <= 0f)
        {
            BreakBlock();
        }
    }

    public virtual void ReversePlayer2Angle()
    {
            Angle *= -1f;
    }


    public void SetBlock(BlockData block, VehicleController vehicleController)
    {
        vehicle = vehicleController;

        BlockName = block.BlockName;
        BlockSprite = block.BlockSprite;
        Core = block.Core;

        HP = block.HP;
        Defence = block.Defence;
        Weight = block.Weight;

        HeatConductivity = block.HeatConductivity;
        MeltingPoint = block.MeltingPoint;

        Button = block.Button;
        ReverseP2 = block.ReverseP2;

        Rotatable = block.Rotatable;
        Directional = block.Directional;
        ExtraSprite = block.ExtraSprite;
        AttachmentEnabled = (ExtraSprite != null);
        Angle = block.Angle;
        if (vehicle.PlayerIndex == 2)
            ReversePlayer2Angle();

        ExtraVariableNames.Clear();
        for (int i = 0; i < block.ExtraVariableNames.Count; i++)
        {
            ExtraVariableNames.Add(block.ExtraVariableNames[i]);
        }

        ExtraVariables.Clear();
        for (int i = 0; i < block.ExtraVariables.Count; i++)
        {
            ExtraVariables.Add(block.ExtraVariables[i]);
        }

        spriteRenderer.sprite = BlockSprite;
    }

    public virtual void SetHeatEffect()
    {
        spriteRenderer.material.SetFloat("Heat", Heat);
        spriteRenderer.material.SetFloat("MeltingPoint", MeltingPoint);
        Attachment.GetComponent<SpriteRenderer>().material.SetFloat("Heat", Heat);
        Attachment.GetComponent<SpriteRenderer>().material.SetFloat("MeltingPoint", MeltingPoint);
    }

    public virtual void SetDamageFlashEffect()
    {
        spriteRenderer.material.SetFloat("Flash", DamageFlash);
        Attachment.GetComponent<SpriteRenderer>().material.SetFloat("Flash", DamageFlash);
        DamageFlash = Mathf.Max(DamageFlash - (Time.deltaTime * 10f), 0f);
    }

    public virtual void HeatDamage()
    {
        if (Heat > MeltingPoint)
        {
            Damage((Heat - MeltingPoint) * Time.deltaTime, 0f, true);
        }
    }

    public virtual void ShockedEffect()
    {
        if (ShockedTimer > 0f)
        {
            ShockedTimer -= Time.deltaTime;
            ShockedTimer = Mathf.Max(ShockedTimer, 0f);

            if (ShockedParticleEffect == null)
            {
                GameObject particleEffect = Instantiate(EffectObjects.instance.ShockedParticleEffect, transform.position, Quaternion.identity);
                particleEffect.transform.parent = transform;
                ShockedParticleEffect = particleEffect.GetComponent<ParticleSystem>();
            }

            if (!ShockedParticleEffect.isPlaying)
                ShockedParticleEffect.Play();

            Shocked = true;
        }
        else
        {
            if (ShockedParticleEffect != null)
            {
                ShockedParticleEffect.Stop();
            }

            Shocked = false;
        }
    }

    public virtual void Start()
    {
        MaxHP = HP;

        if (AttachmentEnabled)
        {
            Attachment.sprite = ExtraSprite;
            Attachment.transform.eulerAngles = Vector3.forward * Angle * -1f;
        }
        if (Rotatable)
        {
            transform.eulerAngles = Vector3.forward * Angle * -1f;
        }
        else
        {
            transform.eulerAngles = Vector3.forward * 0f;
        }
    }

    public virtual void Update()
    {
        SetHeatEffect();
        SetDamageFlashEffect();

        HeatDamage();
        ShockedEffect();
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude - 15f > 0f && collision.collider.gameObject.tag != "Projectile")
        {
            Damage((collision.relativeVelocity.magnitude - 15f) * 2f, 2f);
        }
    }
}
