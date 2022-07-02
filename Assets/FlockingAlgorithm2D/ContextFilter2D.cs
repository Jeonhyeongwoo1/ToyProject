using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextFilter2D : ScriptableObject
{
    public abstract List<Transform> Filter(FlockAgent2D agent, List<Transform> context);
}
