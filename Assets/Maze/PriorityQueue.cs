using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Maze
{
    [Serializable]
    public class AStarTileInfo
    {
        public int x;
        public int y;
        public Transform sprite;

        public float F;
        public float G;
        public float H;
        public Vector2 parent;

        public AStarTileInfo(int x, int y, Transform sprite, float H, Vector2 parent, float G = 1)
        {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
            this.G = G;
            this.H = H;

            F = G + H;
            this.parent = parent;
        }
    }

    public class PriorityQueue
    {
        private List<AStarTileInfo> _priorityQueue = new List<AStarTileInfo>();

        public AStarTileInfo CreateNode(int x, int y, Transform sprite, float h, Vector2 parent, float g = 1)
        {
            AStarTileInfo node = new AStarTileInfo(x, y, sprite, h, parent, g);
            return node != null ? node : null;
        }

        public AStarTileInfo Dequeue()
        {
            if (_priorityQueue == null)
            {
                return null;
            }

            if (IsQueueEmpty())
            {
                return null;
            }

            AStarTileInfo node = _priorityQueue[0];
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

        public int Count() => _priorityQueue.Count;

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

                AStarTileInfo temp = _priorityQueue[child];
                _priorityQueue[child] = _priorityQueue[parent];
                _priorityQueue[parent] = temp;
                parent = child;
            }
        }

        public void Enqueue(AStarTileInfo node)
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

                AStarTileInfo temp = _priorityQueue[parent];
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

}
