using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Block
{
    public float TurnSpeed = 90f;

    public GameObject BulletEffect;
    public ParticleSystem BulletEffectSystem;

    private float Cooldown = 0f;

    private Vector3 Target;
    private bool TargetEnabled;

    public void SetTarget()
    {
        if (vehicle.PlayerIndex == 2)
        {
            Target = CorePositions.instance.Core1;
            TargetEnabled = CorePositions.instance.Core1Enabled;
        }
        else
        {
            Target = CorePositions.instance.Core2;
            TargetEnabled = CorePositions.instance.Core2Enabled;
        }
    }

    public void RotateTurret()
    {
        if (TargetEnabled)
        {
            Attachment.transform.right = Vector3.RotateTowards(Attachment.transform.right, Target - Attachment.transform.position,
                TurnSpeed * Time.deltaTime, 0f);

            if (Attachment.transform.up.y < 0f)
                Attachment.flipY = true;
            else
                Attachment.flipY = false;

            Angle = Mathf.Rad2Deg * Mathf.Atan2(Attachment.transform.right.y, Attachment.transform.right.x);
        }
    }

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= 1f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Attachment.transform.right, GetExtraVariable("Range"),
                LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

            if (hit.collider != null)
            {
                Block hitBlock = hit.collider.GetComponent<Block>();
                if (hitBlock != null)
                {
                    hitBlock.Damage(GetExtraVariable("Damage"), 1f, false, this);
                }
            }

            Cooldown -= 1f;
        }
        else
        {
            if (Cooldown < 1f)
                Cooldown += GetExtraVariable("AtkSpd") * Time.deltaTime;
        }
    }

    public void SetBulletEffect()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false))
        {
            if (BulletEffectSystem.isStopped)
                BulletEffectSystem.Play();
        }
        else
            BulletEffectSystem.Stop();
    }

    public override void Start()
    {
        base.Start();

        BulletEffect = GameObject.Instantiate(EffectObjects.instance.MinigunBullets, transform.position + new Vector3(0.6f, 0f, 0f), Quaternion.identity);
        BulletEffect.transform.parent = Attachment.transform;
        BulletEffectSystem = BulletEffect.GetComponent<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        SetTarget();
        RotateTurret();
        Shoot();
        SetBulletEffect();
    }
}
