using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace PhysicsSimulation
{
    public class PhysicsSimulate : MonoBehaviour
    {
        [SerializeField] private int _simulateCount;
        [SerializeField] private GameObject _racerPrefab;
        [SerializeField] private GameObject _entityPrefab;
        [SerializeField] private Map _map;
        [SerializeField] private int _racerCount;

        public GameObject marker;
        private List<GameObject> markerList = new List<GameObject>();
        private List<GameObject> realRacerList = new List<GameObject>();
        private List<GameObject> hiddenRacerList = new List<GameObject>();

        private Scene mainScene;
        private Scene physicsScene;
        private GameObject racerInPhyiscsScene;
        private GameObject wallInPhysicsScene;

        private void Start()
        {
            Physics.autoSimulation = false;

            mainScene = SceneManager.GetActiveScene();
            physicsScene = SceneManager.CreateScene("Physics-Scene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));

            PreparePhysicsScene();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowSimulate();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowRealMove();
            }

            // mainScene.GetPhysicsScene2D().Simulate(Time.fixedDeltaTime);
        }

        private void ShowRealMove()
        {
            for (int i = 0; i < _racerCount; i++)
            {
                GameObject racer = Instantiate(_racerPrefab);
                racer.GetComponent<Rigidbody2D>().gravityScale = 1;
                racer.transform.localPosition += new Vector3(2 + i * 2, 0, 0);
            }
            _map.Rotater.isReal = true;
        }

        private void PreparePhysicsScene()
        {
            SceneManager.SetActiveScene(physicsScene);

            for (int i = 0; i < _racerCount; i++)
            {
                GameObject racer = Instantiate(_racerPrefab);
                racer.transform.localPosition += new Vector3(2 + i * 2, 0, 0);
                hiddenRacerList.Add(racer);
            }

            //racerInPhyiscsScene = Instantiate(_racerPrefab);
            
            wallInPhysicsScene = Instantiate(_entityPrefab);

            SceneManager.SetActiveScene(mainScene);
        }

        public void CreateMovementMarkers()
        {
            for (int i = 0; i < hiddenRacerList.Count; i++)
            {
                GameObject go = GameObject.Instantiate(marker, hiddenRacerList[i].transform.position, Quaternion.identity);
                go.transform.localScale = Vector3.one * 0.1f;
                markerList.Add(go);
            }

            // GameObject go = GameObject.Instantiate(marker, racerInPhyiscsScene.transform.position, Quaternion.identity);
            // go.transform.localScale = Vector3.one * 0.1f;
            // markerList.Add(go);
        }

        public void ShowSimulate()
        {
            //여기서는 한번만

            for(int i = 0; i < hiddenRacerList.Count; i++)
            {
                hiddenRacerList[i].GetComponent<Rigidbody2D>().gravityScale = 1;
            }

            //racerInPhyiscsScene.GetComponent<Rigidbody2D>().gravityScale = 1;
            Rotater rotator = wallInPhysicsScene.GetComponent<Map>().Rotater;
            int step = _simulateCount; //(int)(2 / Time.fixedDeltaTime);
            for (int i = 0; i < step; i++)
            {
                physicsScene.GetPhysicsScene2D().Simulate(Time.fixedDeltaTime);
                //타겟이 되는 목표물들에 대해서 이동을 시키자.
                //물리 연산을 할 때 많이 사용할 수 있을 듯하다.
                rotator.DoRotate();
                //racerInPhyiscsScene.transform.position += new Vector3(0, 1, 0);
                CreateMovementMarkers();
            }
        }
    }
}