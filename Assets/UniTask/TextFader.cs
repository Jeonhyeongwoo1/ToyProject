using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{
    [SerializeField] private Text[] _taskTexts;
    [SerializeField] private Text _completeTaskText;
    [SerializeField] private Transform _TargetTransform;
    
    [SerializeField] private Button _Button;

    private async void Start()
    {
        await UniTask.WhenAll(_TargetTransform.DOScale(Vector3.one, 3).WithCancellation(this.GetCancellationTokenOnDestroy()),
                             _TargetTransform.DOMove(Vector3.one * 50, 3).WithCancellation(this.GetCancellationTokenOnDestroy()));

        await UniTask.WhenAll(FadeTasks(this.GetCancellationTokenOnDestroy()));
        _completeTaskText.DOFade(1.0f, 2.0f).Play();
    }

    private List<UniTask> FadeTasks(CancellationToken ct)
    {
        var taskList = new List<UniTask>();

        foreach (var taskText in _taskTexts)
        {
            var randomValue = Random.Range(1.0f, 5.0f);
            taskList.Add(taskText.DOFade(1.0f, randomValue).Play().ToUniTask(cancellationToken: ct));
        }

        return taskList;
    }
}
