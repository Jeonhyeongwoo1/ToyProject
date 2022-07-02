using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behavior/Avoidance")]
public class AvoidanceBehavior2D : FilterBehavior2D
{

    public override Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock)
    {
        if(context.Count == 0)
        {
            return Vector2.zero;
        }
        
        Vector2 move = Vector2.zero;
        int avoid = 0;
        List<Transform> agents = (contextFilter == null) ? context : contextFilter.Filter(agent, context);
        for(int i = 0; i < agents.Count; i++)
        {
            if(Vector2.SqrMagnitude(context[i].transform.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                avoid++;
                move += (Vector2)(agent.transform.position - context[i].transform.position);
            }

        }

        if (avoid > 0)
            move /= avoid;

        return move;
    }

}
