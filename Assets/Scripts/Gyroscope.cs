using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gyroscope : Block
{
    public float Torque = 10f;
    public Vector3 Direction;

    private float Speed;

    public void RotateVehicle()
    {
        float angle = 0f - Vector2.SignedAngle(Direction, vehicle.transform.up);
        angle -= vehicle.VehicleRigidBody.angularVelocity;
        angle *= Mathf.Abs((angle) / 180f);
        angle *= Torque;

        Speed = angle;
    }

    public override void Start()
    {
        base.Start();

        Direction = transform.up;
    }

    public override void Update()
    {
        base.Update();

        RotateVehicle();
        Attachment.transform.up = Direction;
    }

    public void FixedUpdate()
    {
        vehicle.VehicleRigidBody.AddTorque(Speed);
    }
}
