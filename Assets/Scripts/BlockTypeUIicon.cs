using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTypeUIicon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EditorController controller;

    public float DefaultSize;
    public float SelectedSize;

    public int index = 0;

    void Start()
    {
        Vector3 offset = new Vector3(0f, ((controller.blockTypeList.BlockTypes.Count - 1) * 0.5f) + (0 - index), 0f);
        transform.position = transform.position + offset;
        spriteRenderer.sprite = controller.blockTypeList.BlockTypes[index].UIicon;
    }

    void Update()
    {
        if (controller.SelectedTypeIndex == index)
            transform.localScale = new Vector3(SelectedSize, SelectedSize, transform.localScale.z);
        else
            transform.localScale = new Vector3(DefaultSize, DefaultSize, transform.localScale.z);
    }

    public void OnMouseDown()
    {
        controller.SelectedBlockType = controller.blockTypeList.BlockTypes[index];
        controller.SelectedTypeIndex = index;
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
