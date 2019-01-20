using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillData
{
    public Skills skill;
    public int time;
    public string data;
}

public enum Skills
{
    None = 0,
    Flip_Horizontal = 1,
    Flip_Vertical = 2,
    Flip_HandV = 3,
    Color_BW = 4,
    Color_Nega = 5,
    Fix_Camera = 6
}