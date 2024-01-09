using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEditorGrid : MonoBehaviour
{

    public GameObject Block = null;
    public Vector2 Center = new Vector2(0f, 0f);
    [HideInInspector]
    public Vector2 GridSize = new Vector2(2f, 2f);

    public List<GameObject> gridCells = new List<GameObject>();

    private EditorController controller;

    void Awake()
    {
        controller = GetComponent<EditorController>();
        GridSize = controller.GridSize;
    }

    public void GenerateGrid()
    {
        Vector2 pos = new Vector2(0f, 0f);
        gridCells.Clear();
        GameObject gridCell = null;

        int i = 0;
        for(int y = 0; y < GridSize.y; y++)
        {
            for(int x = 0; x < GridSize.x; x++)
            {
                pos = Center - ((GridSize - new Vector2(1f, 1f)) * 0.5f) + new Vector2(x, y);
                gridCell = Instantiate(Block, pos, Quaternion.identity);
                gridCell.GetComponent<EditorBlock>().controller = controller;
                gridCell.GetComponent<EditorBlock>().index = i;
                gridCells.Add(gridCell);

                i++;
            }
        }
    }

    public void HideUI()
    {
        for (int i = 0; i < gridCells.Count; i++)
        {
            gridCells[i].SetActive(false);
        }
    }

    public void ShowUI()
    {
        for (int i = 0; i < gridCells.Count; i++)
        {
            gridCells[i].SetActive(true);
        }
    }
}
