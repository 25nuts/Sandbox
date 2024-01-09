using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlockInspector : MonoBehaviour
{
    public Image InspectorBox;

    public Image BudgetDisplay;
    public TextMeshProUGUI BudgetUI;

    public TextMeshProUGUI nameUI;
    public Image imageUI;
    public TextMeshProUGUI DescriptionUI;

    public TextMeshProUGUI CostUI;
    public TextMeshProUGUI HPUI;
    public TextMeshProUGUI DefenceUI;
    public TextMeshProUGUI WeightUI;
    public TextMeshProUGUI HeatConductivityUI;
    public TextMeshProUGUI MeltingPointUI;

    public List<TextMeshProUGUI> ExtraVariablesUI = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> ExtraVariableNamesUI = new List<TextMeshProUGUI>();

    public TMP_Dropdown ButtonDropdown;
    public Toggle ReverseButton;
    public TextMeshProUGUI AngleUI;
    public TextMeshProUGUI AngleN;
    public Slider AngleSlider;
    public TextMeshProUGUI Rotatable;

    private EditorController controller;

    void Awake()
    {
        controller = GetComponent<EditorController>();
    }

    public void RefreshInspector(BlockData selectedBlock)
    {
        nameUI.text = selectedBlock.BlockName;
        imageUI.sprite = selectedBlock.UISprite;
        DescriptionUI.text = selectedBlock.Description;

        CostUI.text = selectedBlock.Cost.ToString();
        HPUI.text = selectedBlock.HP.ToString();
        DefenceUI.text = selectedBlock.Defence.ToString();
        WeightUI.text = selectedBlock.Weight.ToString();
        HeatConductivityUI.text = selectedBlock.HeatConductivity.ToString();
        MeltingPointUI.text = selectedBlock.MeltingPoint.ToString();

        int i = 0;
        foreach (var item in ExtraVariablesUI)
        {
            if (selectedBlock.ExtraVariables.Count > i)
            {
                ExtraVariablesUI[i].text = selectedBlock.ExtraVariables[i].ToString();
                ExtraVariableNamesUI[i].text = selectedBlock.ExtraVariableNames[i];
            }
            else
            {
                ExtraVariablesUI[i].text = "";
                ExtraVariableNamesUI[i].text = "";
            }
            i++;
        }

        if (selectedBlock.CustomButton)
        {
            ButtonDropdown.gameObject.SetActive(true);
            ButtonDropdown.value = selectedBlock.Button;
            ReverseButton.gameObject.SetActive(true);
            ReverseButton.isOn = selectedBlock.ReverseP2;
        }
        else
        {
            ButtonDropdown.gameObject.SetActive(false);
            ReverseButton.gameObject.SetActive(false);
        }

        if (selectedBlock.CustomAngle)
        {
            AngleUI.text = "Angle";
            AngleN.text = selectedBlock.Angle.ToString();
            AngleSlider.gameObject.SetActive(true);
            AngleSlider.value = selectedBlock.Angle;
        }
        else
        {
            AngleUI.text = "";
            AngleN.text = "";
            AngleSlider.gameObject.SetActive(false);
        }

        if (selectedBlock.Rotatable)
        {
            Rotatable.gameObject.SetActive(true);
        }
        else
        {
            Rotatable.gameObject.SetActive(false);
        }
    }

    public void ShowUI()
    {
        InspectorBox.gameObject.SetActive(true);
        BudgetDisplay.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        InspectorBox.gameObject.SetActive(false);
        BudgetDisplay.gameObject.SetActive(false);
    }

    public void ChangeButton()
    {
        if (controller.SelectedGridCell != -1)
        {
            controller.BluePrint[controller.SelectedGridCell].Button = ButtonDropdown.value;
        }
    }

    public void ChangeReverse()
    {
        if (controller.SelectedGridCell != -1)
        {
            controller.BluePrint[controller.SelectedGridCell].ReverseP2 = ReverseButton.isOn;
        }
    }

    public void ChangeAngle()
    {
        if (controller.SelectedGridCell != -1)
        {
            controller.BluePrint[controller.SelectedGridCell].Angle = AngleSlider.value;
            AngleN.text = AngleSlider.value.ToString();
        }
    }

    public void Update()
    {
        BudgetUI.text = "$" + controller.Budget.ToString();
    }
}
