using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

[CustomEditor(typeof(LevelSO))]
public class CustomLevelEditor : Editor
{
    private bool isChanged;
    public override void OnInspectorGUI()
    {
        LevelSO level = target as LevelSO;

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Search Words");
            DrawHorizontalLine(1f, Color.red, 3);

            for (int i = 0; i < level.searchWords.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"Words {i + 1} :", GUILayout.Width(100));
                    level.searchWords[i] = EditorGUILayout.TextField(level.searchWords[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();


        DrawHorizontalLine(1f, Color.red, 10);

        for (int y = 0; y < level.grid.ySize; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < level.grid.xSize; x++)
            {
                level.grid.columns[y].rows[x] = EditorGUILayout.TextField(level.grid.columns[y].rows[x], TextFieldStyle);
            }
            EditorGUILayout.EndHorizontal();
        }

        DrawHorizontalLine(1f, Color.red, 10);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Clear"))
            {
                level.grid.Clear();
            }

            if (GUILayout.Button("Play This Level"))
            {
                var GameInfo = Resources.Load<GameInfo>("GameInfo");
                GameInfo.currentLevel = level;
                var index = level.name.Split("Level");
                GameInfo.levelIndex = Convert.ToInt32(index[1]);

                EditorApplication.EnterPlaymode();
            }
            if (GUILayout.Button("Randomize"))
            {
                for (int y = 0; y < level.grid.ySize; y++)
                    for (int x = 0; x < level.grid.xSize; x++)
                    {
                        var errorCount = Regex.Matches(level.grid.columns[y].rows[x], @"[a-zA-Z0-9ğüşöçıİĞÜŞÖÇ]+$").Count;
                        var turkishLetters = "ABCÇDEFGĞHİIJKLMNOÖPRSŞTUÜVYZ";

                        int index = UnityEngine.Random.Range(0, turkishLetters.Length);

                        if (errorCount == 0)
                        {
                            level.grid.columns[y].rows[x] = turkishLetters[index].ToString();
                        }
                    }
            }

            if (GUI.changed)
                isChanged = true;

            if (isChanged)
            {
                if (GUILayout.Button("SAVE"))
                {
                    EditorUtility.SetDirty(level);
                    isChanged = false;
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawHorizontalLine(float thickness, Color color, float space = 0)
    {
        EditorGUILayout.Space(space);
        Rect rect = GUILayoutUtility.GetRect(0f, thickness);
        EditorGUI.DrawRect(rect, color);
        EditorGUILayout.Space(space);
    }

    private GUIStyle TextFieldStyle => new GUIStyle(GUI.skin.textField)
    {
        alignment = TextAnchor.MiddleCenter,
        normal = { textColor = Color.magenta }
    };
}
