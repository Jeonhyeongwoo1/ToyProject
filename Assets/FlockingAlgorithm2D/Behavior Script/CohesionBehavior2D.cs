using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behavior/Cohesion")]
public class CohesionBehavior2D : FilterBehavior2D
{

    public override Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock)
    {

        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 offset = Vector2.zero;
        List<Transform> agents = (contextFilter == null) ? context : contextFilter.Filter(agent, context);
        for (int i = 0; i < agents.Count; i++)
        {
            offset += (Vector2)context[i].transform.position;
        }

        offset /= context.Count;
        offset = offset - (Vector2)agent.transform.position;

        return offset;
    }

}
