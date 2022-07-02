using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Filter/Obstcle Filter")]
public class ObstacleFilterBehavior : ContextFilter2D
{
    public LayerMask obstcle;

    public override List<Transform> Filter(FlockAgent2D agent, List<Transform> context)
    {
        if(context.Count == 0)
        {
            return null;
        }

        List<Transform> obstcles = new List<Transform>();
        for(int i = 0; i < context.Count; i++)
        {
            if(obstcle ==  (obstcle | 1 << context[i].gameObject.layer))
            {
                obstcles.Add(context[i]);
            }
        }

        return obstcles;
    }
}
