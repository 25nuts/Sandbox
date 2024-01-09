using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Block", menuName = "Block")]

public class BlockData : ScriptableObject
{
    public string BlockName = "Empty";
    public Sprite BlockSprite;
    public Sprite UISprite;
    [TextArea(1, 5)]
    public string Description;
    public bool Core;
    public Vector2 BlockID;

    public float Cost;
    public float HP = 100f;
    public float Defence;
    public float Weight = 1f;

    public float HeatConductivity = 0.1f;
    public float MeltingPoint = 100f;

    public bool CustomButton;
    public bool ReverseP2;
    public int Button;

    public bool Rotatable;
    public bool CustomAngle;
    public bool Directional;
    public Sprite ExtraSprite;
    public float Angle;

    public List<string> ExtraVariableNames = new List<string>();
    public List<float> ExtraVariables = new List<float>();

    public string Script = "Block";
}
