using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : Block
{
    private Vector2 ForceStrength;
    private Vector2 ForcePosition;

    public GameObject FlameEffect;
    public ParticleSystem FlameEffectSystem;

    public void Thrust()
    {
        ForceStrength = new Vector2(0f, 0f);
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetAxis(Button, reverseInput) > 0f)
        {
            ForceStrength = transform.up;
            ForceStrength *= GetExtraVariable("Thrust") * vehicle.inputController.GetAxis(Button, reverseInput) * 10f;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up * -1f, 2.5f,
                    LayerMask.GetMask(vehicle.EnemyLayer, "Ground"));

            if (hit.collider != null)
            {
                Block hitBlock = hit.collider.GetComponent<Block>();
                if (hitBlock != null)
                {
                    hitBlock.Heat += 20f * Time.deltaTime;
                }
            }

            Heat += GetExtraVariable("Heat") * Mathf.Abs(vehicle.inputController.GetAxis(Button, reverseInput)) * Time.deltaTime;
        }
    }

    public void SetFlameEffect()
    {
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetAxis(Button, reverseInput) > 0f)
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

        FlameEffect = GameObject.Instantiate(EffectObjects.instance.ThrusterFlame, transform.position + (transform.up * -0.5f), transform.rotation);
        FlameEffect.transform.parent = transform;
        FlameEffectSystem = FlameEffect.GetComponent<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        Thrust();
        SetFlameEffect();
    }

    public void FixedUpdate()
    {
        ForcePosition = transform.position;
        if (ForceStrength != new Vector2(0f, 0f))
            vehicle.VehicleRigidBody.AddForceAtPosition(ForceStrength, ForcePosition);
    }
}
