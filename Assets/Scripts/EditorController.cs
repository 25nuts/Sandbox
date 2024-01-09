using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    private GenerateEditorGrid gridGenerator;
    private GenerateEditorUI uiGenerator;
    [HideInInspector]
    public BlockInspector blockInspector;
    [HideInInspector]
    public TestBlueprint testBlueprint;
    [HideInInspector]
    public BuildBlueprint buildBlueprint;

    public SaveFileManager saveManager;

    public CameraController cameraController;

    public BlockTypeList blockTypeList;
    public BlockList SelectedBlockType;
    public BlockData SelectedBlock;
    public int SelectedTypeIndex;
    public int SelectedBlockIndex;

    public Vector2Int GridSize = new Vector2Int(2, 2);
    public float Budget;
    [HideInInspector]
    public float BudgetLimit;

    public Inputs inputs;

    public List<BlockData> BluePrint = new List<BlockData>();
    public int SelectedGridCell = -1;
    public bool CorePlaced;
    [HideInInspector]
    public int CorePosition;

    void Awake()
    {
        gridGenerator = GetComponent<GenerateEditorGrid>();
        uiGenerator = GetComponent<GenerateEditorUI>();
        blockInspector = GetComponent<BlockInspector>();
        testBlueprint = GetComponent<TestBlueprint>();
        buildBlueprint = GetComponent<BuildBlueprint>();

        cameraController = Camera.main.GetComponent<CameraController>();

        int gridSize = GridSize.x * GridSize.y;
        for (int i = 0; i < gridSize; i++)
        {
            BluePrint.Add(BlockData.CreateInstance<BlockData>());
        }

        BudgetLimit = Budget;
    }

    void Start()
    {
        SelectedBlock = blockTypeList.BlockTypes[0].Blocks[0];
        gridGenerator.GenerateGrid();
        uiGenerator.GenerateUI();
        blockInspector.RefreshInspector(SelectedBlock);
        Cursor.visible = true;
    }

    public void ActivateTestMode()
    {
        blockInspector.HideUI();
        uiGenerator.HideUI();
        gridGenerator.HideUI();
        saveManager.HideUI();
        buildBlueprint.GenerateBlueprint(BluePrint, 1, inputs, "Team1");
    }

    public void DeactivateTestMode()
    {
        blockInspector.ShowUI();
        uiGenerator.ShowUI();
        gridGenerator.ShowUI();
        saveManager.ShowUI();
        buildBlueprint.DeleteVehicle();
        cameraController.Target = new Vector2(0f, 0f);
        cameraController.Lerp = true;
    }

    public void AddBlock(string blockString, int index)
    {
        GameObject editorBlock = gridGenerator.gridCells[index];
        EditorBlock editorBlockScript = editorBlock.GetComponent<EditorBlock>();

        if (blockString == "null")
        {
            BluePrint[index] = BlockData.CreateInstance<BlockData>();

            editorBlockScript.spriteRenderer.sprite = editorBlockScript.Empty;
            editorBlockScript.attachment.gameObject.SetActive(false);

            return;
        }

        string[] block = blockString.Split(char.Parse(","));

        int x = int.Parse(block[0]);
        int y = int.Parse(block[1]);

        BluePrint[index] = BlockData.Instantiate<BlockData>(blockTypeList.BlockTypes[y].Blocks[x]);
        BlockData blockData = BluePrint[index];

        blockData.Button = int.Parse(block[2]);
        blockData.ReverseP2 = bool.Parse(block[3]);
        blockData.Angle = float.Parse(block[4]);

        if (blockData.Core)
        {
            CorePlaced = true;
            CorePosition = index;
        }

        editorBlockScript.spriteRenderer.sprite = blockData.BlockSprite;
        if (blockData.ExtraSprite != null)
        {
            editorBlockScript.attachment.gameObject.SetActive(true);
            editorBlockScript.attachment.sprite = blockData.ExtraSprite;
        }
        else
        {
            editorBlockScript.attachment.gameObject.SetActive(false);
        }

        Budget -= blockData.Cost;
    }
}
