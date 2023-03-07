using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TrigonometricFunction
{
    [CustomEditor(typeof(TrigonometricFunction))]
    public class TrigonometricFunctionEditor : Editor
    {
        private void OnSceneGUI()
        {
            TrigonometricFunction targetFunction = (TrigonometricFunction)target;

            float viewRadius = 5;
            Handles.DrawWireArc(targetFunction.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);

            Vector3 left = targetFunction.DistToFromAngle(-targetFunction.ViewAngle / 2, false);
            Vector3 right = targetFunction.DistToFromAngle(targetFunction.ViewAngle / 2, false);

            Vector3 pos = targetFunction.transform.position;
            Handles.DrawLine(pos, pos + left * viewRadius);
            Handles.DrawLine(pos, pos + right * viewRadius);

        }
    }
}