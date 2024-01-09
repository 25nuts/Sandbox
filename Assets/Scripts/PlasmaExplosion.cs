using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaExplosion : MonoBehaviour
{
    public float Damage = 0f;
    public float ExplosionRadius;

    void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D collider in colliders)
        {
            Block block = collider.GetComponent<Block>();
            if (block != null)
            {
                if (block.HP > 0f)
                    block.Damage(Damage, 1f);
            }
        }

        Destroy(gameObject, 1f);
    }
}
