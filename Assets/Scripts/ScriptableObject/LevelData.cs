using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "Create Level Data", order = 0)]
public class LevelData : ScriptableObject
{
    public List<LevelSO> levels;
}
