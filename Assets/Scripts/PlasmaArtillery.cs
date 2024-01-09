using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaArtillery : Block
{
    public float ExplosionRadius = 1.6f;

    private float Cooldown = 0f;

    new public bool Turret = true;

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && vehicle.Plasma > 0f && Cooldown >= 1f)
        {
            GameObject plasmaBomb = Instantiate(EffectObjects.instance.PlasmaBomb, transform.position + (Attachment.transform.right * 0.6f), Quaternion.identity);
            if (LayerMask.LayerToName(gameObject.layer) == "Team2")
                plasmaBomb.gameObject.layer = LayerMask.NameToLayer("Projectile2");
            else
                plasmaBomb.gameObject.layer = LayerMask.NameToLayer("Projectile1");

            plasmaBomb.GetComponent<Rigidbody2D>().velocity = vehicle.VehicleRigidBody.velocity;
            plasmaBomb.GetComponent<PlasmaBomb>().Force = GetExtraVariable("Range") * 0.3f;
            plasmaBomb.GetComponent<PlasmaBomb>().Direction = Attachment.transform.right;
            plasmaBomb.GetComponent<PlasmaBomb>().Damage = GetExtraVariable("Damage");
            plasmaBomb.GetComponent<PlasmaBomb>().ExplosionRadius = ExplosionRadius;

            vehicle.VehicleRigidBody.AddForceAtPosition(Attachment.transform.right * GetExtraVariable("Range") * -0.3f, transform.position, ForceMode2D.Impulse);

            vehicle.Plasma--;
            Cooldown -= 1f;
        }
        else
        {
            if (Cooldown < 1f)
                Cooldown += GetExtraVariable("AtkSpd") * Time.deltaTime;
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
