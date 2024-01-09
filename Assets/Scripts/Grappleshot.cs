using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappleshot : Block
{
    private float Cooldown = 0f;

    public GameObject LineEffect;
    public LineRenderer LineEffectRenderer;
    public GameObject Grapplehook;

    new public bool Turret = true;

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= GetExtraVariable("Cooldown"))
        {
            Grapplehook = Instantiate(EffectObjects.instance.Grapplehook, transform.position, Quaternion.identity);
            if (LayerMask.LayerToName(gameObject.layer) == "Team2")
                Grapplehook.gameObject.layer = LayerMask.NameToLayer("Projectile2");
            else
                Grapplehook.gameObject.layer = LayerMask.NameToLayer("Projectile1");

            Grapplehook.GetComponent<Grapplehook>().GrappleShotBlock = gameObject;
            Grapplehook.GetComponent<Rigidbody2D>().velocity = vehicle.VehicleRigidBody.velocity;
            Grapplehook.GetComponent<Grapplehook>().Force = GetExtraVariable("Range") * 0.6f;
            Grapplehook.GetComponent<Grapplehook>().Direction = Attachment.transform.right;
            Grapplehook.GetComponent<Grapplehook>().Strength = GetExtraVariable("Strength");

            Cooldown -= GetExtraVariable("Cooldown");
        }
        else
        {
            if (Cooldown < GetExtraVariable("Cooldown"))
                Cooldown += Time.deltaTime;
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

        if (Cooldown < GetExtraVariable("Cooldown"))
            Attachment.enabled = false;
        else
            Attachment.enabled = true;
    }

    public void GrappleshotLine()
    {
        if (Grapplehook != null)
        {
            LineEffect.gameObject.SetActive(true);
            LineEffectRenderer.SetPosition(0, LineEffect.transform.position);
            LineEffectRenderer.SetPosition(1, Grapplehook.transform.position);
        }
        else
        {
            LineEffect.gameObject.SetActive(false);
        }
    }

    public override void Start()
    {
        base.Start();

        Cooldown = GetExtraVariable("Cooldown");

        LineEffect = GameObject.Instantiate(EffectObjects.instance.GrappleLine, transform.position, Attachment.transform.rotation);
        LineEffect.transform.parent = Attachment.transform;
        LineEffectRenderer = LineEffect.GetComponent<LineRenderer>();
    }

    public override void Update()
    {
        base.Update();

        RotateAttachment();
        Shoot();
        GrappleshotLine();
    }
}
