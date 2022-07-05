using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{

    public class MazeGrid : MonoBehaviour
    {
        public int _lastX;
        public int _lastY;
        public enum TileType { Empty, Wall, Visited }

        int _width;
        int _height;
        int _cellSize;
        int _size;

        Transform[,] _gridObject;
        TileType[,] _gridType;
        int[,] _gridArray;

        int _startX = 1;
        int _startY = 1;

        public void InitGrid(int width, int height, int cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _size = _width * _height;
            _gridArray = new int[width, height];
            _gridObject = new Transform[width, height];
            _gridType = new TileType[width, height];
        }

        public void CreateTile(GameObject tile, Transform parent)
        {

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    GameObject go = Instantiate(tile, GetXY(_cellSize, x, y), Quaternion.identity, parent);
                    _gridObject[y, x] = go.transform;
                    _gridType[y, x] = TileType.Empty;
                }
            }
        }

        /*
            Maze 설계
            1. 메이즈는 홀수 배열이여야한다.
            2. 가장자리를 모두 벽으로 변경한다.
            3. 홀수를 제외한 나머지는 모두 벽으로 만든다.
            4. 홀수번호부터 시작해서 (0, 2) 랜덤으로 0일 경우에는 짝수번호 1일경우네는 홀수번호로 인덱스를 옮겨가면서 empty를 만든다
            5. count - 2번 쨰는 벽 바로 이전단계이기 때문에 empty로 만든다.
    
            Empty, WALL 두 가지 형태로 만든다.        
        */

        /*
            DFS 탐색 방법(모든 것을 다 탐색)
            1. Start 노드를 미리 정해둔다.
            2. 첫 번쨰 노드를 찾는다.
            3. 구조체를 만든다, 좌표 객체 방문가능 등
            4. 스택을 만든다. 스택에는 구조체가 들어가 있다.
            5. 첫번쨰 시작 노드값을 스택에 넣고 스택이 빈값이 될때까지 줄인다
            6. 갈 수 있는 노드들은 4개뿐이고 반복문을 사용해서 좌우위아래에 중에서 갈 수 있는 노드들을 탐색하여 스택에 넣는다. 갈수없는 노드들은 visited true로 변경한다.
            7. 스택에서 다시 팝해서 그 노드에서 갈 수 있는 곳들을 순회한다.

            방문할 수 있는 노드들에 대한 스택(배열 값이 들어가야한다 )
            방문한 노드들에 대한 리스트
            
            스택이 empty가 될 떄까지 돌린다
            방문한 노드들에 대해서는 파란색으로 표시한다.
        */

        /*
            A* star 알고리즘은 가중치가 있는 알고리즘이다. 
            
            1. 스타트 노드와 목표 노드를 미리 정해둔다.
            2. 총 노드 수를 먼저 구한다.
            3. 스타트 노드부터 시작해서 연결되어 있는 노드들을 Open 리스트에 저장한다. 이 떄 저장되어야할 값은 노드 아이디, F,G,H, 부모 노드
            4. 스타트 노드의 연결되어있는 노드들을 구했으면 Close에 저장한다.
            5. Open에 있는 리스트들 중 최소비용 F값인 것 부터 다시탐색한다.
            6. 연결된 노드들에 대해서 다시 Open리스트에 저장한다. 이 때 기존 값이 있는 경우에는 값을 비교하여 더 적은 비용이 드는 것을 저장한다.
            7. 위의 것을 반복한다.
            8. Close에 목표 노드가 나오면 종료한다.

            1. 구조체를 사용한다. node id, f, g, h, parent
            2. 최소 힙을 사용한다.
            3. 휴리스틱 (H) 값을 피타고라스 정의를 이용해서 빗변을 구한다. 이 때 노드 한개당 1,1이므로 값을 한 변의 길이는 1이다. 따라서 (1,3) (2,4) => 2 대신 제곱근을 사용해서 하기
            4. G값은 1이다.

        */

        public List<AStarTileInfo> open = new List<AStarTileInfo>();
        public List<AStarTileInfo> close = new List<AStarTileInfo>();
        private PriorityQueue pq = new PriorityQueue();
   
        float GetSqrH(int x, int y, int dy, int dx)
        {
            return Mathf.Sqrt(Mathf.Pow(dx - x, 2) + Mathf.Pow(dy - y, 2));
        }

        public void starAlgorithm()
        {
            StartCoroutine(StarAlgorithm());
        }

        IEnumerator StarAlgorithm()
        {
            if(_lastX == 0 && _lastY == 0)
            {
                yield break;
            }

            if(_gridType[_lastY, _lastX] != TileType.Empty)
            {
                yield break;
            }

            AStarTileInfo info = new AStarTileInfo(_startX, _startY, _gridObject[_startY, _startX], 0, Vector2.zero, 0);

            pq.Enqueue(info);
            open.Add(info);

            while (!pq.IsQueueEmpty())
            {
                AStarTileInfo tile = pq.Dequeue();

                

                if (_gridType[tile.y + 1, tile.x] == TileType.Empty)
                {
                    CheckTileInfoClose(tile.x, tile.y + 1, tile.x, tile.y, tile.G);
                }

                if (_gridType[tile.y - 1, tile.x] == TileType.Empty)
                {
                    CheckTileInfoClose(tile.x, tile.y - 1, tile.x, tile.y, tile.G);
                }

                if (_gridType[tile.y, tile.x + 1] == TileType.Empty)
                {
                    CheckTileInfoClose(tile.x + 1, tile.y, tile.x, tile.y, tile.G);
                }

                if (_gridType[tile.y, tile.x - 1] == TileType.Empty)
                {
                    CheckTileInfoClose(tile.x - 1, tile.y, tile.x, tile.y, tile.G);
                }

                open.Remove(tile);
                close.Add(tile);

                if (tile.x == _lastX && tile.y == _lastY)
                {
                    AStarTileInfo starTileInfo = close[close.Count - 1];
                    Vector2 p = new Vector2(starTileInfo.x, starTileInfo.y);
                    while(p != Vector2.zero)
                    {
                        starTileInfo.sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                        p = starTileInfo.parent;
                        starTileInfo = close.Find((v)=> v.x == p.x && v.y == p.y);   
                        yield return null; 
                    }

                    yield break;
                }

                yield return null;
            }
        }

        void CheckTileInfoClose(int x, int y, int parentX, int parentY, float G)
        {
            AStarTileInfo sameClose = close.Find((v) => v.x == x && v.y == y);
            if (sameClose == null)
            {
                AddInfo(x, y, parentX, parentY, G);
            }
        }

        void AddInfo(int x, int y, int parentX, int parentY, float G)
        {
            AStarTileInfo newTile = new AStarTileInfo(x, y, _gridObject[y, x], GetSqrH(x, y, _lastY, _lastX), new Vector2(parentX, parentY), 1 + G);
            AStarTileInfo sameOpen = open.Find((v) => v.x == x && v.y == y);
            if (sameOpen != null)
            {
                if (sameOpen.F > newTile.F)
                {
                    sameOpen = newTile;
                }
            }

            pq.Enqueue(newTile);
            open.Add(newTile);
        }


        public struct VisitableTile
        {
            public int x;
            public int y;
            public Transform sprite;
            public bool visited;

            public VisitableTile(int x, int y, Transform sprite, bool visited)
            {
                this.x = x;
                this.y = y;
                this.sprite = sprite;
                this.visited = visited;
            }

        }

        private Stack<VisitableTile> visitableTiles = new Stack<VisitableTile>();
        private int[,] visited;

        public void StartDFS()
        {
            if (_gridObject.Length == 0)
            {
                return;
            }

            visited = new int[_width, _height];
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    visited[j, i] = 0;
                }
            }

            StartCoroutine(DFS());
            //       DFS();
        }

        IEnumerator DFS()
        {
            VisitableTile startTile = new VisitableTile(_startX, _startY, _gridObject[_startY, _startX], true);
            startTile.visited = true;
            visited[_startY, _startX] = 1;
            visitableTiles.Push(startTile);

            while (visitableTiles.Count != 0)
            {
                VisitableTile tile = visitableTiles.Pop();
                if (tile.Equals(null))
                {
                    break;
                }

                tile.sprite.GetComponent<SpriteRenderer>().color = Color.blue;

                //right
                if (_gridType[tile.x, tile.y + 1] == TileType.Empty && visited[tile.x, tile.y + 1] == 0)
                {
                    visitableTiles.Push(new VisitableTile(tile.x, tile.y + 1, _gridObject[tile.x, tile.y + 1], true));
                    visited[tile.x, tile.y + 1] = 1;
                }
                else
                {
                    visited[tile.x, tile.y + 1] = 1;
                }

                //left
                if (_gridType[tile.x, tile.y - 1] == TileType.Empty && visited[tile.x, tile.y - 1] == 0)
                {
                    visitableTiles.Push(new VisitableTile(tile.x, tile.y - 1, _gridObject[tile.x, tile.y - 1], true));
                    visited[tile.x, tile.y - 1] = 1;
                }
                else
                {
                    visited[tile.x, tile.y - 1] = 1;
                }

                //up
                if (_gridType[tile.x + 1, tile.y] == TileType.Empty && visited[tile.x + 1, tile.y] == 0)
                {
                    visitableTiles.Push(new VisitableTile(tile.x + 1, tile.y, _gridObject[tile.x + 1, tile.y], true));
                    visited[tile.x + 1, tile.y] = 1;
                }
                else
                {
                    visited[tile.x + 1, tile.y] = 1;
                }

                //Down
                if (_gridType[tile.x - 1, tile.y] == TileType.Empty && visited[tile.x - 1, tile.y] == 0)
                {
                    visitableTiles.Push(new VisitableTile(tile.x - 1, tile.y, _gridObject[tile.x - 1, tile.y], true));
                    visited[tile.x - 1, tile.y] = 1;
                }
                else
                {
                    visited[tile.x - 1, tile.y] = 1;
                }

                yield return null; 
            }

        }

        public void GenerateMaze()
        {
            if (_size % 2 == 0)
            {
                return;
            }

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        _gridType[y, x] = TileType.Wall;
                        _gridObject[y, x].GetComponent<SpriteRenderer>().color = Color.red;
                        continue;
                    }

                    _gridType[y, x] = TileType.Empty;
                    _gridObject[y, x].GetComponent<SpriteRenderer>().color = Color.green;
                }
            }


            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        continue;
                    }

                    if (x == _width - 2 && y == _height - 2)
                    {
                        continue;
                    }

                    if (y == _height - 2)
                    {
                        _gridObject[y, x + 1].GetComponent<SpriteRenderer>().color = Color.green;
                        _gridType[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == _width - 2)
                    {
                        _gridObject[y + 1, x].GetComponent<SpriteRenderer>().color = Color.green;
                        _gridType[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    int value = Random.Range(0, 2);
                    if (value == 0)
                    {
                        //짝수
                        _gridObject[y, x + 1].GetComponent<SpriteRenderer>().color = Color.green;
                        _gridType[y, x + 1] = TileType.Empty;
                    }
                    else if (value == 1)
                    {
                        _gridObject[y + 1, x].GetComponent<SpriteRenderer>().color = Color.green;
                        _gridType[y + 1, x] = TileType.Empty;
                    }
                }
            }
        }

        private Vector2 GetXY(int cellSize, int x, int y)
        {
            return new Vector2(x * cellSize, y * cellSize);
        }

    }

}
