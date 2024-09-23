using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BRDQ
{
    public class BRDQLoading : MonoBehaviour
    {
        private void Start()
        {
            Invoke(nameof(BRDQLoadMenu), Random.Range(0.25f, 1f));
        }

        private void BRDQLoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}