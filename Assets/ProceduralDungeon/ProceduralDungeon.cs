using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ProceduralDungeon : MonoBehaviour
{

    /*
        1. BSP는 바이너리 공간 파티션이며 이진트리 형식으로 분할해서 구현한다.
        2. 이진 트리형식의 클래스를 구현해야 하며 1. 공간 분할 2. 방 생성 3. 길 생성 순으로 진행된다.
        

        RectInt = x, y, width, height
        좌표는 (x, y)/(x + width, y)/(x + width, y + height)/(x, y + height)

        공간 분할
        1. 재귀를 사용한다.
        2. 최대 노드 수 만큼 분할해야 한다.
        3. Width, height값을 이용해서 값이 더 큰 쪽으로 분할을 한다.
        4. 분할할 때 랜덤값을 줘서 분할한다.
        5. 분할이 완료되면 라인으로 경계선을 만든다.

        방 생성
        1. 재귀를 사용한다.
        2. 이미 분할된 노드들을 이용해서 해당 노드에서 랜덤하게 방 사이즈를 구한다.
        3. 해당 방사이즈 크기로 방을 만든다.

        길 생성
        1. 재귀를 사용한다.
        2. 분할된 방을 찾아 양 노드들을 연결한다.

        이진 트리 클래스
        1. 트리
        2. 노드 수
        3. 맵 크기

        노드
        1. 왼쪽, 오른쪽 노드
        2. 공간 Rect
        3. 룸 Rect
    */

    [Serializable]
    public class DungeonTree
    {
        public Node root;
        public int nodeCount;
        public Vector2Int mapSize;
    }

    [Serializable]
    public class Node
    {
        public Node left;
        public Node right;

        public RectInt nodeSize;
        public RectInt roomSize;
        public Vector2 roomCenterPosition;

        public Node(int x, int y, int width, int height)
        {
            nodeSize.x = x;
            nodeSize.y = y;
            nodeSize.width = width;
            nodeSize.height = height;
        }
    }

    public int maxNode = 3;
    [Range(0, 1)] public float minDivideRatio = 0.4f;
    [Range(0, 1)] public float maxDivideRatio = 0.6f;
    public DungeonTree dungeonTree;
    public LineRenderer linePrefab;
    public LineRenderer rectanglePrefab;
    public LineRenderer roomPrefab;
    public Transform dungeonParent;

    public void DivideTree(Node node, int n)
    {

        if (n == maxNode) { return; }

        RectInt rect = node.nodeSize;
        int length = rect.width >= rect.height ? rect.width : rect.height;
        int split = (int)UnityEngine.Random.Range(length * minDivideRatio, length * maxDivideRatio);

        if (rect.width > rect.height)
        {
            Node left = new Node(rect.x, rect.y, split, rect.height);
            Node right = new Node(rect.x + split, rect.y, rect.width - split, rect.height);
            node.left = left;
            node.right = right;
            DrawLine(new Vector2(rect.x + split, rect.y), new Vector2(rect.x + split, rect.y + rect.height));
        }
        else
        {
            Node left = new Node(rect.x, rect.y, rect.width, split);
            Node right = new Node(rect.x, rect.y + split, rect.width, rect.height - split);
            node.left = left;
            node.right = right;
            DrawLine(new Vector2(rect.x, rect.y + split), new Vector2(rect.width + rect.x, rect.y + split));
        }       

        dungeonTree.nodeCount++;
        DivideTree(node.left, n + 1);
        DivideTree(node.right, n + 1);
    }

    public RectInt GenerateDungeon(Node node, int n)
    {
        if(maxNode == n)
        {
            RectInt rect = node.nodeSize;
            int width = UnityEngine.Random.Range(rect.width / 2, rect.width - 1);
            int height = UnityEngine.Random.Range(rect.height / 2, rect.height - 1);

            int x = rect.x + UnityEngine.Random.Range(1, (rect.width - width));
            int y = rect.y + UnityEngine.Random.Range(1, (rect.height - height));
            int centerX = x + width / 2;
            int centerY = y + height / 2;
            DrawRectangle(x, y, width, height, roomPrefab);
            return new RectInt(x, y, width, height);
        }

        node.left.roomSize = GenerateDungeon(node.left, n + 1);
        node.right.roomSize = GenerateDungeon(node.right, n + 1);
        return node.left.roomSize;
    }

    public void DrawLoad(Node node, int n)
    {
        if(n == maxNode)
        {
            return;
        }

        RectInt left = node.left.roomSize;
        RectInt right = node.right.roomSize;

        int lx = left.x + left.width / 2;
        int ly = left.y + left.height / 2;
        int rx = right.x + right.width / 2;
        int ry = right.y + right.height / 2;

        DrawLine(new Vector2(lx, ly), new Vector2(rx, ry));
        DrawLoad(node.left, n + 1);
        DrawLoad(node.right, n + 1);
    }


    private void DrawRectangle(int x, int y, int width, int height, LineRenderer prefab)
    {
        LineRenderer lineRenderer = Instantiate<LineRenderer>(prefab, dungeonParent);
        lineRenderer.SetPosition(0, new Vector2(x, y));
        lineRenderer.SetPosition(1, new Vector2(x + width, y));
        lineRenderer.SetPosition(2, new Vector2(x + width, y + height));
        lineRenderer.SetPosition(3, new Vector2(x, y + height));
    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate<LineRenderer>(linePrefab, dungeonParent);
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    IEnumerator StartDungeon()
    {
        DrawRectangle(0, 0, dungeonTree.mapSize.x, dungeonTree.mapSize.y, rectanglePrefab);
        DivideTree(dungeonTree.root, 0);
        yield return new WaitForSeconds(1f); 
        dungeonTree.root.roomSize = GenerateDungeon(dungeonTree.root, 0);
        yield return new WaitForSeconds(1f); 
        DrawLoad(dungeonTree.root, 0);
    }

    void Start()
    {
       StartCoroutine(StartDungeon());
    }

}
