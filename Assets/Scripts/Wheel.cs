using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : Block
{
    public PhysicsMaterial2D WheelMaterial;
    public float WheelRadius = 0.45f;
    protected float Velocity;
    public float AirFriction = 0.7f;

    public override void AddCollider()
    {
        BlockCollider = gameObject.AddComponent<CircleCollider2D>();
        GetComponent<CircleCollider2D>().radius = WheelRadius;

        WheelMaterial = new PhysicsMaterial2D();
        WheelMaterial.friction = 0.05f;
        BlockCollider.sharedMaterial = WheelMaterial;
    }

    protected virtual void SetWheelVelocity()
    {
        bool touchingGround;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 0.47f,
                   LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));
        touchingGround = hit.collider != null;

        ContactPoint2D[] contacts = new ContactPoint2D[1];
        BlockCollider.GetContacts(contacts);
        ContactPoint2D contact = contacts[0];
        if (contact.enabled || touchingGround)
        {
            Velocity = vehicle.GetComponent<Rigidbody2D>().velocity.magnitude;
        }
        else
        {
            Velocity -= Mathf.Max(Velocity * AirFriction * Time.deltaTime, Time.deltaTime);
            Velocity = Mathf.Max(Velocity, 0f);
        }
    }

    private void RotateWheel()
    {
        float speed = Velocity;
        speed = 360f * (speed / (WheelRadius * 6.28f));
        speed = speed * Time.deltaTime;
        float direction = Mathf.Sign(vehicle.GetComponent<Rigidbody2D>().velocity.x) * -1f;
        Attachment.transform.Rotate(new Vector3(0f, 0f, speed * direction));
    }
    
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        SetWheelVelocity();
        RotateWheel();
    }
}
