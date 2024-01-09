using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class LightningGun : Block
{
    public int Accuracy = 150;
    public float BonusRatio = 30f;

    private float Cooldown = 0f;

    new public bool Turret = true;

    public void ChainLightningDamage(int index, Block target, Vector2 point)
    {
        Block block = target.vehicle.blockGrid[index];
        if (block != null)
        {
            block.Damage(GetExtraVariable("Damage"), 1f, false, this);
            if (!block.ShockImmunity)
            {
                block.Shocked = true;
                block.ShockedTimer = Mathf.Max(1f, block.ShockedTimer);
            }

            GameObject lightning = Instantiate(EffectObjects.instance.SmallLightning, point, Quaternion.identity);
            lightning.GetComponent<Lightning>().EndPoint = block.transform.position;
        }
    }

    public Vector2 rotate(Vector2 v, float delta)
    {
        delta = delta * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public Vector3 rotate2(Vector3 v, float delta)
    {
        delta = delta * Mathf.Deg2Rad;
        return new Vector3(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta),
            v.z
        );
    }

    private int IntToX(int i)
    {
        return i % vehicle.gridSize.x;
    }

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= 1f && vehicle.Energy >= GetExtraVariable("Energy") && !Shocked)
        {
            GameObject lightning = Instantiate(EffectObjects.instance.Lightning, transform.position + (Attachment.transform.right * 0.6f), Quaternion.identity);

            float inaccuracy = Random.Range(0 - Accuracy, Accuracy) * 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rotate(Attachment.transform.right, inaccuracy), GetExtraVariable("Range"),
                LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

            if (hit.collider != null)
            {
                Block hitBlock = hit.collider.GetComponent<Block>();
                if (hitBlock != null)
                {
                    if (hitBlock.Shocked)
                    {
                        int index = hitBlock.vehicle.blockGrid.IndexOf(hitBlock);

                        if (IntToX(index) + 1 < vehicle.gridSize.x)
                            ChainLightningDamage(index + 1, hitBlock, hit.point);
                        if (IntToX(index) - 1 >= 0)
                            ChainLightningDamage(index - 1, hitBlock, hit.point);
                        if (index + vehicle.gridSize.x < vehicle.gridSize.x * vehicle.gridSize.y)
                            ChainLightningDamage(index + vehicle.gridSize.x, hitBlock, hit.point);
                        if (index - vehicle.gridSize.x >= 0)
                            ChainLightningDamage(index - vehicle.gridSize.x, hitBlock, hit.point);
                    }

                    hitBlock.Damage(GetExtraVariable("Damage"), 1f, false, this);
                    
                    if (!hitBlock.ShockImmunity)
                    {
                        hitBlock.Shocked = true;
                        hitBlock.ShockedTimer = 1f;
                    }
                }
                lightning.GetComponent<Lightning>().EndPoint = hit.point;
            }
            else
                lightning.GetComponent<Lightning>().EndPoint = transform.position + (rotate2(Attachment.transform.right, inaccuracy) * (GetExtraVariable("Range") + 0.1f));

            vehicle.Energy -= GetExtraVariable("Energy");

            Cooldown -= 1f;
        }
        else
        {
            if (Cooldown < 1f)
                Cooldown += (GetExtraVariable("AtkSpd") + AtkSpdBonus()) * Time.deltaTime;
        }
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

    public float AtkSpdBonus()
    {
        return vehicle.Energy / BonusRatio;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        RotateAttachment();
        Shoot();
    }
}
