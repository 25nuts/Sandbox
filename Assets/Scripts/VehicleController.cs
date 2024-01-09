using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleController : MonoBehaviour
{
    public int PlayerIndex;
    public string EnemyLayer;

    public Rigidbody2D VehicleRigidBody;

    public List<Block> blocks = new List<Block>();
    public List<Block> blockGrid = new List<Block>();

    public Vector2Int gridSize = new Vector2Int(2, 2);

    public int CorePosition;

    public CameraController cameraController;
    public Vector2 Center;

    public InputController inputController;
    public bool ReverseP2 = false;

    private int recursion;
    public List<bool> floodGrid = new List<bool>();

    public Image EnergyUI;
    private TextMeshProUGUI EnergyText;
    public Image PlasmaUI;
    private TextMeshProUGUI PlasmaText;

    public float Energy;
    public float EnergyCapacity;
    public float Plasma;
    public float PlasmaCapacity;

    public void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        inputController = GetComponent<InputController>();
    }

    public void CalculateCenterOfMass()
    {
        Vector3 CoM = new Vector3(0f, 0f, 0f);
        float totalMass = 0f;
        for (int i = 0; i < blocks.Count; i++)
        {
            CoM = CoM + (blocks[i].transform.localPosition * blocks[i].Weight);
            totalMass += blocks[i].Weight;
        }
        if (totalMass != 0f)
            CoM /= totalMass;

        VehicleRigidBody.centerOfMass = CoM;
        VehicleRigidBody.mass = totalMass;
    }

    private void SetupEnergyUI()
    {
        EnergyUI.gameObject.SetActive(true);
        EnergyText = EnergyUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void SetupPlasmaUI()
    {
        if (PlasmaCapacity > 0f)
            PlasmaUI.gameObject.SetActive(true);
        PlasmaText = PlasmaUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void CalculateCenterOfVehicle()
    {
        Vector3 pos;
        float left = 0f;
        float right = 0f;
        float down = 0f;
        float up = 0f;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (i == 0)
            {
                pos = blocks[i].transform.position;
                left = pos.x;
                right = pos.x;
                down = pos.y;
                up = pos.y;
            }
            else
            {
                pos = blocks[i].transform.position;
                if (pos.x < left)
                    left = pos.x;
                if (pos.x > right)
                    right = pos.x;
                if (pos.y < down)
                    down = pos.y;
                if (pos.y > up)
                    up = pos.y;
            }
        }

        Center = new Vector2((left + right) * 0.5f, (down + up) * 0.5f);
    }

    public void SpreadHeat()
    {
        Block block;
        List<float> heatQueue = new List<float>();
        float heatLoss;
        int gx = gridSize.x;
        int gy = gridSize.y;

        for (int i = 0; i < blockGrid.Count; i++)
        {
            heatQueue.Add(0f);
        }

        for (int i = 0; i < blockGrid.Count; i++)
        {
            block = blockGrid[i];
            if (block != null)
            {
                heatLoss = block.Heat * block.HeatConductivity * Time.deltaTime;
                heatQueue[i] -= heatLoss;

                if ((i % gx) - 1 >= 0)
                    heatQueue[i - 1] += heatLoss * 0.25f;
                if ((i % gx) + 1 < gx)
                    heatQueue[i + 1] += heatLoss * 0.25f;
                if (i - gx >= 0)
                    heatQueue[i - gx] += heatLoss * 0.25f;
                if (i + gx < gx * gy)
                    heatQueue[i + gx] += heatLoss * 0.25f;
            }
        }

        for (int i = 0; i < blockGrid.Count; i++)
        {
            block = blockGrid[i];
            if (block != null)
            {
                block.Heat += heatQueue[i];
                block.Heat = Mathf.Max(block.Heat, 0f);
            }
        }
    }

    public void CalculateHeat()
    {
        SpreadHeat();
    }

    private void FloodFill(int pos, float direction)
    {
        recursion++;
        if (recursion > 9999)
            return;
        int x = pos % gridSize.x;
        int y = Mathf.FloorToInt(pos / gridSize.x);
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y || floodGrid[pos] || blockGrid[pos] == null || (blockGrid[pos].Directional && blockGrid[pos].Angle != direction))
            return;
        else
        {
            floodGrid[pos] = true;
            if (blockGrid[pos].Directional)
                return;

            if (x + 1 < gridSize.x)
                FloodFill(pos + 1, -90f);
            if (x > 0)
                FloodFill(pos - 1, 90f);
            if (y + 1 < gridSize.y)
                FloodFill(pos + gridSize.x, 180f);
            if (y > 0)
                FloodFill(pos - gridSize.x, 0f);
        }
    }

    public void RemoveUnconnectedBlocks()
    {
        recursion = 0;
        floodGrid.Clear();
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            floodGrid.Add(false);
        }
        FloodFill(CorePosition, 0f);
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            if (floodGrid[i] == false && blockGrid[i] != null)
            {
                blockGrid[i].Falloff(true);
            }
        }
        CalculateCenterOfMass();
    }

    public void DeleteVehicle()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].Falloff(false);
        }

        if (PlayerIndex == 2)
            CorePositions.instance.Core2Enabled = false;
        else
            CorePositions.instance.Core1Enabled = false;

        if (BattleManager.instance != null && BattleManager.instance.BattleStarted)
        {
            string winningPlayer = "Player" + (3f - PlayerIndex).ToString();
            BattleManager.instance.EndBattle(winningPlayer + " Wins", 2f);
        }
        Destroy(gameObject);
    }

    public void SetCoreTarget()
    {
        if (PlayerIndex == 2)
        {
            CorePositions.instance.Core2Enabled = true;
            CorePositions.instance.Core2 = blockGrid[CorePosition].gameObject.transform.position;
        }
        else
        {
            CorePositions.instance.Core1Enabled = true;
            CorePositions.instance.Core1 = blockGrid[CorePosition].gameObject.transform.position;
        }
    }

    public void UpdateEnergyUI()
    {
        if (EnergyCapacity == 0f)
            EnergyText.text = "0%";
        else
            EnergyText.text = Mathf.Floor((Energy / EnergyCapacity) * 100f) + "%";
    }

    public void UpdatePlasmaUI()
    {
        if (PlasmaCapacity == 0f)
            PlasmaText.text = "0";
        else
            PlasmaText.text = Plasma.ToString();
    }

    public void SetCameraTarget()
    {
        if (PlayerIndex == 2)
        {
            cameraController.Target2 = Center;
        }
        else
        {
            cameraController.Target = Center;
        }

        cameraController.Lerp = false;
    }

    public void Start()
    {
        CalculateCenterOfMass();
        SetupEnergyUI();
        SetupPlasmaUI();
        if (PlayerIndex == 2)
        {
            ReverseP2 = true;
            EnemyLayer = "Team1";
        }
        else
        {
            EnemyLayer = "Team2";
        }
    }

    public void Update()
    {
        CalculateCenterOfVehicle();
        CalculateHeat();
        SetCoreTarget();
        UpdateEnergyUI();
        UpdatePlasmaUI();
        SetCameraTarget();
    }
}
