using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeball : Block
{
    public float SwingSpeed = 1f;
    public float AtkSpd = 5f;

    private float Cooldown;
    private bool BlockHit;

    public void SwingSpikeball()
    {
        if (vehicle.PlayerIndex == 2)
            Angle += SwingSpeed * -360f * Time.deltaTime;
        else
            Angle += SwingSpeed * 360f * Time.deltaTime;

        if (Angle > 180f)
            Angle -= 360f;
        if (Angle <= -180f)
            Angle += 360f;
    }

    public override void ReversePlayer2Angle()
    {
        Angle += 90f;
        Angle *= -1f;
        Angle -= 90f;
    }

    public void RotateAttachment()
    {
        if (Angle < -90f || Angle > 90f)
            Attachment.flipY = true;
        else
            Attachment.flipY = false;

        Attachment.transform.localEulerAngles = Vector3.forward * Angle * -1f;
    }

    public void SpikeballCollision()
    {
        if (Cooldown >= 1f)
        {
            BlockHit = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Attachment.transform.right, 0.5f, LayerMask.GetMask(vehicle.EnemyLayer));

            foreach (Collider2D collider in colliders)
            {
                Block block = collider.GetComponent<Block>();
                if (block != null)
                {
                    block.Damage(GetExtraVariable("Damage"), 1f, false, this);
                    BlockHit = true;
                    break;
                }
            }

            if (BlockHit)
                Cooldown -= 1f;
        }
        else
        {
            if (Cooldown < 1f)
                Cooldown += AtkSpd * Time.deltaTime;
        }
    }

    public override void Update()
    {
        base.Update();

        SwingSpikeball();
        RotateAttachment();
        SpikeballCollision();
    }
}
