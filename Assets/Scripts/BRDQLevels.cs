using UnityEngine;
using UnityEngine.SceneManagement;

namespace BRDQ
{
    public class BRDQLevels : MonoBehaviour
    {
        [SerializeField] private BRDQLevelBtn[] _BRDQLevels;

        private void Start()
        {
            var brdqLevelPass = PlayerPrefs.GetInt("BRDQLevelPass", 1);
            
            for (var i = 0; i < _BRDQLevels.Length; i++)
            {
                var index = i;

                var brdqIsInteract = index < brdqLevelPass;
                
                _BRDQLevels[index].interactable = brdqIsInteract;
                _BRDQLevels[index]._BRDQLevel.text = $"Level {index + 1}";
                var brdqLevelColor = _BRDQLevels[index]._BRDQLevel.color;
                brdqLevelColor.a = brdqIsInteract ? 1f : 0.5f;
                _BRDQLevels[index]._BRDQLevel.color = brdqLevelColor;
                
                _BRDQLevels[index].onClick.AddListener(() =>
                {
                    PlayerPrefs.SetInt("BRDQLevel", index + 1);
                    SceneManager.LoadScene("Game");
                });
            }
        }
    }
}