using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObjects : MonoBehaviour
{
    public static EffectObjects instance;

    public GameObject Shockwave;
    public GameObject BrokenBlock;

    public GameObject MinigunBullets;
    public GameObject Cannonball;
    public GameObject SmallShockwave;
    public GameObject FlameJet;
    public GameObject Laser;
    public GameObject LaserCharge;
    public GameObject ThrusterFlame;
    public GameObject SmallCannonball;
    public GameObject HealEffect;
    public GameObject Missle;
    public GameObject Explosion;
    public GameObject Grapplehook;
    public GameObject GrappleLine;
    public GameObject Lightning;
    public GameObject SmallLightning;
    public GameObject ShockedParticleEffect;
    public GameObject PlasmaExplosion;
    public GameObject LargePlasmaExplosion;
    public GameObject PlasmaBomb;
    public GameObject PlasmaShield;
    public GameObject BlueFlameJet;

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Awake()
    {
        CreateSingleton();
    }
}
