using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class CustomLevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        LevelData levelData = target as LevelData;

        if (GUILayout.Button("Load All Levels"))
        {
            var levels = Resources.LoadAll<LevelSO>("Levels");
            levelData.levels.Clear();
            foreach (var level in levels)
            {
                levelData.levels.Add(level);
            }
        }
    }
}
