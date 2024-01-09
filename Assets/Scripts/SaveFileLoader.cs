using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SaveFileLoader : MonoBehaviour
{
    public void ExpandList()
    {
        string[] blueprintArray = BattleManager.instance.BlueprintList.Split(char.Parse("/"));

        for (int i = 0; i < blueprintArray.Length; i++)
        {
            if (blueprintArray[i] != "")
                BattleManager.instance.Blueprints.Add(blueprintArray[i] + "/");
        }
    }

    public void LoadFromTxt()
    {
        string filePath = Application.persistentDataPath + "/BlueprintList.txt";
        BattleManager.instance.BlueprintList = System.IO.File.ReadAllText(filePath);

        ExpandList();
    }

    public void GetBlueprintNames()
    {
        string name;
        string[] blueprintArray;

        for (int i = 0; i < BattleManager.instance.Blueprints.Count; i++)
        {
            blueprintArray = BattleManager.instance.Blueprints[i].Split(char.Parse(";"));
            name = blueprintArray[0];
            BattleManager.instance.BlueprintNames.Add(name);
        }
    }

    private void CopyBlockList(List<BlockData> to, List<BlockData> from)
    {
        for (int i = 0; i < from.Count; i++)
        {
            to.Add(from[i]);
        }
    }

    public void LoadBlueprint(string Blueprint, int player)
    {
        string[] blueprint = Blueprint.Split(char.Parse(";"));
        string block;
        List<BlockData> blocks = new List<BlockData>();

        for (int i = 2; i < blueprint.Length - 1; i++)
        {
            block = blueprint[i];

            if (block == "null")
            {
                blocks.Add(BlockData.CreateInstance<BlockData>());
                continue;
            }

            string[] blockValues = block.Split(char.Parse(","));

            int x = int.Parse(blockValues[0]);
            int y = int.Parse(blockValues[1]);

            blocks.Add(BlockData.Instantiate<BlockData>(BattleManager.instance.blockTypeList.BlockTypes[y].Blocks[x]));
            BlockData blockData = blocks[blocks.Count - 1];

            blockData.Button = int.Parse(blockValues[2]);
            blockData.ReverseP2 = bool.Parse(blockValues[3]);
            blockData.Angle = float.Parse(blockValues[4]);
        }

        if (player == 1)
            CopyBlockList(BattleManager.instance.Blueprint1, blocks);
        if (player == 2)
            CopyBlockList(BattleManager.instance.Blueprint2, blocks);
    }

    public void Start()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/BlueprintList.txt"))
        {
            LoadFromTxt();
            GetBlueprintNames();
            if (!(BattleManager.instance.Blueprints.Count > 0))
                BattleManager.instance.MainMenu();
        }
        else
        {
            BattleManager.instance.MainMenu();
        }
    }
}
