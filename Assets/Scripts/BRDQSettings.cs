using UnityEngine;
using UnityEngine.UI;

namespace BRDQ
{
    public class BRDQSettings : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _BRDQSettings;
        [SerializeField] private Button _BRDQOpen;
        [SerializeField] private Button _BRDQClose;
        
        [SerializeField] private Button _BRDQMusicBtn;
        [SerializeField] private Button _BRDQSoundBtn;

        [SerializeField] private AudioSource _BRDQMusic;
        [SerializeField] private AudioSource _BRDQSound;
        
        [SerializeField] private AudioClip _BRDQClickClip;
        
        [SerializeField] private Sprite _BRDQOn;
        [SerializeField] private Sprite _BRDQOff;

        public void BRDQCLICK() => _BRDQSound.PlayOneShot(_BRDQClickClip);
        
        private void Awake()
        {
            Application.targetFrameRate = 120;
            
            BRDQStaticUtil.BRDQIsPause = false;
            
            _BRDQSettings.BRDQEnableCanvas(false);
            
            _BRDQOpen.onClick.AddListener(() =>
            {
                BRDQStaticUtil.BRDQIsPause = true;
                _BRDQSettings.BRDQEnableCanvas(true);
            });
            _BRDQClose.onClick.AddListener(() =>
            {
                BRDQStaticUtil.BRDQIsPause = false;
                _BRDQSettings.BRDQEnableCanvas(false);
            });

            BRDQMusicControl();
            BRDQSoundControl();
        }

        private void BRDQMusicControl()
        {
            _BRDQMusicBtn.onClick.AddListener(BRDQOnClick);

            var music = PlayerPrefs.GetInt("BRDQMusic", 1);
            
            _BRDQMusicBtn.image.sprite = music == 1 ? _BRDQOn : _BRDQOff;
            _BRDQMusic.mute = music != 1;
            
            return;

            void BRDQOnClick()
            {
                var save = PlayerPrefs.GetInt("BRDQMusic", 1);
                save *= -1;
                PlayerPrefs.SetInt("BRDQMusic", save);
                _BRDQMusicBtn.image.sprite = save == 1 ? _BRDQOn : _BRDQOff;
                _BRDQMusic.mute = save != 1;
            }
        }
        
        private void BRDQSoundControl()
        {
            _BRDQSoundBtn.onClick.AddListener(BRDQOnClick);
            
            var sound = PlayerPrefs.GetInt("BRDQSound", 1);
            
            _BRDQSoundBtn.image.sprite = sound == 1 ? _BRDQOn : _BRDQOff;
            _BRDQSound.mute = sound != 1;
            
            return;
            
            void BRDQOnClick()
            {
                var save = PlayerPrefs.GetInt("BRDQSound", 1);
                save *= -1;
                PlayerPrefs.SetInt("BRDQSound", save);
                _BRDQSoundBtn.image.sprite = save == 1 ? _BRDQOn : _BRDQOff;
                _BRDQSound.mute = save != 1;
            }
        }
    }
}