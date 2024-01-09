using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public EditorController controller;
    public SpriteRenderer spriteRenderer;

    public Vector2 Center = new Vector2(0f, 0f);

    void Update()
    {
        if (controller.SelectedGridCell == -1 || controller.testBlueprint.InTestMode)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            float x = controller.SelectedGridCell % controller.GridSize.x;
            float y = Mathf.Floor(controller.SelectedGridCell / controller.GridSize.x);
            transform.position = Center - ((controller.GridSize - new Vector2(1f, 1f)) * 0.5f) + new Vector2(x, y);
        }
    }
}
