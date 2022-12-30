using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get { return instance; }
        set { }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        else
        {
            Debug.LogWarning($"Singletone is already exist - {name}");
            DestroyImmediate(this);
        }
    }
}