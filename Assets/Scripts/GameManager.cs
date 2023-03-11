using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public void Awake()
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);
        }
    }
}
