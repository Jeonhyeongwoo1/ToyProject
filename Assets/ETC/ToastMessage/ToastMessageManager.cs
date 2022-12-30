using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessageManager : MonoBehaviour
{
    [SerializeField] private ToastMessage _ToastMessagePrefab;
    [SerializeField] private Transform _ToastMessageParent;
    [SerializeField] private List<ToastMessage> _ToastMessageList = new List<ToastMessage>();

    public void Show(string message)
    {
        ToastMessage toastMessage = Instantiate(_ToastMessagePrefab, _ToastMessageParent.position, Quaternion.identity, _ToastMessageParent);
        if (_ToastMessageList.Count > 0)
        {
            _ToastMessageList.ForEach((v) => v.MoveTo());
        }

        _ToastMessageList.Add(toastMessage);
        toastMessage.Init(0, message);
        toastMessage.Show(() => Hide(toastMessage));
    }

    public void Hide(ToastMessage toastMessage)
    {
        _ToastMessageList.Remove(toastMessage);
        toastMessage.Hide();
    }
}