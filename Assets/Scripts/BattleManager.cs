using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public BlockTypeList blockTypeList;

    public float StartDistance;
    public float BattleMaxTime;

    [HideInInspector]
    public float BattleStartTime;
    [HideInInspector]
    public float BattleCurrentTime;

    public InputController Player1Controlls;
    public InputController Player2Controlls;
    [HideInInspector]
    public int SelectedBlueprintIndex1;
    [HideInInspector]
    public int SelectedBlueprintIndex2;
    [HideInInspector]
    public bool Player1Ready;
    [HideInInspector]
    public bool Player2Ready;

    [HideInInspector]
    public string BlueprintList;
    [HideInInspector]
    public List<string> Blueprints = new List<string>();
    [HideInInspector]
    public List<string> BlueprintNames = new List<string>();

    [HideInInspector]
    public List<BlockData> Blueprint1 = new List<BlockData>();
    [HideInInspector]
    public List<BlockData> Blueprint2 = new List<BlockData>();

    [HideInInspector]
    public bool BattleStarted = false;
    [HideInInspector]
    public bool BattleFinished = false;

    public Image EnergyUI1;
    public Image EnergyUI2;
    public Image PlasmaUI1;
    public Image PlasmaUI2;

    public TextMeshProUGUI BattleTimer;

    public GameObject ResultsSceen;

    public GameObject BackButton;

    [HideInInspector]
    public BlueprintSelection blueprintSelection;
    [HideInInspector]
    public SaveFileLoader saveFileLoader;

    public BuildBlueprint BlueprintBuilder;

    void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Awake()
    {
        CreateSingleton();

        blueprintSelection = GetComponent<BlueprintSelection>();
        saveFileLoader = GetComponent<SaveFileLoader>();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void StartBattle()
    {
        blueprintSelection.HideUI();
        blueprintSelection.enabled = false;
        BackButton.SetActive(false);
        BattleTimer.gameObject.SetActive(true);
        Cursor.visible = false;

        saveFileLoader.LoadBlueprint(Blueprints[SelectedBlueprintIndex1], 1);
        saveFileLoader.LoadBlueprint(Blueprints[SelectedBlueprintIndex2], 2);

        BlueprintBuilder.Center.x = 0f - StartDistance;
        BlueprintBuilder.EnergyUI = EnergyUI1;
        BlueprintBuilder.PlasmaUI = PlasmaUI1;
        BlueprintBuilder.GenerateBlueprint(Blueprint1, 1, Player1Controlls.inputs, "Team1");

        BlueprintBuilder.Center.x = StartDistance;
        BlueprintBuilder.EnergyUI = EnergyUI2;
        BlueprintBuilder.PlasmaUI = PlasmaUI2;
        BlueprintBuilder.GenerateBlueprint(Blueprint2, 2, Player2Controlls.inputs, "Team2");

        BattleStartTime = Time.timeSinceLevelLoad;
        BattleStarted = true;
    }

    public void EndBattle (string Results, float WaitTime)
    {
        StartCoroutine(EndBattleCoroutine(Results, WaitTime));
    }

    public IEnumerator EndBattleCoroutine(string Results, float WaitTime)
    {
        if (WaitTime > 0f)
        {
            yield return new WaitForSeconds(WaitTime);
        }

        if (BattleFinished)
            yield break;

        ResultsSceen.SetActive(true);
        ResultsSceen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Results;

        BackButton.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible = true;
        BattleStarted = false;
        BattleFinished = true;
    }

    public bool EndBattleButton()
    {
        return (Input.GetKey(KeyCode.Backspace) && Input.GetKey(KeyCode.LeftShift));
    }

    public void SetTimer()
    {
        BattleCurrentTime = Time.timeSinceLevelLoad - BattleStartTime;

        float currentTime = Mathf.Floor(BattleMaxTime - BattleCurrentTime);
        currentTime = Mathf.Max(0, currentTime);
        float minutes = Mathf.Floor(currentTime / 60f);
        float seconds = currentTime % 60;

        BattleTimer.text = minutes.ToString() + ":" + seconds.ToString("00");

        if (currentTime <= 0f || EndBattleButton())
        {
            StartCoroutine(EndBattleCoroutine("Tie", 0f));
        }
    }

    public void Update()
    {
        if (!BattleFinished)
        {
            if (Player1Ready &&  Player2Ready && !BattleStarted)
                StartBattle();
            if (BattleStarted)
            {
                SetTimer();
            }
        }
    }
}
