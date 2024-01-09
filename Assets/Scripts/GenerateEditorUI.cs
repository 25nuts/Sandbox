using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEditorUI : MonoBehaviour
{
    public GameObject BlockType;
    public GameObject Block;

    public Vector2 Center = new Vector2(0f, 0f);
    public Vector2 Offset = new Vector2(-2f, 2f);

    private List<GameObject> uiCells = new List<GameObject>();

    private EditorController controller;

    void Awake()
    {
        controller = GetComponent<EditorController>();
    }

    public void GenerateUI()
    {
        Vector2 pos = new Vector2(0f, 0f);
        int types = controller.blockTypeList.BlockTypes.Count;
        uiCells.Clear();
        GameObject uiCell = null;

        for (int i = 0; i < types; i++)
        {
            pos = Center + new Vector2(Offset.x, 0f);
            uiCell = Instantiate(BlockType, pos, Quaternion.identity);
            uiCell.GetComponent<BlockTypeUIicon>().controller = controller;
            uiCell.GetComponent<BlockTypeUIicon>().index = i;
            uiCells.Add(uiCell);
        }

        types = 8;
        for (int i = 0; i < types; i++)
        {
            pos = Center + new Vector2(0f, Offset.y);
            uiCell = Instantiate(Block, pos, Quaternion.identity);
            uiCell.GetComponent<BlockUIicon>().controller = controller;
            uiCell.GetComponent<BlockUIicon>().index = i;
            uiCells.Add(uiCell);
        }
    }

    public void HideUI()
    {
        for (int i = 0; i < uiCells.Count; i++)
        {
            uiCells[i].SetActive(false);
        }
    }

    public void ShowUI()
    {
        for (int i = 0; i < uiCells.Count; i++)
        {
            uiCells[i].SetActive(true);
        }
    }
}
