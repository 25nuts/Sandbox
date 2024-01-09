using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingBlock : Block
{
    public void CoolBlock(int index)
    {
        Block block = vehicle.blockGrid[index];
        if (block != null)
        {
            if (vehicle.Energy > GetExtraVariable("Energy") * Time.deltaTime && block.Heat > GetExtraVariable("Cooling") && !Shocked)
            {
                block.Heat -= GetExtraVariable("Cooling") * Time.deltaTime;
                block.Heat = Mathf.Max(block.Heat, 0f);

                vehicle.Energy -= GetExtraVariable("Energy") * Time.deltaTime;
                vehicle.Energy = Mathf.Max(vehicle.Energy, 0f);
            }
        }
    }

    private int IntToX(int i)
    {
        return i % vehicle.gridSize.x;
    }

    public void CoolBlocks()
    {
        int index = vehicle.blockGrid.IndexOf(this);

        CoolBlock(index);
        if (IntToX(index) + 1 < vehicle.gridSize.x)
            CoolBlock(index + 1);
        if (IntToX(index) - 1 >= 0)
            CoolBlock(index - 1);
        if (index + vehicle.gridSize.x < vehicle.gridSize.x * vehicle.gridSize.y)
            CoolBlock(index + vehicle.gridSize.x);
        if (index - vehicle.gridSize.x >= 0)
            CoolBlock(index - vehicle.gridSize.x);
    }

    public override void Update()
    {
        base.Update();

        CoolBlocks();
    }
}
