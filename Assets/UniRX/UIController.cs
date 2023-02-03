using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Threading;
using System;

namespace UniRxExample
{

    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button clickButton;
        [SerializeField] private Text clickText;
        [SerializeField] private Text subText;

        private IntReactiveProperty count = new IntReactiveProperty(10);
        private ReactiveProperty<int> intCount = new ReactiveProperty<int>();

        private IDisposable Subscribe(IObserver<string> observer)
        {
            observer.OnNext("Test");
            observer.OnNext("Custom");
            observer.OnCompleted();

            return null;
        }

        private void Test()
        {
            // clickButton.OnClickAsObservable()
            //             .Subscribe((v) => clickText.text = "Clicked");

            clickButton.OnClickAsObservable()
                        .SubscribeToText(clickText, (v) => "Clicked");
            // clickButton.onClick.AsObservable()
            //             .Subscribe((v) => clickText.text = "Clicked");


            var stream = Observable.Create<string>(Subscribe);

            stream.Subscribe((v) => Debug.Log(v), (v) => Debug.Log(v), () => Debug.Log("OnComplete"));


            Debug.Log(DateTime.Now);

            var time = Observable.Return(DateTime.Now);
            Observable.Empty<Unit>()
                        .Delay(TimeSpan.FromSeconds(3))
                        .Subscribe((_) => time.Subscribe((v) => Debug.Log(v)));

        }

        private void ObservableTime()
        {
            Observable.Empty<Unit>()
                               .Subscribe((v) => Debug.Log("OnNext"), () => Debug.Log("OnComplete"));

            Observable.Return(3)
                        .Delay(TimeSpan.FromSeconds(3))
                        .Subscribe((v) => Debug.Log("Value :" + v));

            var interval = Observable.Interval(TimeSpan.FromSeconds(1))
                                        .Subscribe((v) => Debug.Log("Interval :" + v), () => Debug.Log("Interval complete"));

            Observable.Timer(TimeSpan.FromSeconds(2))
                        .Subscribe((v) => Debug.Log("OnTimeout" + DateTime.Now));


            Observable.Timer(TimeSpan.FromSeconds(2))
                        .Subscribe((v) => interval.Dispose());

            // Observable.EveryUpdate()
            //             .Subscribe((v)=> Debug.Log("EveryUpdate"));

            // Observable.EveryFixedUpdate()
            //             .Subscribe((v)=> Debug.Log(v));


            Debug.Log($"Frame : {Time.frameCount}");
            Observable.Start(() =>
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                MainThreadDispatcher.Post(_ => Debug.Log($"Frame : {Time.frameCount}"), new object());
                return Thread.CurrentThread.ManagedThreadId;
            })
                .Subscribe(
                    id => Debug.Log($"Finished : {id}"),
                    err => Debug.Log(err)
                );
        }

        private void Subject()
        {
            Subject<string> subject = new Subject<string>();
            subject.Subscribe((v) => Debug.Log("Message1 +" + v));
            subject.Subscribe((v) => Debug.Log("Message2 +" + v));

            subject.OnNext("Hi");
            subject.OnNext("complete");
            subject.OnCompleted();
            subject.OnNext("Not call");
        }

        [SerializeField] private List<Vector2> moveList;

        private void Start()
        {
            Debug.Log(DateTime.Now);
            //ObservableTime();
            //Subject();

            // Observable.FromCoroutineValue<Vector2>(MoveCor)
            //             .Subscribe((v) => Debug.Log(v), () => Debug.Log("Complete"));


            count.Value = 5;
            intCount.Value = 10;
        }

        private IEnumerator MoveCor()
        {
            foreach (var v in moveList)
            {
                yield return v;
            }

            // yield return moveList.GetEnumerator();
        }
    }
}