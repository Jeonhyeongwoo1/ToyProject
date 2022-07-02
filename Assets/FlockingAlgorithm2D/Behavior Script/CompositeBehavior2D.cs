using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock2D/Behavior/Composite")]
public class CompositeBehavior2D : FlockBehavior2D
{
    public FlockBehavior2D[] behavior2Ds;
    public float[] weights;

    public override Vector2 CalculateMove(FlockAgent2D agent, List<Transform> context, Flock2D flock)
    {

        if(behavior2Ds.Length != weights.Length)
        {
            Debug.LogError("Dismatched Behavior");
            return Vector2.zero;
        }

        Vector2 calculateMove = Vector2.zero;
        Vector2 partialMove = Vector2.zero;
        for (int i = 0; i < behavior2Ds.Length; i++)
        {
            partialMove += behavior2Ds[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove.sqrMagnitude > weights[i] * weights[i])
            {
                partialMove.Normalize();
                partialMove *= weights[i];
            }

            calculateMove += partialMove;
        }

        return calculateMove;
    }

}
