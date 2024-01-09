using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class EditorBlock : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EditorController controller;
    public Sprite Empty;
    public SpriteRenderer attachment;
    private BlockData block;
    public int index;

    public void PlaceBlock()
    {
        block = controller.SelectedBlock;
        if ((!(block.Core && controller.CorePlaced)) && block.Cost <= controller.Budget + controller.BluePrint[index].Cost)
        {
            spriteRenderer.sprite = controller.SelectedBlock.BlockSprite;
            controller.Budget = controller.Budget + controller.BluePrint[index].Cost - block.Cost;
            if (controller.BluePrint[index].Core)
                controller.CorePlaced = false;
            if (block.Core)
            {
                controller.CorePlaced = true;
                controller.CorePosition = index;
            }
            if (block.ExtraSprite != null)
            {
                attachment.gameObject.SetActive(true);
                attachment.sprite = block.ExtraSprite;
            }
            else
            {
                attachment.gameObject.SetActive(false);
            }
            controller.SelectedGridCell = index;
            if (controller.BluePrint[index].BlockName != controller.SelectedBlock.BlockName)
            {
                controller.BluePrint[index] = BlockData.Instantiate<BlockData>(block);
                controller.blockInspector.RefreshInspector(controller.SelectedBlock);
            }
            else
            {
                controller.blockInspector.RefreshInspector(controller.BluePrint[index]);
            }
        }
    }

    public void DeleteBlock()
    {
        spriteRenderer.sprite = Empty;
        controller.Budget = controller.Budget + controller.BluePrint[index].Cost;
        attachment.gameObject.SetActive(false);
        if (controller.BluePrint[index].Core)
            controller.CorePlaced = false;
        controller.BluePrint[index] = BlockData.CreateInstance<BlockData>();
        if (controller.SelectedGridCell == index)
        {
            controller.SelectedGridCell = -1;
            controller.blockInspector.RefreshInspector(controller.SelectedBlock);
        }
    }

    public void SelectBlock()
    {
        controller.SelectedGridCell = index;
        block = controller.BluePrint[index];
        controller.blockInspector.RefreshInspector(block);
    }

    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == Empty)
            PlaceBlock();
        else
            SelectBlock();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && spriteRenderer.sprite != Empty)
        {
            DeleteBlock();
        }
    }

    void Update()
    {
        if (controller.BluePrint[index].Rotatable)
        {
            if (index == controller.SelectedGridCell)
            {
                if (Input.GetKeyDown(KeyCode.Q) && !controller.saveManager.inputField.isFocused)
                {
                    controller.BluePrint[index].Angle = controller.BluePrint[index].Angle - 90f;
                    if (controller.BluePrint[index].Angle <= -180f)
                        controller.BluePrint[index].Angle = controller.BluePrint[index].Angle + 360f;
                }
                if (Input.GetKeyDown(KeyCode.E) && !controller.saveManager.inputField.isFocused)
                {
                    controller.BluePrint[index].Angle = controller.BluePrint[index].Angle + 90f;
                    if (controller.BluePrint[index].Angle > 180f)
                        controller.BluePrint[index].Angle = controller.BluePrint[index].Angle - 360f;
                }
            }

            transform.eulerAngles = Vector3.forward * controller.BluePrint[index].Angle * -1f;
        }
        else
        {
            transform.eulerAngles = Vector3.forward * 0f;
        }

        block = controller.BluePrint[index];
        if (block.ExtraSprite != null)
        {
            attachment.transform.eulerAngles = Vector3.forward * block.Angle * -1f;
            if (block.Angle < -90f || block.Angle > 90f)
                attachment.flipY = true;
            else
                attachment.flipY = false;
        }
    }
}
