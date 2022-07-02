using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock2D/Behavior/Circle Radius")]
public class CircleRadiusBehavior2D : FilterBehavior2D
{
    public Vector2 center;
    public int radius = 60;

    public override Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock)
    {
        Vector2 centerOffset = (center - (Vector2)agent.transform.position);

        float t = centerOffset.sqrMagnitude / radius;
        if(t < 0.9f)
        {
            return Vector2.zero;
        }

        return centerOffset * t * t;
    }

}
