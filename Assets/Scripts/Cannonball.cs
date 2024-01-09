using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float Force = 1f;

    public float Damage;
    public Vector2 Direction;

    public Rigidbody2D thisRigidbody;

    public void Start()
    {
        thisRigidbody.velocity += Direction * Force;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Block target = collision.collider.GetComponent<Block>();

        if (target != null)
        {
            target.Damage(Damage, 1f);
        }
        
        Instantiate(EffectObjects.instance.SmallShockwave, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
