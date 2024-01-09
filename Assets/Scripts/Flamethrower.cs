using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UIElements;

public class Flamethrower : Block
{
    public GameObject FlameEffect;
    public ParticleSystem FlameEffectSystem;

    public int RaycastCount = 7;
    public float Spread = 6f;

    new public bool Turret = true;

    public void HeatRay(Vector2 Direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, GetExtraVariable("Range"),
            LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

        if (hit.collider != null)
        {
            Block hitBlock = hit.collider.GetComponent<Block>();
            if (hitBlock != null)
            {
                hitBlock.Heat += (GetExtraVariable("HeatDamage") / RaycastCount) * Time.deltaTime;
            }
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

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false))
        {
            float currentRayDirection = Spread;
            for (int i = 0; i < RaycastCount; i++)
            {
                HeatRay(rotate(Attachment.transform.right, currentRayDirection));
                currentRayDirection -= (Spread / (RaycastCount - 1)) * 2f;
            }

            Heat += GetExtraVariable("Heat") * Time.deltaTime;
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

    public void SetFlameEffect()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false))
        {
            if (FlameEffectSystem.isStopped)
                FlameEffectSystem.Play();
        }
        else
            FlameEffectSystem.Stop();
    }

    public override void Start()
    {
        base.Start();

        FlameEffect = GameObject.Instantiate(EffectObjects.instance.FlameJet, transform.position + (Attachment.transform.right * 1.15f), Attachment.transform.rotation);
        FlameEffect.transform.parent = Attachment.transform;
        FlameEffectSystem = FlameEffect.GetComponent<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        RotateAttachment();
        Shoot();
        SetFlameEffect();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float currentRayDirection = Spread;
        for (int i = 0; i < RaycastCount; i++)
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (rotate(Attachment.transform.right, currentRayDirection) * GetExtraVariable("Range")));
            currentRayDirection -= (Spread / (RaycastCount - 1)) * 2f;
        }
    }
}
