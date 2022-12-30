using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeartManager))]
public class HeartEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // _objectType 필드를 가져옵니다.
        var type = (HeartShowType)serializedObject.FindProperty("_HeartShowType").intValue;
        // _objectType 필드를 Inspector에 노출 시켜줍니다.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_HeartShowType"));

        switch (type)
        {
            case HeartShowType.Center:
            break;
            case HeartShowType.Background:
            break;
        }

    }
}
