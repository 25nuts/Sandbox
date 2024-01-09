using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveFileManager : MonoBehaviour
{
    public string BlueprintName;
    public int CurrentBlueprintIndex;
    public string CurrentBlueprint;
    public string BlueprintList;
    public List<string> Blueprints = new List<string>();

    public Image errorBox;
    public TextMeshProUGUI errorMessage;

    public TMP_InputField inputField;
    public Button DeleteButton;
    public TMP_Dropdown LoadDropdown;
    public Button SaveButton;
    public Button SaveAsNewButton;

    public Button BackButton;

    public EditorController controller;

    public void Awake()
    {
        BlueprintName = "";
        CurrentBlueprintIndex = -1;
    }

    public void DisplayError(string text)
    {
        errorBox.gameObject.SetActive(true);
        errorMessage.text = text;
    }

    public void HideUI()
    {
        inputField.gameObject.SetActive(false);
        DeleteButton.gameObject.SetActive(false);
        LoadDropdown.gameObject.SetActive(false);
        SaveButton.gameObject.SetActive(false);
        SaveAsNewButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        inputField.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
        LoadDropdown.gameObject.SetActive(true);
        SaveButton.gameObject.SetActive(true);
        SaveAsNewButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
    }

    public void ChangeName()
    {
        string name = inputField.text;
        if (name.Contains(";") || name.Contains("/"))
        {
            DisplayError("Invalid Name");
            inputField.text = BlueprintName;
        }
        else
        {
            BlueprintName = name;
        }
    }

    private bool CheckName()
    {
        return (BlueprintName.Contains(";") || BlueprintName.Contains("/") || BlueprintName == "");
    }

    public string GenerateBlueprintString()
    {
        string blueprint;

        blueprint = BlueprintName + ";";
        blueprint += "8x6;";

        for (int i = 0; i < controller.BluePrint.Count; i++)
        {
            BlockData block = controller.BluePrint[i];

            if (block.BlockName != "Empty")
            { 
                blueprint += block.BlockID.x.ToString() + ",";
                blueprint += block.BlockID.y.ToString() + ",";

                blueprint += block.Button.ToString() + ",";
                blueprint += block.ReverseP2.ToString() + ",";
                blueprint += block.Angle.ToString() + ",";

                blueprint += ";";
            }
            else
            {
                blueprint += "null;";
            }
        }

        blueprint += "/";
        return blueprint;
    }

    public void CombineList()
    {
        string combination = "";
        for (int i = 0; i < Blueprints.Count; i++)
        {
            combination += Blueprints[i];
        }
        BlueprintList = combination;
    }

    public void SaveToTxt()
    {
        CombineList();

        string blueprintData = BlueprintList;
        string filePath = Application.persistentDataPath + "/BlueprintList.txt";
        System.IO.File.WriteAllText(filePath, blueprintData);
    }
    public void ExpandList()
    {
        string[] blueprintArray = BlueprintList.Split(char.Parse("/"));

        for (int i = 0; i < blueprintArray.Length; i++)
        {
            if (blueprintArray[i] != "")
                Blueprints.Add(blueprintArray[i] + "/");
        }
    }

    public void LoadFromTxt()
    {
        string filePath = Application.persistentDataPath + "/BlueprintList.txt";
        BlueprintList = System.IO.File.ReadAllText(filePath);

        ExpandList();
    }


    public void FillLoadDropdown()
    {
        string name;
        string[] blueprintArray;
        LoadDropdown.ClearOptions();
        LoadDropdown.options.Add(new TMP_Dropdown.OptionData(""));
        for (int i = 0; i < Blueprints.Count; i++)
        {
            blueprintArray = Blueprints[i].Split(char.Parse(";"));
            name = blueprintArray[0];
            LoadDropdown.options.Add(new TMP_Dropdown.OptionData(name));
        }
        LoadDropdown.RefreshShownValue();
    }

    public void SaveAsNew()
    {
        if (CheckName())
        {
            DisplayError("Invalid Name");
            return;
        }

        if (!controller.testBlueprint.TestVehicle())
            return;

        CurrentBlueprintIndex = Blueprints.Count;
        CurrentBlueprint = GenerateBlueprintString();
        BlueprintList += CurrentBlueprint;
        Blueprints.Add(CurrentBlueprint);
        SaveButton.interactable = true;
        LoadDropdown.interactable = true;

        SaveToTxt();
        FillLoadDropdown();
    }

    public void Load()
    {
        if (LoadDropdown.value != 0)
        {
            CurrentBlueprintIndex = LoadDropdown.value - 1;
            CurrentBlueprint = Blueprints[CurrentBlueprintIndex];

            string[] blueprint = CurrentBlueprint.Split(char.Parse(";"));

            BlueprintName = blueprint[0];
            inputField.text = BlueprintName;

            controller.Budget = controller.BudgetLimit;
            controller.CorePlaced = false;
            for (int i = 2; i < blueprint.Length - 1; i++)
            {
                controller.AddBlock(blueprint[i], i - 2);
            }

            controller.SelectedGridCell = -1;
            SaveButton.interactable = true;
        }
    }
    
    public void Save()
    {
        if (CurrentBlueprintIndex == -1)
            return;

        if (CheckName())
        {
            DisplayError("Invalid Name");
            return;
        }

        if (!controller.testBlueprint.TestVehicle())
            return;

        CurrentBlueprint = GenerateBlueprintString();
        Blueprints[CurrentBlueprintIndex] = CurrentBlueprint;
        LoadDropdown.interactable = true;

        SaveToTxt();
        FillLoadDropdown();
    }

    public void Delete()
    {
        inputField.text = "";
        BlueprintName = "";
        controller.Budget = controller.BudgetLimit;
        controller.CorePlaced = false;

        for (int i = 0; i < 48; i++)
        {
            controller.AddBlock("null", i);
        }

        controller.SelectedGridCell = -1;

        if (CurrentBlueprintIndex == -1)
            return;

        Blueprints.RemoveAt(CurrentBlueprintIndex);

        SaveButton.interactable = false;
        if (Blueprints.Count == 0)
            LoadDropdown.interactable = false;

        CurrentBlueprintIndex = -1;

        SaveToTxt();
        FillLoadDropdown();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Start()
    {
        CurrentBlueprintIndex = -1;

        if (System.IO.File.Exists(Application.persistentDataPath + "/BlueprintList.txt"))
        {
            LoadFromTxt();
            FillLoadDropdown();
            if (!(Blueprints.Count > 0))
                LoadDropdown.interactable = false;
        }
        else
        {
            LoadDropdown.interactable = false;
        }
    }
}
