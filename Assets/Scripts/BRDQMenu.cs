using UnityEngine;
using UnityEngine.UI;

namespace BRDQ
{
    public class BRDQMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _BRDQMenu;
        [SerializeField] private CanvasGroup _BRDQLevels;
        [SerializeField] private CanvasGroup _BRDQExit;
        
        [Space]
        [SerializeField] private Button _BRDQLevelsBtn;
        [SerializeField] private Button _BRDQExitBtn;
        
        [Space]
        [SerializeField] private Button[] _BRDQBacksBtn;
        
        [Space]
        [SerializeField] private Button _BRDQQuitBtn;

        private void Awake()
        {
            _BRDQMenu.BRDQEnableCanvas(true);
            _BRDQLevels.BRDQEnableCanvas(false);
            _BRDQExit.BRDQEnableCanvas(false);
            
            _BRDQLevelsBtn.onClick.AddListener(() =>
            {
                _BRDQMenu.BRDQEnableCanvas(false);
                _BRDQLevels.BRDQEnableCanvas(true);
            });
            
            _BRDQExitBtn.onClick.AddListener(() =>
            {
                _BRDQMenu.BRDQEnableCanvas(false);
                _BRDQExit.BRDQEnableCanvas(true);
            });

            foreach (var brdqBtn in _BRDQBacksBtn)
            {
                brdqBtn.onClick.AddListener(() =>
                {
                    _BRDQMenu.BRDQEnableCanvas(true);
                    _BRDQLevels.BRDQEnableCanvas(false);
                    _BRDQExit.BRDQEnableCanvas(false);
                });
            }
            
            _BRDQQuitBtn.onClick.AddListener(Application.Quit);
        }
    }
}