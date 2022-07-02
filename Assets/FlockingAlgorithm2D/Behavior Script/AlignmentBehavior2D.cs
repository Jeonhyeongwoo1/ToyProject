using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Behavior/Alignment")]
public class AlignmentBehavior2D : FilterBehavior2D
{
    public override Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock)
    {
        if(context.Count == 0)
        {
            return agent.transform.up;
        }

        Vector2 alignmentMove = Vector2.zero;
        List<Transform> agents = (contextFilter == null) ? context : contextFilter.Filter(agent, context);
        for(int i = 0; i < agents.Count; i++)
        {
            alignmentMove += (Vector2) context[i].transform.up;
        }
        alignmentMove /= context.Count;

        return alignmentMove;

    }


}
