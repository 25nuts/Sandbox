using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "BlockTypeList", menuName = "BlockTypeList")]

public class BlockTypeList : ScriptableObject
{
    public List<BlockList> BlockTypes = new List<BlockList>();
}
