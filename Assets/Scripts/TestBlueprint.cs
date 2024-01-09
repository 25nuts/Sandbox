using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestBlueprint : MonoBehaviour
{
    public bool InTestMode;
    public Image errorBox;
    public TextMeshProUGUI errorMessage;
    private int recursion;
    [SerializeField]
    private List<bool> floodGrid = new List<bool>();
    private Vector2Int gridSize = new Vector2Int(0, 0);
    private bool testBool;

    private EditorController controller;

    void Awake()
    {
        controller = GetComponent<EditorController>();
        gridSize  = controller.GridSize;
    }

    public void DisplayError(string text)
    {
        errorBox.gameObject.SetActive(true);
        errorMessage.text = text;
    }

    private void FloodFill(int pos, float direction)
    {
        recursion++;
        if (recursion > 9999)
            return;
        int x = pos % gridSize.x;
        int y = Mathf.FloorToInt(pos / gridSize.x);
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y || floodGrid[pos] || controller.BluePrint[pos].BlockName == "Empty" || (controller.BluePrint[pos].Directional && controller.BluePrint[pos].Angle != direction))
            return;
        else
        {
            floodGrid[pos] = true;
            if (controller.BluePrint[pos].Directional)
                return;
            FloodFill(pos + 1, -90f);
            FloodFill(pos - 1, 90f);
            FloodFill(pos + gridSize.x, 180f);
            FloodFill(pos - gridSize.x, 0f);
        }
    }

    private void FloodCheck()
    {
        recursion = 0;
        floodGrid.Clear();
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            floodGrid.Add(false);
        }
        FloodFill(controller.CorePosition, 0f);
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            if (floodGrid[i] == false && controller.BluePrint[i].BlockName != "Empty")
            {
                testBool = false;
            }
        }
    }

    public bool TestVehicle()
    {
        if (controller.CorePlaced)
        {
            testBool = true;
            FloodCheck();
            if (testBool)
            {
                return true;
            }
            else
            {
                DisplayError("Disconnected Blocks");
                return false;
            }
        }
        else
        {
            DisplayError("Missing Core");
            return false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !controller.saveManager.inputField.isFocused)
        {
            if (InTestMode)
            {
                InTestMode = false;
                controller.DeactivateTestMode();
            }
            else
            {
                if (TestVehicle())
                {
                    InTestMode = true;
                    errorBox.gameObject.SetActive(false);
                    controller.ActivateTestMode();
                }
            }
        }
    }
}
