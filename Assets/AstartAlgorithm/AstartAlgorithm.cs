using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AstartAlgorithm : MonoBehaviour
{
    public List<Node> open;
    public List<Node> close;
    
    float[,] graphArray = new float[,] {
        {0, 5.6f, float.MaxValue, 6.8f, float.MaxValue, float.MaxValue, float.MaxValue},
        {5.6f, 0, 4.3f, float.MaxValue, 6.5f, float.MaxValue, float.MaxValue},
        {float.MaxValue, 4.3f, 0, 5.6f, float.MaxValue, 5.8f, 7},
        {6.8f, float.MaxValue, 5.6f, 0, float.MaxValue, 6.5f,float.MaxValue },
        {float.MaxValue, 6.5f, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, 5.2f},
        {float.MaxValue, float.MaxValue, 5.8f, 6.5f, float.MaxValue,0, 5.5f},
        {float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue, 5.2f, 5.5f, 0}
    };

    void StartAstartAlgorithm()
    {

        PriorityQueue priorityQueue = new PriorityQueue();
        Node startNode = new Node(0, 0, 0);
        priorityQueue.Enqueue(startNode);
        open.Add(startNode);

        while(!priorityQueue.IsQueueEmpty())
        {
            Node node = priorityQueue.Dequeue();
            
            int index = node.nodeId;
            for(int i = 0; i < graphArray.GetLength(1); i++)
            {
                /*
                1. 현재 인덱스에 그래프 노드를 순회한다.
                2. close에 노드가 있다면 지나간다
                3. 새로운 노드를 만든다
                4. 새로운 노드가 Open에 있는지 확인한다 있다면 값을 비교하여 더 작을 경우 바꾼다
                5. 새로운 노드를 open, 큐에 넣는다
                */
                if (graphArray[index, i] == 0 || graphArray[index, i] == float.MaxValue) { continue; }
                
                Node closeNode = close.Find((v) => v.nodeId == i);
                if (closeNode != null) { continue; }

                Node newNode = new Node(i, node.G + graphArray[index, i], index);
                if (newNode == null) { break; }

                Node sameNode = open.Find((v)=> v.nodeId == newNode.nodeId);
                if(sameNode != null)
                {
                    if(sameNode.F > newNode.F)
                    {
                        sameNode = newNode;
                    }
                }
                else
                {
                    open.Add(newNode);
                    priorityQueue.Enqueue(newNode);
                }
            }

            open.Remove(node);
            close.Add(node);

            if(node.nodeId == 6)
            {
                break;
            }
        }

        for(int i = 0; i < close.Count; i++)
        {
         //   print(close[i].nodeId);
        }

    }


    Node SearchNodeInfo()
    {
        float min = float.MaxValue;
        Node parent = new Node(0, 0, 0);
        for (int i = 0; i < open.Count; i++)
        {
            if (min > open[i].F)
            {
                min = open[i].F;
                parent = open[i];
            }
        }

        return parent;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartAstartAlgorithm();
     //   Init();
    }
}
