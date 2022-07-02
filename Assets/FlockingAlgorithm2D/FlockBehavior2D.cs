using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior2D : ScriptableObject
{
    public abstract Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock);
}
