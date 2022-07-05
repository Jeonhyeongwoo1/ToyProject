using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGrid : MonoBehaviour
{
   
    private int _width;
    private int _height;
    private float _cellSize;

    private int[,] _gridArray;
    
    public UIGrid(int width, int height, float cellSize, GameObject prefab, Transform parent)
    {
        _width = width;
        _height = height;
        _gridArray = new int[_width, _height];

        for(int i = 0; i < _gridArray.GetLength(0); i++)
        {
            for(int j = 0; j < _gridArray.GetLength(1); j++)
            {
                GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = GetPosition(j, i, cellSize);
            }
        }
    }

    Vector2 GetPosition(int x, int y, float cellSize)
    {
        return new Vector2(Mathf.FloorToInt(x * cellSize), Mathf.FloorToInt(y * cellSize));
    }

}
