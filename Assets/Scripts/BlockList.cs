using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "BlockList", menuName = "BlockList")]

public class BlockList : ScriptableObject
{
    public Sprite UIicon;
    public List<BlockData> Blocks = new List<BlockData>();
}
