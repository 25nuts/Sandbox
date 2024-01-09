using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Block
{
    public float EnergyCapacity;
    public float Recharge;

    public override void DeleteBlock()
    {
        vehicle.EnergyCapacity -= EnergyCapacity;
        if (vehicle.Energy > vehicle.EnergyCapacity)
            vehicle.Energy = vehicle.EnergyCapacity;

        base.DeleteBlock();
    }

    public override void Start()
    {
        base.Start();

        EnergyCapacity = GetExtraVariable("EnergyCapacity");
        Recharge = GetExtraVariable("Recharge");

        vehicle.Energy += EnergyCapacity;
        vehicle.EnergyCapacity += EnergyCapacity;
    }

    
    public override void Update()
    {
        base.Update();

        if (vehicle.Energy < vehicle.EnergyCapacity && !Shocked)
        {
            vehicle.Energy = Mathf.Min(vehicle.Energy + (Recharge * Time.deltaTime), vehicle.EnergyCapacity);
            Heat += GetExtraVariable("Heat") * Time.deltaTime;
        }
    }
}
