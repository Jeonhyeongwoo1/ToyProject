using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Config/Create itemData ScriptableObject")]
public class ItemData : ScriptableObject
{
    public string ItemName => _ItemName;
    public string Description => _Description;
    public string Type => _Type;

    private string _ItemName;
    private string _Description;
    private string _Type;
}
