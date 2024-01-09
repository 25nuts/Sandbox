using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredWheel : Wheel
{
    public float Strength = 30f;
    private Vector2 ForceStrength;
    private Vector2 ForcePosition;

    private Vector2 rotate (Vector2 v, float delta)
    {
        delta = delta * Mathf.Deg2Rad;
        return new Vector2(v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta), v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta));
    }

    public void Drive()
    {
        ForceStrength = new Vector2(0f, 0f);
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetAxis(Button, reverseInput) != 0f && vehicle.Energy >= GetExtraVariable("Energy") * Time.deltaTime && !Shocked)
        {
            float strength = GetExtraVariable("Speed");
            strength *= vehicle.inputController.GetAxis(Button, reverseInput);
            if (vehicle.Energy < 10f && GetExtraVariable("Energy") != 0f)
                strength *= vehicle.Energy * 0.1f;

            ContactPoint2D[] contacts = new ContactPoint2D[6];
            BlockCollider.GetContacts(contacts);
            ContactPoint2D contact;
            Vector2 normals = new Vector2(0f, 0f);
            for (int i = 0; i < 6; i++)
            {
                contact = contacts[i];
                normals += contact.normal;
            }
            normals.Normalize();

            if (normals == new Vector2(0f, 0f))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.47f,
                    LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

                if (hit.collider != null)
                {
                    normals = transform.up;
                }
            }

            if (normals != new Vector2(0f, 0f))
            {
                Vector2 tangent = rotate(normals, -90f);

                ForceStrength = tangent * strength;
                ForcePosition = new Vector2(transform.position.x, transform.position.y) + (normals * WheelRadius);

                if (Mathf.Sign(vehicle.VehicleRigidBody.velocity.x) != Mathf.Sign(strength) && vehicle.VehicleRigidBody.velocity.x != 0f)
                {
                    ForceStrength *= 3f;
                }

                vehicle.Energy = Mathf.Max(vehicle.Energy - (GetExtraVariable("Energy") * Time.deltaTime), 0f);

                Heat += GetExtraVariable("Heat") * Mathf.Abs(vehicle.inputController.GetAxis(Button, reverseInput)) * Time.deltaTime;
            }
        }
    }

    public override void Update()
    {
        base.Update();

        Drive();
    }

    public void FixedUpdate()
    {
        if (ForceStrength != new Vector2(0f, 0f))
            vehicle.VehicleRigidBody.AddForceAtPosition(ForceStrength, ForcePosition);
    }
}
