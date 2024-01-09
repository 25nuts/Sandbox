using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BlueprintSelection : MonoBehaviour
{
    public List<GameObject> BlueprintList = new List<GameObject>();
    public List<GameObject> BlueprintList2 = new List<GameObject>();

    public GameObject PlayerAidText1;
    public GameObject PlayerAidText2;

    [SerializeField]
    private int ScrollButton;
    [SerializeField]
    private int FastScrollButton;
    [SerializeField]
    private int FastScrollSpeed;
    [SerializeField]
    private int ConfirmButton;

    public void Start()
    {
        BattleManager.instance.SelectedBlueprintIndex1 = 0;
        BattleManager.instance.SelectedBlueprintIndex2 = 0;
    }

    private void ChangeIndex()
    {
        if (!(BattleManager.instance.Player1Ready))
        {
            if (BattleManager.instance.Player1Controlls.GetButton(ScrollButton, false, true))
                BattleManager.instance.SelectedBlueprintIndex1++;
            if (BattleManager.instance.Player1Controlls.GetButton(ScrollButton, true, true))
                BattleManager.instance.SelectedBlueprintIndex1--;
            if (BattleManager.instance.Player1Controlls.GetButton(FastScrollButton, false, true) && FastScrollSpeed < BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex1 += FastScrollSpeed;
            if (BattleManager.instance.Player1Controlls.GetButton(FastScrollButton, true, true) && FastScrollSpeed < BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex1 -= FastScrollSpeed;

            if (BattleManager.instance.SelectedBlueprintIndex1 >= BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex1 -= BattleManager.instance.Blueprints.Count;
            if (BattleManager.instance.SelectedBlueprintIndex1 < 0)
                BattleManager.instance.SelectedBlueprintIndex1 += BattleManager.instance.Blueprints.Count;
        }
        if (!(BattleManager.instance.Player2Ready))
        {
            if (BattleManager.instance.Player2Controlls.GetButton(ScrollButton, false, true))
                BattleManager.instance.SelectedBlueprintIndex2++;
            if (BattleManager.instance.Player2Controlls.GetButton(ScrollButton, true, true))
                BattleManager.instance.SelectedBlueprintIndex2--;
            if (BattleManager.instance.Player2Controlls.GetButton(FastScrollButton, false, true) && FastScrollSpeed < BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex2 += FastScrollSpeed;
            if (BattleManager.instance.Player2Controlls.GetButton(FastScrollButton, true, true) && FastScrollSpeed < BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex2 -= FastScrollSpeed;

            if (BattleManager.instance.SelectedBlueprintIndex2 >= BattleManager.instance.Blueprints.Count)
                BattleManager.instance.SelectedBlueprintIndex2 -= BattleManager.instance.Blueprints.Count;
            if (BattleManager.instance.SelectedBlueprintIndex2 < 0)
                BattleManager.instance.SelectedBlueprintIndex2 += BattleManager.instance.Blueprints.Count;
        }
    }

    private void SelectDeselect()
    {
        if (BattleManager.instance.Player1Controlls.GetButton(ConfirmButton, false, true))
        {
            BattleManager.instance.Player1Ready = true;
        }
        if (BattleManager.instance.Player1Controlls.GetButton(ConfirmButton, true, true))
        {
            BattleManager.instance.Player1Ready = false;
        }

        if (BattleManager.instance.Player2Controlls.GetButton(ConfirmButton, false, true))
        {
            BattleManager.instance.Player2Ready = true;
        }
        if (BattleManager.instance.Player2Controlls.GetButton(ConfirmButton, true, true))
        {
            BattleManager.instance.Player2Ready = false;
        }
    }

    public void DisplayBlueprint(List<GameObject> list, int index, int i, bool one)
    {
        int n = index + i - 2;
        if (n < 0 || n >= BattleManager.instance.Blueprints.Count || (one && i != 2))
        {
            list[i].SetActive(false);
            return;
        }
        list[i].SetActive(true);
        list[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = BattleManager.instance.BlueprintNames[n];
    }

    public void DisplayBlueprints(List<GameObject> list, int index, bool one)
    {
        for (int i = 0; i < list.Count; i++)
        {
            DisplayBlueprint(list, index, i, one);
        }
    }

    private void HideList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(false);
        }
    }

    public void HideUI()
    {
        HideList(BlueprintList);
        HideList(BlueprintList2);
        PlayerAidText1.SetActive(false);
        PlayerAidText2.SetActive(false);
    }

    public void Update()
    {
        ChangeIndex();
        SelectDeselect();
        DisplayBlueprints(BlueprintList, BattleManager.instance.SelectedBlueprintIndex1, BattleManager.instance.Player1Ready);
        DisplayBlueprints(BlueprintList2, BattleManager.instance.SelectedBlueprintIndex2, BattleManager.instance.Player2Ready);
    }
}
