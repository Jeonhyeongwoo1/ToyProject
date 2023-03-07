using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dot
{
    [CustomEditor(typeof(Dot))]
    public class DotEditor : Editor
    {

        private void OnSceneGUI()
        {
            Dot dot = (Dot)target;
            float viewRadius = 5;
            Handles.DrawWireArc(dot.transform.position, Vector3.up, Vector3.forward, 360, viewRadius);

            Vector3 left = dot.DistToFromAngle(false, dot.Angle * 0.5f * -1);
            Vector3 right = dot.DistToFromAngle(false, dot.Angle * 0.5f);

            Handles.DrawLine(dot.transform.position, dot.transform.position + left * viewRadius);
            Handles.DrawLine(dot.transform.position, dot.transform.position + right * viewRadius);

        }
    }
}
