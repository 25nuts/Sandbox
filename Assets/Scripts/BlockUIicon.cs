using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIicon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider;
    public EditorController controller;

    public float DefaultSize;
    public float SelectedSize;

    public int index = 0;

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (index < controller.SelectedBlockType.Blocks.Count)
        {
            Vector3 offset = new Vector3(((controller.SelectedBlockType.Blocks.Count - 1) * -0.5f) + index, 0f, 0f);
            transform.position = startingPosition + offset;
            if (controller.SelectedBlockType.Blocks.Contains(controller.SelectedBlock) && controller.SelectedBlockIndex == index)
                transform.localScale = new Vector3(SelectedSize, SelectedSize, transform.localScale.z);
            else
                transform.localScale = new Vector3(DefaultSize, DefaultSize, transform.localScale.z);
            spriteRenderer.sprite = controller.SelectedBlockType.Blocks[index].UISprite;
            boxCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
    }

    public void OnMouseDown()
    {
        controller.SelectedBlock = controller.SelectedBlockType.Blocks[index];
        controller.SelectedBlockIndex = index;
        controller.SelectedGridCell = -1;
        controller.blockInspector.RefreshInspector(controller.SelectedBlock);
    }

    public void OnMouseOver()
    {
        spriteRenderer.color = Color.gray;
    }

    public void OnMouseExit()
    {
        spriteRenderer.color = Color.white;
    }
}
