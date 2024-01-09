using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBlueprint : MonoBehaviour
{
    public GameObject VehicleController;
    public GameObject Block;

    public Vector2 Center = new Vector2(0f, 0f);
    public Vector2 GridSize = new Vector2(2f, 2f);
    public GameObject vehicle;

    public Image EnergyUI;
    public Image PlasmaUI;

    void Awake()
    {

    }

    public void GenerateBlueprint(List<BlockData> blueprint, int playerIndex, Inputs inputs, string layer)
    {
        vehicle = Instantiate(VehicleController, new Vector3(Center.x, Center.y, 0f), Quaternion.identity);
        vehicle.layer = LayerMask.NameToLayer(layer);
        vehicle.GetComponent<InputController>().inputs = inputs;
        VehicleController vehicleController = vehicle.GetComponent<VehicleController>();
        vehicleController.PlayerIndex = playerIndex;
        vehicleController.gridSize = new Vector2Int(Mathf.FloorToInt(GridSize.x), Mathf.FloorToInt(GridSize.y));
        vehicleController.EnergyUI = EnergyUI;
        vehicleController.PlasmaUI = PlasmaUI;

        Vector2 pos = new Vector2(0f, 0f);
        GameObject block;

        BlockData currentBlock;

        for (int i = 0; i < blueprint.Count; i++)
        {
            float x = i % GridSize.x;
            float y = Mathf.Floor(i / GridSize.x);

            if (playerIndex == 2)
                currentBlock = blueprint[Mathf.FloorToInt((GridSize.x - x - 1f) + (y * GridSize.x))];
            else
                currentBlock = blueprint[i];

            if (currentBlock.BlockName != "Empty")
            {
                pos = Center - ((GridSize - new Vector2(1f, 1f)) * 0.5f) + new Vector2(x, y);
                block = Instantiate(Block, pos, Quaternion.identity);

                block.layer = LayerMask.NameToLayer(layer);
                block.transform.SetParent(vehicle.transform);
                block.AddComponent(Type.GetType(currentBlock.Script));
                block.GetComponent<Block>().SetBlock(currentBlock, vehicleController);

                vehicleController.blocks.Add(block.GetComponent<Block>());
                vehicleController.blockGrid.Add(block.GetComponent<Block>());
                if (currentBlock.Core)
                    vehicleController.CorePosition = i;
            }
            else
            {
                vehicleController.blockGrid.Add(null);
            }
        }
    }

    public void DeleteVehicle()
    {
        Destroy(vehicle);
        EnergyUI.gameObject.SetActive(false);
        PlasmaUI.gameObject.SetActive(false);
    }
}
