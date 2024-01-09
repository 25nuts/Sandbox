using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : Block
{
    private float Cooldown = 0f;

    new public bool Turret = true;

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= 1f)
        {
            GameObject cannonball = Instantiate(EffectObjects.instance.Cannonball, transform.position + (Attachment.transform.right * 0.8f), Quaternion.identity);
            if (LayerMask.LayerToName(gameObject.layer) == "Team2")
                cannonball.gameObject.layer = LayerMask.NameToLayer("Projectile2");
            else
                cannonball.gameObject.layer = LayerMask.NameToLayer("Projectile1");

            cannonball.GetComponent<Rigidbody2D>().velocity = vehicle.VehicleRigidBody.velocity;
            cannonball.GetComponent<Cannonball>().Force = GetExtraVariable("Range") * 0.6f;
            cannonball.GetComponent<Cannonball>().Direction = Attachment.transform.right;
            cannonball.GetComponent<Cannonball>().Damage = GetExtraVariable("Damage");

            vehicle.VehicleRigidBody.AddForceAtPosition(Attachment.transform.right * GetExtraVariable("Range") * -0.6f, transform.position, ForceMode2D.Impulse);

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
