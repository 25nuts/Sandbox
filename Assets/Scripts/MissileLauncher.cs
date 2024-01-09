using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : Block
{
    public float TurnSpeed = 90f;

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

        if (Cooldown < GetExtraVariable("ReloadTime"))
            Attachment.enabled = false;
        else
            Attachment.enabled = true;
    }

    public void Shoot()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetButton(Button, reverseInput, false) && Cooldown >= GetExtraVariable("ReloadTime"))
        {
            GameObject missile = Instantiate(EffectObjects.instance.Missle, transform.position, Quaternion.identity);
            if (LayerMask.LayerToName(gameObject.layer) == "Team2")
                missile.gameObject.layer = LayerMask.NameToLayer("Projectile2");
            else
                missile.gameObject.layer = LayerMask.NameToLayer("Projectile1");
            missile.transform.right = Attachment.transform.right;
            missile.GetComponent<Missile>().Target = 3 - vehicle.PlayerIndex;
            missile.GetComponent<Missile>().Damage = GetExtraVariable("Damage");

            Cooldown -= GetExtraVariable("ReloadTime");
        }
        else
        {
            if (Cooldown < GetExtraVariable("ReloadTime"))
                Cooldown += Time.deltaTime;
        }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        SetTarget();
        RotateTurret();
        Shoot();
    }
}
