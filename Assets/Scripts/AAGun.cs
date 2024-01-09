using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AAGun : Block
{
    private float Cooldown = 0f;

    private Vector2 Barrel1 = new Vector2(1f, 0.15f);
    private Vector2 Barrel2 = new Vector2(1f, -0.15f);
    private bool Barrel;

    new public bool Turret = true;

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= 1f)
        {
            Vector3 pos;
            if (Barrel)
                pos = (Attachment.transform.right * Barrel1.x) + (Attachment.transform.up * Barrel1.y);
            else
                pos = (Attachment.transform.right * Barrel2.x) + (Attachment.transform.up * Barrel2.y);
            Barrel = !Barrel;

            GameObject cannonball = Instantiate(EffectObjects.instance.SmallCannonball, transform.position + pos, Quaternion.identity);
            if (LayerMask.LayerToName(gameObject.layer) == "Team2")
                cannonball.gameObject.layer = LayerMask.NameToLayer("Projectile2");
            else
                cannonball.gameObject.layer = LayerMask.NameToLayer("Projectile1");
            cannonball.GetComponent<Rigidbody2D>().velocity = vehicle.VehicleRigidBody.velocity;
            cannonball.GetComponent<Cannonball>().Force = GetExtraVariable("Range") * 0.6f;
            cannonball.GetComponent<Cannonball>().Direction = Attachment.transform.right;
            cannonball.GetComponent<Cannonball>().Damage = GetExtraVariable("Damage");

            vehicle.VehicleRigidBody.AddForceAtPosition(Attachment.transform.right * GetExtraVariable("Range") * -0.15f, transform.position, ForceMode2D.Impulse);

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
