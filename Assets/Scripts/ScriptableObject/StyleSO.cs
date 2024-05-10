using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "StyleSO", menuName = "StyleSO", order = 0)]
public class StyleSO : ScriptableObject
{
    public List<StyleData> styleDatas;

    public StyleSheet GetStyle(StyleType _type)
    {
        return styleDatas.FirstOrDefault(x => x.type == _type).style;
    }
}


[Serializable]
public struct StyleData
{
    public StyleType type;
    public StyleSheet style;
}


public enum StyleType
{
    CreateWindow,
    EditWindow,
}