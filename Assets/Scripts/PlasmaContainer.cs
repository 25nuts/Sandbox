using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaContainer : Block
{
    public float PlasmaCapacity;

    public float ExplosionRadius = 1.2f;

    public override void DeleteBlock()
    {
        vehicle.PlasmaCapacity -= PlasmaCapacity;
        if (vehicle.Plasma > vehicle.PlasmaCapacity)
            vehicle.Plasma = vehicle.PlasmaCapacity;

        base.DeleteBlock();
    }

    public override void Start()
    {
        base.Start();

        PlasmaCapacity = GetExtraVariable("PlasmaCapacity");

        vehicle.Plasma += PlasmaCapacity;
        vehicle.PlasmaCapacity += PlasmaCapacity;
        vehicle.PlasmaUI.gameObject.SetActive(true);
    }

    public override void BreakBlock()
    {
        GameObject Explosion = Instantiate(EffectObjects.instance.PlasmaExplosion, transform.position, Quaternion.identity);
        Explosion.GetComponent<PlasmaExplosion>().Damage = GetExtraVariable("Damage");
        Explosion.GetComponent<PlasmaExplosion>().ExplosionRadius = ExplosionRadius;

        base.BreakBlock();
    }
}
