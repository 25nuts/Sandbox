using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rotator : Block
{
    public void RotateBlock(int index)
    {
        Block block = vehicle.blockGrid[index];
        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (block != null)
        {
                block.Angle += vehicle.inputController.GetAxis(Button, reverseInput) * (GetExtraVariable("Speed") / block.Weight) * Time.deltaTime;
                if (block.Angle <= -180f)
                    block.Angle += 360f;
                if (block.Angle > 180f)
                    block.Angle -= 360f;
        }
    }

    private int IntToX(int i)
    {
        return i % vehicle.gridSize.x;
    }

    public void RotateBlocks()
    {
        int index = vehicle.blockGrid.IndexOf(this);

        bool reverseInput = vehicle.ReverseP2 && ReverseP2;
        if (vehicle.inputController.GetAxis(Button, reverseInput) != 0f)
        {
            if (IntToX(index) + 1 < vehicle.gridSize.x)
                RotateBlock(index + 1);
            if (IntToX(index) - 1 >= 0)
                RotateBlock(index - 1);
            if (index + vehicle.gridSize.x < vehicle.gridSize.x * vehicle.gridSize.y)
                RotateBlock(index + vehicle.gridSize.x);
            if (index - vehicle.gridSize.x >= 0)
                RotateBlock(index - vehicle.gridSize.x);

            Attachment.transform.Rotate(Vector3.forward * vehicle.inputController.GetAxis(Button, reverseInput) * GetExtraVariable("Speed") * 4f * Time.deltaTime);
        }
    }

    public override void Update()
    {
        base.Update();

        RotateBlocks();
    }
}
