using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FiedlOfViewEditor : Editor
{
    void OnSceneGUI() {
        FieldOfView fow = (FieldOfView) target;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);

        Vector3 left = fow.DirFromAngle(fow.viewAngle * 0.5f, false);
        Vector3 right = fow.DirFromAngle(-fow.viewAngle * 0.5f, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + left * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + right * fow.viewRadius);

        foreach(Transform target in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, target.position);
        }
    }
}
