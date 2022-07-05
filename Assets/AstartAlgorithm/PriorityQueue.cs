using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node
{
    public int nodeId;
    public float F; // 값에 의해 우선순위 결정
    public float G;
    public float sqrH; //Sqr
    public int parent;
    public Vector2 coordinate;

    Vector2 MakeCoordinate(int id)
    {
        if (id == 0)
        {
            return Vector2.zero;
        }

        return new Vector2(UnityEngine.Random.Range(0, 20), UnityEngine.Random.Range(0, 20));
    }

    float GetSqrH(Vector2 current, Vector2 destination)
    {
        if(current == Vector2.zero)
        {
            return 0;
        }

        return MathF.Sqrt(destination.x - current.x) + MathF.Sqrt(destination.y - current.y);
    }
    
    public Node(int nodeId, float g, int parent)
    {
        Vector2 goalCoordinate = new Vector2(30, 30);

        this.nodeId = nodeId;
        this.G = g;
        this.parent = parent;
        this.coordinate = MakeCoordinate(nodeId);
        this.sqrH = GetSqrH(coordinate, goalCoordinate);

        F = G + sqrH;
    }
}

public class PriorityQueue
{

    private List<Node> _priorityQueue = new List<Node>();

    public Node CreateNode(int nodeId, float G, int parent)
    {
        Node node = new Node(nodeId, G, parent);
        return node != null ? node : null;
    }

    public Node Dequeue()
    {
        if (_priorityQueue == null)
        {
            return null;
        }

        if (IsQueueEmpty())
        {
            return null;
        }

        Node node = _priorityQueue[0];
        ReHeapDown();
        return node;
    }

    public void Destory()
    {
        if (IsQueueEmpty())
        {
            return;
        }

        _priorityQueue.Clear();
    }

    public bool IsQueueEmpty()
    {
        return _priorityQueue.Count == 0;
    }

    void ReHeapDown()
    {
        _priorityQueue[0] = _priorityQueue[_priorityQueue.Count - 1];
        _priorityQueue.RemoveAt(_priorityQueue.Count - 1);

        int parent = 0;
        int left, right = 0;

        int child = 0;
        while (true)
        {

            left = parent * 2 + 1;
            right = parent * 2 + 2;

            if (left > _priorityQueue.Count - 1)
            {
                break;
            }

            if (left == _priorityQueue.Count - 1)
            {
                child = left;
            }
            else if (_priorityQueue[left].F < _priorityQueue[right].F)
            {
                child = left;
            }
            else if (_priorityQueue[left].F >= _priorityQueue[right].F)
            {
                child = right;
            }

            if (_priorityQueue[child].F > _priorityQueue[parent].F)
            {
                break;
            }

            Node temp = _priorityQueue[child];
            _priorityQueue[child] = _priorityQueue[parent];
            _priorityQueue[parent] = temp;
            parent = child;
        }
    }

    public void Enqueue(Node node)
    {
        if (_priorityQueue == null || node == null)
        {
            return;
        }

        _priorityQueue.Add(node);
        ReHeapUp(_priorityQueue.Count - 1);

    }

    void ReHeapUp(int child)
    {
        int parent = child / 2;
        while (child > 0)
        {
            parent = child / 2;

            if (_priorityQueue[parent].F < _priorityQueue[child].F)
            {
                break;
            }

            Node temp = _priorityQueue[parent];
            _priorityQueue[parent] = _priorityQueue[child];
            _priorityQueue[child] = temp;
            child = parent;
        }
    }

    void PrintPriorityQueue()
    {
        if (_priorityQueue.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _priorityQueue.Count; i++)
        {
            Debug.Log(_priorityQueue[i].F);
        }
    }
}
