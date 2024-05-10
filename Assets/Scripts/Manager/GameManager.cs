using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameInfo _gameInfo;

    public GameObject letterPrefab;
    public Transform _canvas;

    private void Start()
    {
        var currentLevel = _gameInfo.currentLevel;

        for (int y = 0; y < currentLevel.grid.ySize; y++)
        {
            for (int x = 0; x < currentLevel.grid.xSize; x++)
            {
                var letter = Instantiate(letterPrefab, _canvas).GetComponent<Letter>();
                letter.Initialize(currentLevel.grid.columns[y].rows[x]);
            }
        }

        var gridLayoutGroup = _canvas.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = currentLevel.grid.xSize;
    }
}
