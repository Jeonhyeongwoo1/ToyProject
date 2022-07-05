using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{

    public class MazeTest : MonoBehaviour
    {
        public GameObject tilePrefab;
        public MazeGrid grid;
        
        // Start is called before the first frame update
        void Start()
        {
            grid.InitGrid(25, 25, 1);
            grid.CreateTile(tilePrefab, grid.transform);
            grid.GenerateMaze();
        }

        public void StartAstartAlgorithm()
        {
            grid.starAlgorithm();
        }

    }

}
