using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock2D/Filter/Same Filter")]
public class SameFilterBehavior : ContextFilter2D
{

    public override List<Transform> Filter(FlockAgent2D agent, List<Transform> context)
    {
        if(agent.flock2D == null)
        {
            return null;
        }

        List<Transform> agents = new List<Transform>();
        for(int i = 0; i < context.Count; i++)
        {
            FlockAgent2D flockAgent2D = context[i].GetComponent<FlockAgent2D>();
            if(flockAgent2D != null && flockAgent2D.flock2D == agent.flock2D)
            {
                agents.Add(context[i]);
            }
        }

        return agents;
    }

}
