using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inputs", menuName = "Inputs")]

public class Inputs : ScriptableObject
{
    public List<string> Buttons = new List<string>();
    public List<bool> Axis = new List<bool>();
    public float AxisThreshold = 0.9f;
}
