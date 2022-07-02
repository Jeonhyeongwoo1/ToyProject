using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock2D : MonoBehaviour
{
    [Range(0, 500)] public int startingCount;
    public FlockAgent2D flockAgent;
    public FlockBehavior2D flockBehavior2D;
    [Range(0, 10)] public float moveSpeed;
    [Range(0, 10)] public float neighborRadius;
    [Range(0, 10)] public float avoidanceRadiusMulitplier;
    [Range(0, 1)] public float agentDensity = 0.08f;
    [Range(0, 10)] public float driveFactor = 10;

    float squareNeighborRadius;
    float squareAvoidanceRadius;
    float squareMaxMoveSpeed;

    public float SquareAvoidanceRadius { get => squareAvoidanceRadius; }
    public List<FlockAgent2D> agentList = new List<FlockAgent2D>();

    // Start is called before the first frame update
    void Start()
    {
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMulitplier;
    
        for(int i = 0; i < startingCount; i++)
        {
            FlockAgent2D newAgent = Instantiate<FlockAgent2D>(flockAgent, 
                                    Random.insideUnitCircle * startingCount * agentDensity,
                                    Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 300)))
                                    , transform);

            agentList.Add(newAgent);
            newAgent.Initialize(this);
            newAgent.name = "Agent " + i;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent2D agent in agentList)
        {
            List<Transform> context = GetNearNeighbor(agent);
            Vector2 move = flockBehavior2D.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxMoveSpeed)
            {
                move = move.normalized * moveSpeed;
            }

            agent.Move(move);
        }
    }

    List<Transform> GetNearNeighbor(FlockAgent2D agent)
    {
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        List<Transform> list = new List<Transform>();
        for(int i = 0; i < neighbors.Length; i++)
        {
            if (agent.collider != neighbors[i])
                list.Add(neighbors[i].transform);
        }

        return list;
    }
}
