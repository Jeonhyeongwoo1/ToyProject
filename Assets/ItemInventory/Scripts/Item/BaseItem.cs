using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Portion,
    ETC,
    Weapon,
    Armor,
    ACC
}

public class BaseItem : MonoBehaviour
{
    public ItemType ItemType => itemType;

    [SerializeField] protected ItemType itemType;
}