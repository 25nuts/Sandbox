using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGenerator : Block
{
    public float PlasmaCapacity;
    public float Charge;
    public float ChargeTime = 5f;

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

        vehicle.PlasmaCapacity += PlasmaCapacity;
        vehicle.PlasmaUI.gameObject.SetActive(true);
    }

    public override void Update()
    {
        base.Update();

        if (vehicle.Plasma < vehicle.PlasmaCapacity && vehicle.Energy > GetExtraVariable("Energy")  && !Shocked)
        {
            vehicle.Energy = Mathf.Max(vehicle.Energy - (GetExtraVariable("Energy") * Time.deltaTime), 0f);
            Heat += GetExtraVariable("Heat") * Time.deltaTime;
            Charge += Time.deltaTime;
            if (Charge >= ChargeTime)
            {
                vehicle.Plasma++;
                Charge -= ChargeTime;
            }
        }
    }
}
