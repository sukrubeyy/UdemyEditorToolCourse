using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Create New Level", order = 0)]
public class LevelSO : ScriptableObject
{
    public List<string> searchWords = new List<string>();
    public int time;
    public Grid grid;
}

[Serializable]
public class Grid
{
    public Column[] columns;
    public int xSize;
    public int ySize;

    public Grid(int _xSize, int _ySize)
    {
        xSize = _xSize;
        ySize = _ySize;


        columns = new Column[ySize];

        for (int y = 0; y < ySize; y++)
        {
            columns[y] = new Column(xSize);
        }
    }

    public void SetLetter()
    {
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                columns[y].rows[x] = "A";
            }
        }
    }

    public void Clear()
    {
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                columns[y].rows[x] = "";
            }
        }
    }
}

[Serializable]
public class Column
{
    public string[] rows;
    public Column(int sizeX)
    {
        rows = new string[sizeX];
        for (int x = 0; x < sizeX; x++)
        {
            rows[x] = "";
        }
    }
}
