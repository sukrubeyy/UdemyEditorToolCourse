using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameInfo", menuName = "Create Game Info", order = 0)]
public class GameInfo : ScriptableObject
{
    public int levelIndex;
    public LevelSO currentLevel;
}
