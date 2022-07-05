using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;
    private UIGrid uIGrid;
    // Start is called before the first frame update
    void Start()
    {
        uIGrid = new UIGrid(4, 4, 100, prefab, parent);
    }

    void Update() {
        
    }

}
