using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum HeartShowType
{
    Center,
    Background
}

public class HeartManager : MonoBehaviour, IVisible
{
    [System.Serializable]
    public class RandomRange
    {
        public float min;
        public float max;
    }

    [System.Serializable]
    public class CenterTypeHeartData
    {
        public RandomRange canvasAlphaRandomRangeValue;
        public RandomRange axisYRandomRangeValue;
        public RandomRange axisXRandomRangeValue;
        public RandomRange createHeartRandomRangeValue;
        public float delayTime = 1f;
        public float duration = 3f;
    }

    [System.Serializable]
    public class BackgroundTypeHeartData
    {
        public RandomRange startAxisXRandomRangeValue;
        public RandomRange scaleRandomRangeValue;
        public RandomRange createHeartRandomRangeValue;
        public RandomRange canvasAlphaRandomRangeValue;
        public RandomRange axisYRandomRangeValue;
        public RandomRange axisXRandomRangeValue;
        public float delayTime = 1f;
        public float duration = 3f;
        public float startAixsY;
    }

    [SerializeField] private HeartShowType _HeartShowType;
    [SerializeField] private Heart _HeartPrefab;
    [SerializeField] private Transform _HeartParent;
    [SerializeField] private CenterTypeHeartData _CenterTypeHeartData;
    [SerializeField] private BackgroundTypeHeartData _BackgroundTypeHeartData;
    [SerializeField] private List<Heart> _OpenedHeartImageList = new List<Heart>();

    private void Start()
    {
        Show();
    }

    public void Show(UnityAction done = null)
    {
        StartCoroutine(ShowHeart());

        done?.Invoke();
    }

    private void ShowCenterTypeHeart(CenterTypeHeartData data)
    {
        int count = Random.Range((int)data.createHeartRandomRangeValue.min, (int)data.createHeartRandomRangeValue.max);
        for (int i = 0; i < count; i++)
        {
            Heart heart = Instantiate(_HeartPrefab, Vector3.one, Quaternion.identity, _HeartParent);
            _OpenedHeartImageList.Add(heart);
            heart.name = heart + _OpenedHeartImageList.Count.ToString();
            heart.Init(Random.Range(data.canvasAlphaRandomRangeValue.min, data.canvasAlphaRandomRangeValue.max), 1, false);

            Vector3 endPos = _HeartParent.transform.position + new Vector3(Random.Range(data.axisXRandomRangeValue.min, data.axisXRandomRangeValue.max)
                                                                            , Random.Range(data.axisYRandomRangeValue.min, data.axisYRandomRangeValue.max)
                                                                            , 0);
            heart.Play(_HeartParent.position, endPos, data.duration);
        }
    }

    private void ShowBackgroundTypeHeart(BackgroundTypeHeartData data)
    {
        int count = Random.Range((int)data.createHeartRandomRangeValue.min, (int)data.createHeartRandomRangeValue.max);
        for (int i = 0; i < count; i++)
        {
            Heart heart = Instantiate(_HeartPrefab, Vector3.one, Quaternion.identity, _HeartParent);
            _OpenedHeartImageList.Add(heart);
            heart.name = heart + _OpenedHeartImageList.Count.ToString();
            float scale = Random.Range(data.scaleRandomRangeValue.min, data.scaleRandomRangeValue.max);
            heart.Init(Random.Range(data.canvasAlphaRandomRangeValue.min, data.canvasAlphaRandomRangeValue.max), scale, true);

            Vector3 randomValue = new Vector3(Random.Range(data.axisXRandomRangeValue.min, data.axisXRandomRangeValue.max)
                                                                            , Random.Range(data.axisYRandomRangeValue.min, data.axisYRandomRangeValue.max)
                                                                            , 0);
            Vector3 startPos = transform.position + new Vector3(randomValue.x, data.startAixsY, 0);// + new Vector3(randomValue.x + Random.Range(data.startAxisXRandomRangeValue.min, data.startAxisXRandomRangeValue.max), data.startAixsY);
            Vector3 endPos = transform.position + randomValue;
            heart.Play(startPos, endPos, data.duration);
        }
    }

    private IEnumerator ShowHeart()
    {
        while (true)
        {
            switch (_HeartShowType)
            {
                case HeartShowType.Center:
                    CenterTypeHeartData centerTypeHeartData = _CenterTypeHeartData;
                    ShowCenterTypeHeart(centerTypeHeartData);
                    yield return new WaitForSeconds(centerTypeHeartData.delayTime);
                    break;
                case HeartShowType.Background:
                    BackgroundTypeHeartData backgroundTypeHeartData = _BackgroundTypeHeartData;
                    ShowBackgroundTypeHeart(backgroundTypeHeartData);
                    yield return new WaitForSeconds(backgroundTypeHeartData.delayTime);
                    break;
            }

        }
    }

    public void Hide(UnityAction done = null)
    {
        StopAllCoroutines();
    }

    [ContextMenu("Show")]
    public void Show()
    {
        StartCoroutine(ShowHeart());
    }
}
