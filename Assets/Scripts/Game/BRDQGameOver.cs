using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BRDQ
{
    public class BRDQGameOver : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _BRDQGame;
        [SerializeField] private CanvasGroup _BRDQWin;
        [SerializeField] private CanvasGroup _BRDQLose;
        
        [SerializeField] private List<Button> _BRDQHome;
        [SerializeField] private Button[] _BRDQRestart;
        
        [SerializeField] private TMP_Text[] _BRDQLevel;
        
        [SerializeField] private TMP_Text _BRDQTimer;
        
        [Space]
        public AudioClip _BRDQWinClip;
        public AudioClip _BRDQLoseClip;
        public AudioSource _BRDQSfx;
        public AudioSource _BRDQMusic;

        private float _brdqTimer = 60f;
        
        private int _brdqLevel;

        private void Start()
        {
            _BRDQTimer.text = _brdqTimer.BRDQToTimeString();

            foreach (var brdqLvl in _BRDQLevel)
            {
                _brdqLevel = PlayerPrefs.GetInt("BRDQLevel", 1);
                brdqLvl.text = $"Level {_brdqLevel}";
            }
            
            _BRDQGame.BRDQEnableCanvas(true);
            _BRDQWin.BRDQEnableCanvas(false);
            _BRDQLose.BRDQEnableCanvas(false);

            foreach (var brdqButton in _BRDQRestart)
                brdqButton.onClick.AddListener(()=> SceneManager.LoadScene("Game"));
            
            foreach (var brdqButton in _BRDQHome)
                brdqButton.onClick.AddListener(() =>
                {
                    BRDQStaticUtil.BRDQIsGameOver = true;

                    SceneManager.LoadScene("Menu");
                });
            
            BRDQStaticUtil.BRDQIsGameOver = false;
        }

        public void BRDQWin()
        {
            BRDQStaticUtil.BRDQIsGameOver = true;

            _BRDQMusic.DOFade(0.1f, 0.1f).SetEase(Ease.Flash);
            _BRDQSfx.PlayOneShot(_BRDQWinClip);
            
            _BRDQGame.interactable = false;

            _BRDQWin.DOFade(1f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                _BRDQWin.BRDQEnableCanvas(true);
            });

            _brdqLevel++;
            
            var lvlsPass = PlayerPrefs.GetInt("BRDQLevelPass", 1);
            if (lvlsPass < _brdqLevel)
                PlayerPrefs.SetInt("BRDQLevelPass", _brdqLevel);
            
            PlayerPrefs.SetInt("BRDQLevel", Mathf.Clamp(_brdqLevel, 1, 99));
        }
        
        private void BRDQLose()
        {
            BRDQStaticUtil.BRDQIsGameOver = true;
            
            _BRDQMusic.DOFade(0.1f, 0.1f).SetEase(Ease.Flash);
            _BRDQSfx.PlayOneShot(_BRDQLoseClip);
            
            _BRDQGame.interactable = false;
            
            _BRDQLose.DOFade(1f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                _BRDQLose.BRDQEnableCanvas(true);
            });
        }
        
        private void Update()
        {
            if (BRDQStaticUtil.BRDQStopUpdate)
                return;
            
            _brdqTimer -= Time.deltaTime;

            _BRDQTimer.text = _brdqTimer.BRDQToTimeString();

            if (_brdqTimer > 0) 
                return;
            
            _BRDQTimer.text = _brdqTimer.BRDQToTimeString();
            
            BRDQLose();
        }
    }
}