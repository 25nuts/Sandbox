using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBomb : MonoBehaviour
{
    public float Force = 1f;

    public float Damage;
    public float ExplosionRadius;
    public Vector2 Direction;

    public Rigidbody2D thisRigidbody;

    public void Start()
    {
        thisRigidbody.velocity += Direction * Force;
    }

    public void Update()
    {
        transform.right = thisRigidbody.velocity;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject Explosion = Instantiate(EffectObjects.instance.LargePlasmaExplosion, transform.position, Quaternion.identity);
        Explosion.GetComponent<PlasmaExplosion>().Damage = Damage;
        Explosion.GetComponent<PlasmaExplosion>().ExplosionRadius = ExplosionRadius;

        Destroy(gameObject);
    }
}
