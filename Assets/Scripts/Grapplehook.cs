using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplehook : MonoBehaviour
{
    public GameObject GrappleShotBlock;

    public float Force = 1f;

    public float Strength;
    public Vector2 Direction;
    public bool Connected;

    public Rigidbody2D thisRigidbody;

    public Block ConnectedBlock;

    public void Start()
    {
        thisRigidbody.velocity += Direction * Force;
        Connected = false;
    }

    public void Update()
    {
        if (!Connected) 
            transform.right = thisRigidbody.velocity;
    }

    public void FixedUpdate()
    {
        if (Connected && GrappleShotBlock != null)
        {
            ConnectedBlock.vehicle.VehicleRigidBody.AddForceAtPosition(Strength * Vector3.Normalize(GrappleShotBlock.transform.position - transform.position), transform.position);
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(EffectObjects.instance.SmallShockwave, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Connected)
        {
            Block target = collision.collider.GetComponent<Block>();

            if (target != null)
            {
                transform.parent = target.transform;
                thisRigidbody.simulated = false;
                GetComponent<CircleCollider2D>().enabled = false;
                ConnectedBlock = target;

                Connected = true;

                StartCoroutine(DelayedDestroy());
            }
            else
            {
                Instantiate(EffectObjects.instance.SmallShockwave, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
