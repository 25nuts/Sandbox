using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : Block
{
    public GameObject LaserEffect;
    public LineRenderer LaserEffectRenderer;
    public GameObject LaserChargeEffect;
    public ParticleSystem LaserChargeEffectSystem;

    public float ChargeTime = 2f;
    public float Charge = 0f;

    new public bool Turret = true;

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
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && vehicle.Energy >= 1f && !Shocked)
        {
            if (Charge >= ChargeTime)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Attachment.transform.right, GetExtraVariable("Range"),
                    LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

                if (hit.collider != null)
                {
                    Block hitBlock = hit.collider.GetComponent<Block>();
                    if (hitBlock != null)
                    {
                        hitBlock.Damage(GetExtraVariable("DPS") * Time.deltaTime, Time.deltaTime, false, this);
                        hitBlock.Heat += GetExtraVariable("HeatDamage") * Time.deltaTime;
                    }
                    LaserEffectRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    LaserEffectRenderer.SetPosition(1, transform.position + (Attachment.transform.right * (GetExtraVariable("Range") + 0.1f)));
                }
                LaserEffect.SetActive(true);

                Heat += GetExtraVariable("Heat") * Time.deltaTime;
            }
            else
            {
                Charge += Time.deltaTime;
                LaserEffect.SetActive(false);
                if (!LaserChargeEffectSystem.isPlaying)
                    LaserChargeEffectSystem.Play();
            }

            vehicle.Energy -= GetExtraVariable("Energy") * Time.deltaTime;
            vehicle.Energy = Mathf.Max(vehicle.Energy, 0);
        }
        else
        {
            Charge = 0f;
            LaserEffect.SetActive(false);
            LaserChargeEffectSystem.Clear();
            LaserChargeEffectSystem.Stop();
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

    public void SetLaserEffect()
    {
        LaserEffectRenderer.SetPosition(0, LaserEffect.transform.position);
    }

    public override void Start()
    {
        base.Start();

        LaserEffect = GameObject.Instantiate(EffectObjects.instance.Laser, transform.position + (Attachment.transform.right * 0.3f), Attachment.transform.rotation);
        LaserEffect.transform.parent = Attachment.transform;
        LaserEffectRenderer = LaserEffect.GetComponent<LineRenderer>();
        LaserChargeEffect = GameObject.Instantiate(EffectObjects.instance.LaserCharge, transform.position + (Attachment.transform.right * 0.3f), Attachment.transform.rotation);
        LaserChargeEffect.transform.parent = Attachment.transform;
        LaserChargeEffectSystem = LaserChargeEffect.GetComponent<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        RotateAttachment();
        Shoot();
        SetLaserEffect();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Attachment.transform.right * GetExtraVariable("Range")));
    }
}
