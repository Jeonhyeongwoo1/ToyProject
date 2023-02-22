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

        public GameObject marker;
        private List<GameObject> markerList = new List<GameObject>();

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
            if (Input.GetMouseButtonDown(0))
            {
                ShowSimulate();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowRealMove();
            }

            mainScene.GetPhysicsScene2D().Simulate(Time.fixedDeltaTime);
        }

        private void ShowRealMove()
        {
            GameObject racer = Instantiate(_racerPrefab);
            racer.GetComponent<Rigidbody2D>().gravityScale = 1;
        }

        private void PreparePhysicsScene()
        {
            SceneManager.SetActiveScene(physicsScene);

            racerInPhyiscsScene = Instantiate(_racerPrefab);
            wallInPhysicsScene = Instantiate(_entityPrefab);

            SceneManager.SetActiveScene(mainScene);
        }

        public void CreateMovementMarkers()
        {
            GameObject go = GameObject.Instantiate(marker, racerInPhyiscsScene.transform.position, Quaternion.identity);
            go.transform.localScale = Vector3.one * 0.1f;
            markerList.Add(go);
        }

        public void ShowSimulate()
        {
            //racerInPhyiscsScene.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 30);
            racerInPhyiscsScene.GetComponent<Rigidbody2D>().gravityScale = 1;
            Transform rotater = wallInPhysicsScene.transform.Find("CrossRevolvingDoor");
            rotater.GetComponent<Rotater>().DoRotate();
            int step = _simulateCount;
            for (int i = 0; i < step; i++)
            {
                physicsScene.GetPhysicsScene2D().Simulate(Time.fixedDeltaTime);
                //타겟이 되는 목표물들에 대해서 이동을 시키자.
                //물리 연산을 할 때 많이 사용할 수 있을 듯하다.


                //racerInPhyiscsScene.transform.position += new Vector3(0, 1, 0);
                CreateMovementMarkers();
            }


            //  Physics.autoSimulation = true;

        }
    }
}