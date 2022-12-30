using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace StumbleGuyz
{
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField] private FormMover _MainFormMover;
        [SerializeField] private Button _GoToMainButton;
        [SerializeField] private Text _PlayerNameText;
        [SerializeField] private Text _CrownCountText;
        [SerializeField] private Text _RankCountText;

        private void Start()
        {
            _GoToMainButton.OnClickAsObservable()
                            .Subscribe((v) => _MainFormMover.Move());
        }

        public void SetPlayerName(string name)
        {
            _PlayerNameText.text = name;
        }

        public void SetCrownCount(string count)
        {
            _CrownCountText.text = count;
        }

        public void SetRankCount(string count)
        {
            _RankCountText.text = count;
        }
    }
}
