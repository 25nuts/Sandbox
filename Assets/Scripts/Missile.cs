using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float Force = 10f;
    public float ExplosionRadius = 1f;
    public float ExplosionForce = 10f;

    public float Damage;
    public int Target;
    public Vector3 TargetPosition;
    public bool TargetEnabled;

    public Rigidbody2D thisRigidbody;
    public SpriteRenderer spriteRenderer;

    public void Start()
    {

    }

    public void SetTarget()
    {
        if (Target == 1)
        {
            TargetPosition = CorePositions.instance.Core1;
            TargetEnabled = CorePositions.instance.Core1Enabled;
        }
        else
        {
            TargetPosition = CorePositions.instance.Core2;
            TargetEnabled = CorePositions.instance.Core2Enabled;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D collider in colliders)
        {
            Block block = collider.GetComponent<Block>();
            if (block != null )
            {
                block.Damage(Damage, 1f);
                block.vehicle.VehicleRigidBody.AddForceAtPosition((block.transform.position - transform.position) * ExplosionForce, block.transform.position, ForceMode2D.Impulse);
            }
        }

        Instantiate(EffectObjects.instance.Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Update()
    {
        SetTarget();
        if (TargetEnabled)
        {
            transform.right = Vector3.RotateTowards(transform.right, Vector3.Normalize(TargetPosition - transform.position), 90f * Time.deltaTime, 0f);
        }
        thisRigidbody.AddForce(transform.right * 2f);

        if (transform.up.y < 0f)
            spriteRenderer.flipY = true;
        else
            spriteRenderer.flipY = false;
    }
}
